using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using static XamalTiler.Game1;
using static XamalTiler.My_Layouts;

namespace XamalTiler
{

	internal static class Layout_Init
	{

		#region Variable Declaration

		internal static DisplayOrientation _displayOrientation;

		#endregion


		internal static void Initialise(
			GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Point screenSize)
		{
			Create_Image._pixelData = new byte[_fieldHeight * _fieldWidth * 4];
			Create_Image._pixelDataStore = new byte[_fieldHeight * _fieldWidth * 4];

			Colour_Class.Set_Vertices();

			List<Divider> dividers = new List<Divider>();

			Init_Layouts(graphics.GraphicsDevice, spriteBatch, screenSize);

			_displayOrientation = screenSize.X < screenSize.Y ? DisplayOrientation.Portrait : DisplayOrientation.LandscapeLeft;

			Rectangle area, area2 = new Rectangle(Point.Zero, screenSize);

			area2.Inflate(-4, -4);


			if (_displayOrientation == DisplayOrientation.Portrait)
			{
				area2.Y = area2.Bottom - (area2.Width + 0);
				area2.Height = area2.Width;

				area = area2;
				area.Y = 4;
				area.Height = area2.Y - area.Y;
			}
			else
			{
				area2.X = area2.Right - (area2.Height + 0);
				area2.Width = area2.Height;

				area = area2;
				area.X = 4;
				area.Width = area2.X - area.X;
			}
			

			#region Layout 0 - Start Iterating

			Rectangle grid = new Rectangle(0, 0, 8, 8);

			dividers.Add(new Divider(0, area, grid.Size));

			
			dividers.Add(new Divider(1, area2, new Point(1, 1)));

			Add_Layout(0, dividers);

			int butwid = (grid.Width - 2) / 3;

			Rectangle start = new Rectangle(0, 0, butwid, 1);

			Add_Button(0, start, "Lace");

			start.X = butwid + 1;
			start.Width = start.X + butwid;
			Add_Button(5, start, "Scar");

			start.X = grid.Width - butwid;
			start.Width = grid.Width;
			Add_Button(6, start, "Hexa");

			start.X = 0; start.Width = grid.Width;
			start.Y = 1; start.Height = 2;
			Add_Button(1, start, "Resume", MyButtonState.Hidden);

			start.Y = 2; start.Height = 3;
			start.X = 2; start.Width = grid.Width;
			Add_Button(2, start, "Palette", MyButtonState.Hidden, UserInputType.None);

			start.X = 0; start.Width = 1;
			Add_Button(7, start, "<", MyButtonState.Hidden);
			start.X = 1; start.Width = 2;
			Add_Button(8, start, ">", MyButtonState.Hidden);

			start.Y = 4; start.Height = 5;
			start.X = 0; start.Width = grid.Width;
			Add_Button(3, start, "Edit Image>", MyButtonState.Hidden);

			start.Y = 3; start.Height = 4;
			start.X = 0; start.Width = grid.Width;
			Add_Button(20,0, start, _spreadRenderTarget, MyButtonState.Hidden, UserInputType.Drag);

			//start.X = 0; start.Width = 1;
			//Add_Button(10, start, "<", MyButtonState.Hidden, UserInputType.Press);
			//start.X = 1; start.Width = 2;
			//Add_Button(11, start, ">", MyButtonState.Hidden, UserInputType.Press);

			//start.Y = 3; start.Height = 7;
			//start.X = 2; start.Width = grid.Width;
			//Add_Transparent_Button(12, start, "Colour Weighting", MyButtonState.Hidden, UserInputType.None);



			start = new Rectangle(Point.Zero, dividers[1]._grid);
			Add_Button(4, 1, start, _imageRenderTarget,  MyButtonState.Visible, UserInputType.PinchDrag);

			#endregion


			#region Layout 1 - Edit Image  

			dividers[1] = new Divider(1, area2, new Point(1, 6));

			Add_Layout(1, dividers);

			start = new Rectangle(0, 0, grid.Width, 1);

			Add_Button(0, start, "<Create Image");
	
			start.Y = 1; start.Height = 2;
			start.X = 2; start.Width = grid.Width;
			Add_Button(1, start, "Palette", MyButtonState.Visible, UserInputType.None);
			start.X = 0; start.Width = grid.Width - 1;

			start.Y = 2; start.Height = 3;
			start.X = 3; start.Width = grid.Width - 2;
			Add_Button(15, start, "Random", MyButtonState.Visible, UserInputType.None);
			start.X = grid.Width - 2; start.Width = grid.Width - 1;
			Add_Button(9, start, "A");
			start.X = grid.Width - 1; start.Width = grid.Width;
			Add_Button(12, start, "B");

			start.Y = 2; start.Height = 3;
			start.X = 0; start.Width = 1;
			Add_Button(16, start, "<", MyButtonState.Greyed);
			start.X = 1; start.Width = 2;
			Add_Button(17, start, ">", MyButtonState.Greyed);
			start.X = 2; start.Width = 3;
			Add_Button(18, start, "X", MyButtonState.Greyed);


			start.Y = 3; start.Height = 4;
			start.X = 2; start.Width = grid.Width;
			Add_Button(2, start, "Linear", MyButtonState.Visible, UserInputType.None);
			start.X = 0; start.Width = grid.Width;

			start.Y = 4; start.Height = 5;
			start.X = 2; start.Width = grid.Width;
			Add_Button(3, start, "Sharp Image", MyButtonState.Visible, UserInputType.None);
			start.X = 0; start.Width = grid.Width;

			start.Y++; start.Height++;
			Add_Button(4, start, "Start From 0");

			start.Y++; start.Height++;
			Add_Button(20, 0, start, _spreadRenderTarget, MyButtonState.Visible, UserInputType.Drag);

			start.Y++; start.Height++;
			Add_Button(5, start, "Full Screen");

			start.Y = 1; start.Height = 2;
			start.X = 0; start.Width = 1;
			Add_Button(7, start, "<");
			start.X = 1; start.Width = 2;
			Add_Button(8, start, ">");


			start.Y = 3; start.Height = 4;
			start.X = 0; start.Width = 1;
			Add_Button(10, start, "<");
			start.X = 1; start.Width = 2;
			Add_Button(11, start, ">");

			start.Y = 4; start.Height = 5;
			start.X = 0; start.Width = 1;
			Add_Button(13, start, "<");
			start.X = 1; start.Width = 2;
			Add_Button(14, start, ">");

			

			start = new Rectangle(0, 0, 1, 6);
			Add_Button(6, 1, start, _imageRenderTarget, MyButtonState.Visible, UserInputType.PinchDrag);


			#endregion


			#region Layout 2 Fullscreen Save Image

			dividers[1] = new Divider(1, area2, new Point(1, 1));

			grid = Find_Grid_For_FullScreen(screenSize);
			
			Rectangle rect = Rectangle.Empty; 
			rect.Size = screenSize;
			rect.Inflate(-10, -10);

			Add_Layout(2, new List<Divider>() { new Divider(0, rect, grid.Size) });

			start = new Rectangle(0, 0, grid.Width, grid.Height);
		

			Add_Button(0, 0, start, _fullScreenTarget, MyButtonState.FullScreen, UserInputType.PinchDrag);


			start = new Rectangle(0, 0, 1, 1);
			Add_Transparent_Button(1, start, "<");

			start = grid;
			if (_displayOrientation != DisplayOrientation.Portrait)
			{
				start.X = start.Width / 2 - 4;
				start.Width -= 4;
			}

			start = new Rectangle(start.X, grid.Height - 2, start.Width, grid.Height - 1);
			Add_Transparent_Button(2, start, "Save Image");

			start = new Rectangle(start.X, grid.Height - 1, start.Width, grid.Height);
			Add_Transparent_Button(3, start, "Set as Wallpaper");

			#endregion


			#region Layout 3 Run Iteration Passes

			grid = new Rectangle(0, 0, 11, 7);

			Add_Layout(3, dividers);

			start = new Rectangle(0, 1, grid.Width, 2);

			Add_Button(0, start, "Stop");
			
			start.Y = 2; start.Height = 3;
			start.X = 2; start.Width = grid.Width;
			Add_Button(1, start, "Palette", MyButtonState.Visible, UserInputType.None);

			start.X = 0; start.Width = 1;
			Add_Button(4, start, "<");
			start.X = 1; start.Width = 2;
			Add_Button(5, start, ">");


			start.X = 0; start.Width = grid.Width;
			start.Y = 0; start.Height = 1;
			Add_Button(2, start, "", MyButtonState.Visible, UserInputType.None);

			start = new Rectangle(0, 0, 1, 1);
			Add_Button(3, 1, start, _imageRenderTarget, MyButtonState.Visible, UserInputType.PinchDrag);

			#endregion


			Activate_Layout(0);

		}


		private static Rectangle Find_Grid_For_FullScreen(Point screenSize)
		{
			if (screenSize.X < 480)
			{
				if (_displayOrientation == DisplayOrientation.Portrait)
					return new Rectangle(0, 0, 8, 13);
				else
					return new Rectangle(0, 0, 13, 8);
			}
			else
			{
				if (_displayOrientation == DisplayOrientation.Portrait)
					return new Rectangle(0, 0, 8, 17);
				else
					return new Rectangle(0, 0, 17, 8);
			}
		}
	}
}