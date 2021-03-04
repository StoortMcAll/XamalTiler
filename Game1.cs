using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using static XamalTiler.My_Layouts;
using Android.Views;
using Android.Util;
using System.IO.IsolatedStorage;
using Android.Content;
using zzz;

namespace XamalTiler
{
	public partial class Game1 : Game
	{
		#region Variable Declaration

		public enum ProgramState { Menu, Iterate, StopIterate, Sampling }
		public enum IterateState { StartIterate, Iterating, StopIterate, PollingData, StoppingIterate }

		public enum QuiltType { Square, Hexagon, Icon }
		public enum SampleState { findMax, setDisplayData, cutSpurious, ended, setSpectrum }
		public enum PollIterateData { Waiting, StartCopyNewIteratioins, CopyingNewIterations, CopyNewCompleted, Failure }
		public enum ColourType { Linear, Stretch, SquareRoot }
		public enum HexagonValueNames { sk11, sk12, sk21, sel11, sel21 }

		internal static Color White = Color.White, Ghosted = new Color(64, 64, 64, 64);

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Viewport _viewport;
		
		internal static int _fieldWidth = 1024, _fieldHeight = 1024;

		internal static Point _backBufferSize, _displaySize;

		internal static float _displayRatio;

		public static QuiltType _quiltType = QuiltType.Square;

		internal static IterateState _iterateState = IterateState.StartIterate;

		internal static GameTime _gameTime;

		internal static Action DoIterations;

		internal static RenderTarget2D _imageRenderTarget, _spreadRenderTarget, _fullScreenTarget, _screenshotTarget;

		internal static float _imageScale = 1.0f;

		internal static Vector2 _imageOffset = new Vector2(0, 0);

		internal static bool _drawImageTarget, _isDataPolled;

		internal static int[,] _field = new int[_fieldWidth, _fieldHeight];

		public static int _iterations = 0;
		public static long _iterCounter = 0;
		internal static  int _buildTextureMillisecs = 0;

		internal static UserInputType _userInputType;
		internal static GestureType _activeGestureType;
		internal static UserInputEvent _userInputEvent = UserInputEvent.None;
		public static int _buttonHit;


		public static ProgramState _progState = ProgramState.Menu;

		public static PollIterateData _pollIterateDataState = PollIterateData.Waiting;

		internal const double p2 = 2 * Math.PI;
		internal static double sq3 = Math.Sqrt(3);

		internal const ProgramState Menu = ProgramState.Menu;
		internal const ProgramState Iterate = ProgramState.Iterate;
		internal const ProgramState StopIterate = ProgramState.StopIterate;

		internal static Texture2D _1by1, _tileTexture, _tileSaveTexture;

		internal static SpriteFont[] _fonts = new SpriteFont[2];


		internal static Random _rand = new Random();

		public static SquareValues _square;

		public static Task _task;

		public static HexagonValues _hexValues;

		public static IconTrigValues _itrigValues;

		public static bool _fatalValues = false;

		internal static long _gameTimeMillisecs;

		internal static int _stride = _fieldWidth * 4;

		#endregion



		public Game1(int width, int height)
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			TouchPanel.EnabledGestures = GestureType.Pinch | GestureType.PinchComplete |
					GestureType.FreeDrag | GestureType.DragComplete | GestureType.Tap |
					GestureType.None;

			if (width > height)
				_displaySize = new Point(height, width);
			else
				_displaySize = new Point(width, height);

			graphics.PreferredBackBufferWidth = width;
			graphics.PreferredBackBufferHeight = height;

			graphics.IsFullScreen = true;

			graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

			graphics.ApplyChanges();
		}

		public void ConfigurationChanged(int width, int height)
		{
			graphics.PreferredBackBufferWidth = width;
			graphics.PreferredBackBufferHeight = height;
			graphics.ApplyChanges();
		}


		protected override void Initialize()
		{
			base.Initialize();

			DebugWindow.Initialise(this, graphics, spriteBatch);
			
			ResolveViewPort();

			_backBufferSize = new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
		
			_displayRatio = _backBufferSize.X / (float)_backBufferSize.Y;

			_imageRenderTarget = new RenderTarget2D(GraphicsDevice, _fieldWidth, _fieldHeight);

			_spreadRenderTarget = new RenderTarget2D(GraphicsDevice, _fieldWidth, _fieldHeight);

			_fullScreenTarget = new RenderTarget2D(GraphicsDevice, _backBufferSize.X, _backBufferSize.Y);

			_screenshotTarget = new RenderTarget2D(GraphicsDevice, _displaySize.X, _displaySize.Y);

			_tileTexture = new Texture2D(GraphicsDevice, _fieldWidth, _fieldHeight);
			_tileSaveTexture = new Texture2D(GraphicsDevice, _fieldWidth, _fieldHeight);

			Create_Image._graphicsDevice = GraphicsDevice;
			Create_Image._spriteBatch = spriteBatch;

			Initialise.Preset_Values();
			Initialise.Add_Preset_Colors();

			Layout_Init.Initialise(GraphicsDevice, spriteBatch, _backBufferSize);

			Create_Image.Update_Image_Full();

			DoIterations = delegate () { Iterate_Class.IterateTask(); };

			_drawImageTarget = true;

			GraphicsDevice.SetRenderTarget(_imageRenderTarget);
			GraphicsDevice.Clear(Color.TransparentBlack);
			GraphicsDevice.SetRenderTarget(null);

			Colour_Class.Draw_SpreadRenderTarget();

			//DistributionClass.Initialise();
			//ResolveViewPort();
		}

		private void ResolveViewPort()
		{
			GraphicsDevice.SetRenderTarget(null);

			_viewport = GraphicsDevice.Viewport;

			if (_viewport.Width == graphics.PreferredBackBufferWidth) return;// ||
				_viewport = new Viewport(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

			GraphicsDevice.Viewport = _viewport;

			graphics.ApplyChanges();

		}

		protected override void LoadContent()
		{
			ResourceContentManager resxContent;	resxContent = 
				new ResourceContentManager(this.Services, Resource1.ResourceManager);
			ContentManager libcontent = resxContent;

			spriteBatch = new SpriteBatch(GraphicsDevice);

			_1by1 = new Texture2D(GraphicsDevice, 1, 1);
			_1by1.SetData(new Color[1] { Color.White});

			MyFonts.Initialise(libcontent);
		}

		protected override void UnloadContent()
		{
		}



		protected override void Update(GameTime gameTime)
		{
			_gameTime = gameTime;

			_gameTimeMillisecs += _gameTime.ElapsedGameTime.Milliseconds;

			_buttonHit = User_Input.Update_Input();

			if (_fatalValues && _currentLayoutID == 3)
				_buttonHit = 0;

			Update_ASynch();


			DebugWindow.Update(Point.Zero, false);

			base.Update(gameTime);
		}


		internal void Update_ASynch()
		{
			switch (_progState)
			{
				case Menu:
					if (_isDataPolled)
					{
						_isDataPolled = false;

						Create_Image.Set_Texture_PixelData();
					}

					switch (_currentLayoutID)
					{
						case 0:
							Menu_Class.Update_Main_Menu();

							break;
						case 1:
							Menu_Class.Update_Analyse_Menu();

							break;
						case 2:
							Menu_Class.Update_FullScreen_Menu();
							break;
						case 3:
							break;
						default:
							break;
					}

					break;
				case Iterate:
					Menu_Class.Update_Iterate_Menu();

					if (_progState == ProgramState.Menu) break;

					switch (_iterateState)
					{
						case IterateState.StartIterate:
							_iterateState = IterateState.Iterating;

							_iterations = 0;

							if (_task != null && _task.Status == TaskStatus.Running)
							{
								_task.Start();
							}
							else
								_task = Task.Factory.StartNew(DoIterations);

							break;
						case IterateState.Iterating:
							if(_isDataPolled)
							{
								_isDataPolled = false;

								Create_Image.Set_Texture_PixelData();
							}
							if (_gameTimeMillisecs > 250)
							{
								_gameTimeMillisecs = 0;

								//_iterCounter += _iterations;
								My_Layouts.Change_Button_Text(2,
									"Max - " + Create_Image._maxHits.ToString() +
									" of " + Colour_Class._currentColorSet._colorSpreadCount.ToString());

								

								//_iterCounter = 0;

								_iterateState = IterateState.PollingData;
							}

							break;
						default:
							break;
					}
					break;
				default:
					break;
			}
		}



		protected override void Draw(GameTime gameTime)
		{
			if (_drawImageTarget)
			{
				if (_currentLayoutID == 2)
					Create_Image.Draw_FullScreen();
				else
					Create_Image.Draw_Target();
			}

			//DistributionClass.Draw_Target();

			GraphicsDevice.SetRenderTarget(null);
			//ResolveViewPort();
			if (_currentLayoutID == 2)
				GraphicsDevice.Clear(Color.Black);
			else
				GraphicsDevice.Clear(new Color(21, 60, 120));

			spriteBatch.Begin();

			My_Layouts.Draw();

			DebugWindow.Draw();

			spriteBatch.End();

			base.Draw(gameTime);
		}
	
	}
}
