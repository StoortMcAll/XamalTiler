using System;
using System.Collections.Generic;
//using Android.Graphics;
using Android.Widget;
//using Android.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using static XamalTiler.Game1;
using static XamalTiler.MyFonts;

namespace XamalTiler
{
	internal static partial class My_Layouts
	{
		const bool Test_FontRectangle_IsVisible = false;
		
		#region Variable Declaration

		internal enum ButtonTypes { TextButton, ImageButton, TextBox }
		internal enum MyButtonState { Visible, Greyed, Hidden, FullScreen }

		public enum UserInputType { Press, Release, Pinch, Drag, PinchDrag, None }
		public enum UserInputEvent { None, Pressed, Released, Pinched, Dragged }

		internal static Color DrawColor = White;

		internal static GraphicsDevice _graphicsDevice;
		internal static SpriteBatch _spriteBatch;

		static Point _screenSize;

		static List<MyLayout> _layouts = new List<MyLayout>();

		public static MyLayout _currentLayout;
		public static int _currentLayoutID = 0;

		public static MyButton _currentButton;

		internal struct Divider
		{
			internal int _ID;
			internal Rectangle _area;
			internal Point _grid;

			internal Divider(int id, Rectangle area, Point grid)
			{
				_ID = id;
				_area = area;
				_grid = grid;
			}
		}

		#endregion


		internal static void Init_Layouts(
			GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Point screenSize)
		{
			_graphicsDevice = graphicsDevice;
			_spriteBatch = spriteBatch;
			
			_screenSize = screenSize;
		}


		#region Static Methods


		internal static int Button_Contains(Vector2 location)
		{
			int id = -1;
			for (int i = 0; i < _currentLayout._buttons.Count; i++)
			{
				if (_currentLayout._buttons[i]._myButtonState == MyButtonState.Visible)
					if (_currentLayout._buttons[i].Contains(location))
						id = _currentLayout._buttons[i]._ID;
			}

			return id;
		}

		internal static Rectangle Button_Canvas(int buttonID)
		{
			return ButtonFromID(buttonID).Get_TextureCanvas();
		}
			

		internal static void Hide_Button(int buttonID)
		{
			ButtonFromID(buttonID)._myButtonState = MyButtonState.Hidden;
		}
		internal static void Show_Button(int buttonID)
		{
			if (IsIDGood(buttonID))
			{
				Set_CurrentButton(buttonID);

				if (_currentButton._myButtonState != MyButtonState.Visible)
				{
					_currentButton._myButtonState = MyButtonState.Visible;

					_currentButton.Draw_Target();
				}
			}
		}
		internal static void Grey_Button(int buttonID)
		{
			if (IsIDGood(buttonID))
			{
				Set_CurrentButton(buttonID);

				if (_currentButton._myButtonState != MyButtonState.Greyed)
				{
					_currentButton._myButtonState = MyButtonState.Greyed;

					_currentButton.Draw_Target();
				}
			}
		}

	 	static bool IsIDGood(int buttonID)
		{
			if (ButtonFromID(buttonID) == null) return false;

			return true;
		}

		internal static void Set_CurrentButton(int buttonID)
		{
			MyButton MyButton = ButtonFromID(buttonID);

			if (MyButton == null) return;

			_currentButton = MyButton;
		}

		internal static MyButton ButtonFromID(int buttonID)
		{
			for (int i = 0; i < _currentLayout._buttons.Count; i++)
			{
				if (_currentLayout._buttons[i]._ID == buttonID)
				{
					return _currentLayout._buttons[i];
				}
			}

			return null;
		}

		internal static void Add_Layout(int ID, List<Divider> dividers)
		{
			_layouts.Add(new MyLayout(ID, dividers));

			Activate_Layout(ID);
		}
		
		internal static void Activate_Layout(int ID)
		{
			for (int i = 0; i < _layouts.Count; i++)
			{
				if (_layouts[i]._layoutID == ID)
				{
					_currentLayout = _layouts[i];

					_currentLayoutID = ID;

					return;
				}
			}
		}


		internal static void Add_Button(int ID, Rectangle area, string text, 
			MyButtonState myButtonState = MyButtonState.Visible,
				UserInputType inputType = UserInputType.Release)
		{
			_currentLayout.Add_Button(ID, area, text, myButtonState, inputType);
		}
		internal static void Add_Transparent_Button(int ID, Rectangle area, string text,
			MyButtonState myButtonState = MyButtonState.Visible,
				UserInputType inputType = UserInputType.Release)
		{
			_currentLayout.Add_Button(ID, area, text, myButtonState, inputType);
			ButtonFromID(ID).Set_Transparent(true);
		}
		internal static void Add_Button(int ID, int dividerID, Rectangle area, Texture2D texture,
			MyButtonState myButtonState = MyButtonState.Visible,
				UserInputType inputType = UserInputType.Release)
		{
			_currentLayout.Add_Button(ID, dividerID, area, texture, myButtonState, inputType);
		}


		internal static void Set_Button_Trans(int ID, bool isTransparent)
		{
			if (!IsIDGood(ID)) return;

			if (_currentLayout._buttons.Count <= ID) return;

			ButtonFromID(ID).Set_Transparent(isTransparent);
		}

		internal static void Change_Button_Text(int ID, string text)
		{
			if (!IsIDGood(ID)) return;

			if (_currentLayout._buttons.Count <= ID) return;

			ButtonFromID(ID).Set_Text(text);
		}

		internal static UserInputType Get_ButtonType(int ID)
		{
			if (!IsIDGood(ID)) return UserInputType.Release;

			return ButtonFromID(ID)._inputType;
		}

		#endregion

		internal static void Draw()
		{
			if (ScreenShot._saveImageState == ScreenShot.SaveImageState.Ready)
				DrawColor = White;
			else
				DrawColor = Ghosted;

			_currentLayout.Draw();
		}
		
	}
}