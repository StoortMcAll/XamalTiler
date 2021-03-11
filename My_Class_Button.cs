using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using static XamalTiler.Game1;
using static XamalTiler.MyFonts;

namespace XamalTiler
{

	internal static partial class My_Layouts
	{
		public class MyButton
		{
			#region Variable Declaration

			public MyLayout _parentLayout;

			SpriteFont _buttonFont;

			float _fontScale = 1.0f;

			Color _layoutCol = Color.LightGray;
			Color _borderCol = Color.Black;
			Color _background = new Color(32, 32, 40);// Color.LightGray;
			Color _backGrey = new Color(64, 64, 80);// Color.Gray;

			Color _fontCol = new Color(160, 160, 200);
			Color _fontShadowCol = new Color(16, 16, 20);
			Color _fontGrey = new Color(32, 32, 32);

			Color _greyed = new Color(50, 50, 50, 50);

			Texture2D _texture;

			RenderTarget2D _buttonTarget;

			List<Rectangle> _borders;

			Rectangle _canvas, _border, _area, _canvasTexture, _textRectangle;

			bool _hasText, _hasTexture, _isTransparent = false;

			SpriteEffects _undoSpriteEffect = SpriteEffects.None;

			Vector2 _textPosition;

			string _text;

			public int _ID;

			public UserInputType _inputType;

			public MyButtonState _myButtonState;

			#endregion


			public MyButton(MyLayout parentLayout, int ID, Rectangle area, string text,
				MyButtonState myButtonState = MyButtonState.Visible,
				UserInputType inputType = UserInputType.Release)
			{
				_parentLayout = parentLayout;

				_ID = ID;

				_buttonTarget = new RenderTarget2D(
					_graphicsDevice,
					area.Width, area.Height);

				_buttonFont = _parentLayout._myFont._font;

				_canvas = _area = area;

				_area.Location = Point.Zero;

				_text = text;

				_hasTexture = false;

				_border = _area;

				if (inputType != UserInputType.None) ;
					//_area.Inflate(-4, -4);
				else
				{
					_background = new Color(40, 40, 50);
					_fontCol = new Color(180, 180, 205);
					///_background = Color.DarkSlateBlue;
					///_fontCol = Color.LightGray;
				}

				_myButtonState = myButtonState;

				_inputType = inputType;

				//Set_Borders();

				Set_Text(text);
			}

			public MyButton(MyLayout parentLayout, int ID, Rectangle area, Texture2D texture,
				MyButtonState myButtonState = MyButtonState.Visible,
				UserInputType inputType = UserInputType.Release)
			{
				_parentLayout = parentLayout;

				_ID = ID;

				_buttonTarget = new RenderTarget2D(
					_graphicsDevice,
					area.Width, area.Height);

				_buttonFont = _parentLayout._myFont._font;

				_canvas = _canvasTexture = _area = area;

				_area.Location = Point.Zero;

				_hasText = false;

				_hasTexture = true;

				_texture = texture;

				_border = _area;

			//	_area.Inflate(-4, -4);

			//	_canvasTexture.Inflate(-4, -4);

				//_fontScale = Best_Fit_Font();

				_myButtonState = myButtonState;

				_inputType = inputType;

				Set_Borders();

				Draw_Target();
			}


			public void Set_Text(string text)
			{
				_text = text;

				_hasText = _text != "";

				if (_hasText == false) return;

			
				_fontScale = Best_Fit_Font();

				Vector2 fontsize = _buttonFont.MeasureString(text) * _fontScale;

				_textPosition = Vector2.Zero;

				_textPosition.X = _area.Center.X - (int)fontsize.X / 2;
				_textPosition.Y = _area.Center.Y - 2 - (int)fontsize.Y / 2;

				_textRectangle = Rectangle.Empty;

				_textRectangle.Location = _textPosition.ToPoint();
				_textRectangle.Size = fontsize.ToPoint();
				
				Draw_Target();
			}

			float Best_Fit_Font()
			{
				Set_CurrentFont(0);

				float fontscale = 1.0f;

				while (_area.Height - 4 < _currentMyFont._font.MeasureString(_text).Y * fontscale)
					fontscale *= 0.99f;

				while (_area.Width - 4 < _currentMyFont._font.MeasureString(_text).X * fontscale)
					fontscale *= 0.99f;

				return fontscale;
			}

			public void Set_Transparent(bool isTransparent)
			{
				_isTransparent = isTransparent;

				//if (_isTransparent)
				//	_fontCol = new Color(8, 8, 8);
				//else

				_fontCol = new Color(100, 100, 200);

				Set_Borders();

				Draw_Target();
			}

			private void Set_Borders()
			{
				if (_isTransparent)
				{
					_layoutCol = Color.TransparentBlack;

					_borders = new List<Rectangle>(4);

					Rectangle temp = _border;

					temp.Width = 4;

					_borders.Add(temp);

					temp.X = _border.Right - 4;

					_borders.Add(temp);

					temp = _border;

					temp.Height = 4;

					_borders.Add(temp);

					temp.Y = _border.Bottom - 4;

					_borders.Add(temp);

				}
				else
				{
					_borders = new List<Rectangle>(1) { _border };
				}
			}

			public bool Contains(Vector2 location)
			{
				if (_inputType == UserInputType.None) return false;

				return _canvas.Contains(location);
			}

			public Rectangle Get_TextureCanvas()
			{
				return _canvasTexture;
			}

			public void Draw_Target()
			{
				_graphicsDevice.SetRenderTarget(_buttonTarget);
				_graphicsDevice.Clear(_layoutCol);

				_spriteBatch.Begin();

				//foreach (var border in _borders)
				//	_spriteBatch.Draw(_1by1, border, _borderCol);

			
				if (_myButtonState == MyButtonState.Greyed)
				{
					_spriteBatch.Draw(_1by1, _area, _backGrey);

					if (_hasText)
					{
						if (Test_FontRectangle_IsVisible) _spriteBatch.Draw(_1by1, _textRectangle, Color.Red);

						_spriteBatch.DrawString(_buttonFont, _text,
							_textPosition + new Vector2(0, -1.0f), _fontShadowCol, 0, Vector2.Zero, _fontScale, SpriteEffects.None, 0.95f);
						_spriteBatch.DrawString(_buttonFont, _text,
							_textPosition + new Vector2(0, 1.0f), _fontShadowCol, 0, Vector2.Zero, _fontScale, SpriteEffects.None, 0.95f);
						_spriteBatch.DrawString(_buttonFont, _text,
							_textPosition + new Vector2(-1.0f, 0), _fontShadowCol, 0, Vector2.Zero, _fontScale, SpriteEffects.None, 0.95f);
						_spriteBatch.DrawString(_buttonFont, _text,
							_textPosition + new Vector2(1.0f, 0), _fontShadowCol, 0, Vector2.Zero, _fontScale, SpriteEffects.None, 0.95f);

						_spriteBatch.DrawString(_buttonFont, _text, _textPosition, _fontGrey, 0, Vector2.Zero, _fontScale, SpriteEffects.None, 1.0f);
					}

					_spriteBatch.Draw(_1by1, _area, _greyed);
				}
				else
				{
					_spriteBatch.Draw(_1by1, _area, _background * (_isTransparent ? 0.7125f : 1.0f));

					if (_hasText)
					{
						if (Test_FontRectangle_IsVisible) _spriteBatch.Draw(_1by1, _textRectangle, Color.Red);

						_spriteBatch.DrawString(_buttonFont, _text,
							_textPosition + new Vector2(0, -1.0f), _fontShadowCol, 0, Vector2.Zero, _fontScale, SpriteEffects.None, 0.95f);
						_spriteBatch.DrawString(_buttonFont, _text,
							_textPosition + new Vector2(0, 1.0f), _fontShadowCol, 0, Vector2.Zero, _fontScale, SpriteEffects.None, 0.95f);
						_spriteBatch.DrawString(_buttonFont, _text,
							_textPosition + new Vector2(-1.0f, 0), _fontShadowCol, 0, Vector2.Zero, _fontScale, SpriteEffects.None, 0.95f);
						_spriteBatch.DrawString(_buttonFont, _text,
							_textPosition + new Vector2(1.0f, 0), _fontShadowCol, 0, Vector2.Zero, _fontScale, SpriteEffects.None, 0.95f);

						_spriteBatch.DrawString(_buttonFont, _text, _textPosition, _fontCol, 0, Vector2.Zero, _fontScale, SpriteEffects.None, 1.0f);
					}
				}

				_spriteBatch.End();
			}


			public void Draw()
			{
				_spriteBatch.Draw(_buttonTarget, _canvas, DrawColor);

				if (_hasTexture)
					_spriteBatch.Draw(_texture, _canvasTexture, DrawColor);
			}
		}


	}
}