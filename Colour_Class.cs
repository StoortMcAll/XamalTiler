using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Microsoft.Xna.Framework;

using static XamalTiler.Game1;
using static XamalTiler.My_Layouts;

namespace XamalTiler
{
    internal static partial class Colour_Class
    {
        #region Variable Declaration

        const int PrimaryColourCount1 = 33;
        const int PrimaryColourCount2 = 13;
        const int PrimaryColourCount3 = 13;
        const int PrimaryColourCount4 = 13;
        internal static int PrimaryColourCount;

        public static Color[] PrimaryColors;

        public static Color[] PrimaryColors1 = new Color[PrimaryColourCount1] {
                                                Color.Black, 
                                                new Color(32, 0, 0), new Color(0, 32, 0),new Color(0, 0, 32),
                                                new Color(128, 0, 0),new Color(0, 128, 0), new Color(0, 0, 128),//3
                                                Color.SeaGreen, Color.Silver, Color.Gold, Color.RosyBrown, Color.PowderBlue,
                                                Color.Red, Color.Lime, Color.Blue,//6
                                                new Color(255, 128, 0), new Color(128, 255, 0), new Color(128, 0, 255),//9
                                                new Color(255, 0, 128), new Color(0, 255, 128), new Color(0, 128, 255),//12
                                                new Color(255, 128, 128), new Color(128, 255, 128), new Color(128, 128, 255),//15
                                                Color.Yellow, Color.Magenta, Color.Cyan,//18
                                                new Color(255, 255, 128), new Color(255, 128, 255), new Color(128, 255, 255),//21
                                                new Color(255, 255, 240), new Color(255, 240, 255), new Color(240, 255, 255) };//24

        public static Color[] PrimaryColors2 = new Color[PrimaryColourCount2] {
                                                Color.Black,
                                                new Color(32, 0, 0), 
                                                new Color(0, 64, 0),//3
                                                new Color(0, 0, 96),
                                                new Color(112, 0, 0),
                                                new Color(0, 128, 0),

                                                Color.Blue,//6
                                                
                                                new Color(255, 128, 0),//9
                                                
                                                new Color(0, 255, 128),//12
                                                
                                                new Color(128, 128, 255),//15
                                                
                                                Color.Yellow,//18
                                                
                                                new Color(128, 255, 255),//21
                                                
                                                new Color(255, 240, 255) };//24

        public static Color[] PrimaryColors3 = new Color[PrimaryColourCount3] {
                                                Color.Black,

                                                new Color(0, 0, 32),
                                                new Color(64, 0, 0),//3
                                                new Color(0, 96, 0),
                                                new Color(0, 0, 112),
                                                new Color(128, 0, 0),//3
                                                Color.Lime,//6
                                                new Color(128, 0, 255),//9
                                                new Color(255, 0, 128),//12
                                                new Color(128, 255, 128),//15
                                                Color.Cyan,//18
                                                new Color(255, 255, 128),//21
                                                new Color(240, 255, 255) };//24

        public static Color[] PrimaryColors4 = new Color[PrimaryColourCount4] {
                                                Color.Black,
                                                new Color(0, 32, 0),
                                                new Color(0, 0, 64),//3
                                                new Color(96, 0, 0),
                                                new Color(0, 112, 0),
                                                new Color(0, 0, 128),//3
                                                Color.Red,//6
                                                new Color(128, 255, 0),
                                                new Color(0, 128, 255),//12
                                                new Color(255, 128, 128),
                                                Color.Magenta,
                                                new Color(128, 255, 255),//21
                                                new Color(255, 255, 240) };//24

        public static List<Color_Set> _colorSets = new List<Color_Set>();

        public static List<Color_Set> _colorSetsNew = new List<Color_Set>();

        public static List<ColorRanges> _colorRanges = new List<ColorRanges>();

        internal static int _currentColorSetID = 0, _currentSpreadCount = 0, _colorSetID = 0, _colorSetNewID = -1;

        internal static Color_Set _currentColorSet;

        internal static List<Color> _colorSpread;

        internal static Random _colourRand;

        internal static bool _usingRandomSpread = false;

        static bool _randomSpreadExists = false;

        #endregion

        public class Color_Set
        {
            #region Variable Declaration

            public List<Color> _colorSpread = new List<Color>();

            public int _colorSpreadCount;

            public int _colorSetID = 0;

            #endregion


            public Color_Set(ColorRanges colorRanges)
            {
                _colorRanges.Add(colorRanges);

                _colorSpread = colorRanges._colorSpread;

                _colorSpreadCount = colorRanges._colorSpreadCount;

                _colorSetID = colorRanges._colorSetID = _colorSets.Count;
            }

            public Color_Set(ColorSpreadList colorSpread)
            {
                _colorSpread = colorSpread._colorSpread;

                _colorSpreadCount = colorSpread._colorSpreadCount;

                _colorSetID = colorSpread._colorSpreadCount = _colorSets.Count;
            }
        }

        public struct ColorRange
        {
            #region Variable Declaration

            public int _range;
            public Color _color;

            #endregion


            public ColorRange(int range, Color color)
            {
                _range = range;
                _color = color;
            }
        }

        public class ColorRanges
        {
            #region Variable Declaration

            List<ColorRange> _colorRanges = new List<ColorRange>();

            public List<Color> _colorSpread;

            public int _colorSpreadCount;

            public int _colorSetID = 0;

            #endregion


            public ColorRanges(ColorRange colorRange)
            {
                _colorRanges.Add(colorRange);

                Process_ColorRanges();

                _colorSpreadCount = _colorSpread.Count - 1;
            }
            public ColorRanges(List<ColorRange> colorRanges)
            {
                _colorRanges.AddRange(colorRanges);

                Process_ColorRanges();

                _colorSpreadCount = _colorSpread.Count - 1;
            }


            public void Add_ColorRanges(ColorRange colorRange)
            {
                _colorRanges.Add(colorRange);

                Process_ColorRanges();
            }
            public void Add_ColorRanges(List<ColorRange> colorRanges)
            {
                colorRanges.AddRange(colorRanges);

                Process_ColorRanges();
            }


            private void Process_ColorRanges()
            {
                _colorSpread = new List<Color>();

                if (_colorRanges.Count < 2) return;

                for (int i = 1; i < _colorRanges.Count; i++)
                {
                    int maxd = _colorRanges[i]._range;

                    byte[] rs = Color_Channel_Range(maxd, _colorRanges[i - 1]._color.R, _colorRanges[i]._color.R);
                    byte[] gs = Color_Channel_Range(maxd, _colorRanges[i - 1]._color.G, _colorRanges[i]._color.G);
                    byte[] bs = Color_Channel_Range(maxd, _colorRanges[i - 1]._color.B, _colorRanges[i]._color.B);

                    for (int j = 0; j < rs.Length; j++)
                    {
                        _colorSpread.Add(new Color(rs[j], gs[j], bs[j]));
                    }
                }
            }

            private byte[] Color_Channel_Range(int maxd, byte r1, byte r2)
            {
                byte[] results = new byte[maxd];

                results[0] = r1;

                float dx = (r2 - r1) / (float)maxd;

                if (dx == 0)
                    for (int i = 1; i < maxd; i++)
                        results[i] = (byte)r1;
                else
                    for (int i = 1; i < maxd; i++)
                        results[i] = (byte)(r1 + (dx * i));

                return results;
            }
        }

        public class ColorSpreadList
        {
            #region Variable Declaration

            public List<Color> _colors = new List<Color>();
            public List<Color> _colorSpread;
            public int _colorSpreadCount;

            public int _colorSetID = 0;

            #endregion


            public ColorSpreadList()
            {
                _colors.Add(Color.Black);
                _colors.Add(Color.Yellow);

                Full_Color_Spread();
            }
            public ColorSpreadList(Color color)
            {
                _colors.Add(Color.Black);
                _colors.Add(color);

                Full_Color_Spread();
            }
            public ColorSpreadList(List<Color> colors)
            {
                if (colors.Count < 2)
                    _colors.Add(Color.Black);
                if (colors.Count == 0)
                    _colors.Add(Color.Yellow);
                else _colors.AddRange(colors);

                Full_Color_Spread();
            }


            public void Add_Color(Color color)
            {
                _colors.Add(color);

                Calc_Color_Spread(_colors[^1], color);

                _colorSpreadCount = _colors.Count;
            }

            public void Full_Color_Spread()
            {
                _colorSpread = new List<Color>();

                for (int i = 1; i < _colors.Count; i++)
                    Calc_Color_Spread(_colors[i - 1], _colors[i]);


                _colorSpreadCount = _colorSpread.Count - 1;
            }

            private void Calc_Color_Spread(Color minColor, Color maxColor)
            {

                int dr = maxColor.R - minColor.R;
                int dg = maxColor.G - minColor.G;
                int db = maxColor.B - minColor.B;

                int maxd = Math.Max(Math.Abs(dr), Math.Abs(dg));
                maxd = Math.Max(maxd, Math.Abs(db));

                byte[] rs = Color_Channel_Range(maxd, minColor.R, maxColor.R);
                byte[] gs = Color_Channel_Range(maxd, minColor.G, maxColor.G);
                byte[] bs = Color_Channel_Range(maxd, minColor.B, maxColor.B);

                for (int i = 0; i < rs.Length; i++)
                {
                    _colorSpread.Add(new Color(rs[i], gs[i], bs[i]));
                }
            }

            private byte[] Color_Channel_Range(int maxd, byte r1, byte r2)
            {
                byte[] results = new byte[maxd];

                results[0] = r1;

                float dx = r2 - r1;
                float fmax = 1.0f / maxd;

                if (dx == 0)
                    for (int i = 1; i < maxd; i++)
                        results[i] = (byte)r1;
                else
                    for (int i = 1; i < maxd; i++)
                        results[i] = (byte)(r1 + (dx * i * fmax));

                return results;
            }
        }


        public static bool TrueNotFalse { get { return _rand.Next(2) != 0; } }


        internal static void NewRandom_ColourSeries()
        {
            bool luminanceflip = false;

            List<ColorRange> colorlist = new List<ColorRange>();

            int counter, maxnewcols, adder = 64;

            _colourRand = new Random();

            if (Hits._maxHitsCounted == false) Create_Image.Find_Max_Hits();

            maxnewcols = (int)Create_Image._maxHits;

            if (maxnewcols < 64) maxnewcols = 64;
            else if (maxnewcols > 1024) maxnewcols = 1024;

            counter = 0;

            colorlist.Add(new ColorRange(0, Get_RandomColour(true)));

            do
            {
                counter += adder;

                if (counter + adder > maxnewcols) counter = maxnewcols;
                else adder *= 2;

                colorlist.Add(new ColorRange(counter, Get_RandomColour(luminanceflip)));

                luminanceflip = !luminanceflip;

            } while (counter < maxnewcols);

            Add_New_RandomColorSet(new Color_Set(new ColorRanges(colorlist)));
        }

        internal static void NewRandom_ColourSeries3()
        {
            bool luminanceflip = false;

            List<ColorRange> colorlist = new List<ColorRange>();

            int maxnewcols;
            int counter, colourrangecounter = 0, colourlistindex = 0;

            _colourRand = new Random();

            Set_PrimaryColour_Choice();

            if (Hits._maxHitsCounted == false) Create_Image.Find_Max_Hits();

            maxnewcols = (int)Create_Image._maxHits;

            if (maxnewcols < 256) maxnewcols = 256;
            else if (maxnewcols > 2048) maxnewcols = 2048;

            Color color;

            colorlist.Add(new ColorRange(0, Get_RandomColour(true)));
            
            while (colourrangecounter < (int)maxnewcols)
            {
               
                color = Get_RandomColour(luminanceflip);
                luminanceflip = !luminanceflip;

                counter = Max_Difference_Between(colorlist[colourlistindex++]._color, color);

                colourrangecounter += counter;

                colorlist.Add(new ColorRange(counter, color));

            }

            Add_New_RandomColorSet(new Color_Set(new ColorRanges(colorlist)));
        }

        private static Color Get_RandomColour()
		{
            return new Color(
                _colourRand.Next(256),
                _colourRand.Next(256),
                _colourRand.Next(256)
                ,255);
        }

        private static Color Get_RandomColour(bool isDark)
        {
            Color color;

            bool getnewcolor = true;
            
			do
			{
                color = new Color(
                    _colourRand.Next(256), _colourRand.Next(256),
                    _colourRand.Next(256) , 255);

                if (isDark)
				{
                    if (RGBLuminance(color) < 0.25f) getnewcolor = false;
				}
                else
				{
                    if (RGBLuminance(color) > 0.75f) getnewcolor = false;
                }

            } while (getnewcolor);

            return color;
        }
        private static float RGBLuminance(Color color)
		{
            float div = 1.0f / 255.0f;

            return 0.2126f * (color.R * div) + 0.7152f * (color.G * div) + 0.0722f * (color.B * div);
        }

        private static int Max_Difference_Between(Color color1, Color color2)
		{
            int dif = MathHelper.Max(
                Math.Abs((int)color1.R - (int)color2.R),
                Math.Abs((int)color1.B - (int)color2.B));

            dif = MathHelper.Max(
                dif,
                Math.Abs((int)color1.G - (int)color2.G));

            return dif;
        }


        internal static void Add_New_RandomColorSet(Color_Set colorset)
		{
            if (!_randomSpreadExists)
            {
                My_Layouts.Show_Button(16);
                My_Layouts.Show_Button(17);
                My_Layouts.Show_Button(18);
                _randomSpreadExists = true;
            }
            else _colorSets.RemoveAt(_colorSets.Count - 1);

            _colorSets.Add(colorset);
            _colorSetsNew.Add(colorset);

            if (_colorSetsNew.Count > 50) _colorSetsNew.RemoveAt(0);

            _colorSetNewID = _colorSetsNew.Count - 1;

            _currentColorSetID = _colorSets.Count - 1;
        }

        internal static bool Del_New_RandomColorSet()
        {
            if (_colorSetsNew.Count == 0) return false;

            _colorSets.RemoveAt(_colorSets.Count - 1);
            _colorSetsNew.RemoveAt(_colorSetNewID);

            if (--_colorSetNewID < 0) _colorSetNewID = _colorSetsNew.Count - 1;

            if (_colorSetsNew.Count == 0)
            {
                _currentColorSetID = _colorSetID;

                My_Layouts.Grey_Button(16);
                My_Layouts.Grey_Button(17);
                My_Layouts.Grey_Button(18);

                _randomSpreadExists = false;
            }
            else _currentColorSetID = _colorSetNewID;

            return true;
        }


        internal static void Set_RandomCol_Current(int index)
		{
            _colorSetNewID = index;

            _colorSets.RemoveAt(_colorSets.Count - 1);

            _colorSets.Add(_colorSetsNew[_colorSetNewID]);

            _currentColorSetID = _colorSets.Count - 1;

            _usingRandomSpread = true;
        }


        internal static void Set_PrimaryColour_Choice()
		{

            int primarychoice = _colourRand.Next(4);

            switch (primarychoice)
            {
                case 0:
                    PrimaryColors = PrimaryColors1;
                    PrimaryColourCount = PrimaryColourCount1;
                    break;
                case 1:
                    PrimaryColors = PrimaryColors2;
                    PrimaryColourCount = PrimaryColourCount2;
                    break;
                case 2:
                    PrimaryColors = PrimaryColors3;
                    PrimaryColourCount = PrimaryColourCount3;
                    break;
                case 3:
                    PrimaryColors = PrimaryColors4;
                    PrimaryColourCount = PrimaryColourCount4;
                    break;
                default:
                    break;

            }
        }
   
        internal static bool Set_CurrentColourSet(int ID)
		{
            //if (_currentColorSetID == ID) return false;

            _currentColorSetID = ID;

            _currentColorSet = _colorSets[_currentColorSetID];

            _colorSpread = _currentColorSet._colorSpread;

            _currentSpreadCount = _currentColorSet._colorSpreadCount - 1;

            return true;
   //         if (_currentSpreadCount > _adjustSpreadCount)
			//{
   //             Set_Vertices();
   //         }
        }
    }
}