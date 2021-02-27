using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using static XamalTiler.Game1;
using static XamalTiler.MyFonts;


namespace XamalTiler
{
	internal static partial class My_Layouts
	{

		internal class MyLayout
		{
			#region Variable Declaration

			public int _layoutID;

			internal MyFont _myFont;

			public Divider _activeDivide;

			Rectangle _backPane;

			Color _backPaneCol = new Color(14, 40, 80);

			public List<Divider> _dividers = new List<Divider>();

			public List<MyButton> _buttons = new List<MyButton>();

			#endregion


			internal MyLayout(int ID, List<Divider> dividers)
			{
				_layoutID = ID;

				_dividers = dividers;

				_activeDivide = _dividers[0];

				_myFont = Set_CurrentFont(6);

				_backPane = _dividers[0]._area;

				for (int i = 1; i < _dividers.Count; i++)
					_backPane = Rectangle.Union(_backPane, _dividers[i]._area);

				_backPane.Inflate(2, 2);
			}


			internal void Draw()
			{
				_spriteBatch.Draw(_1by1, _backPane, _backPaneCol);

				foreach (var button in _buttons)
				{
					if (button._myButtonState != MyButtonState.Hidden)
						button.Draw();
				}
			}


			internal void Add_Button(int ID, Rectangle area, string text,
				MyButtonState myButtonState = MyButtonState.Visible,
				UserInputType inputType = UserInputType.Release)
			{
				_activeDivide = _dividers[0];

				Rectangle canvas = new Rectangle(PosX(area.X), PosY(area.Y), 0, 0);
				canvas.Width = PosX(area.Width) - canvas.X + 1;
				canvas.Height = PosY(area.Height) - canvas.Y + 1;

				canvas.Inflate(0, -4);

				canvas.X += 2;
				canvas.Width -= 4;

				_currentLayout._buttons.Add(new MyButton(this, ID, canvas, text, myButtonState, inputType));
			}
			internal void Add_Button(int ID, int dividerID, Rectangle area, Texture2D texture,
				MyButtonState myButtonState = MyButtonState.Visible,
				UserInputType inputType = UserInputType.Release)
			{
				_activeDivide = _dividers[dividerID];
				//if (myButtonState != MyButtonState.FullScreen)
				//	_activeDivide = _dividers[1];
				//else
				//	_activeDivide = _dividers[0];

				Rectangle canvas = new Rectangle(PosX(area.X), PosY(area.Y), 0, 0);
				canvas.Width = PosX(area.Width) - canvas.X;
				canvas.Height = PosY(area.Height) - canvas.Y;

				if (myButtonState != MyButtonState.FullScreen)
				{
					canvas.Inflate(0, -4);

					canvas.X += 2;
					canvas.Width -= 4;

				}
				else
				{
					canvas.Location = Point.Zero;
					canvas.Width = _screenSize.X;// + 20;
					canvas.Height = _screenSize.Y;// + 20;

					myButtonState = MyButtonState.Visible;
				}

				_currentLayout._buttons.Add(new MyButton(this, ID, canvas, texture, myButtonState, inputType));
			}


			int PosX(int x)
			{
				if (x <= 0) return _activeDivide._area.X;

				if (x >= _activeDivide._grid.X) return _activeDivide._area.Right - 1; ;

				return _activeDivide._area.X + (int)((_activeDivide._area.Width / ((float)_activeDivide._grid.X)) * x);
			}
			int PosY(int y)
			{
				if (y <= 0) return _activeDivide._area.Y;

				if (y >= _activeDivide._grid.Y) return _activeDivide._area.Bottom - 1;

				return _activeDivide._area.Y + (int)((_activeDivide._area.Height / ((float)_activeDivide._grid.Y)) * y);
			}
		}

	}
}