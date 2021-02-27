using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using static XamalTiler.Game1;
using static XamalTiler.Create_Image;
using static XamalTiler.Colour_Class;

namespace XamalTiler
{
    internal static class Initialise
	{
        #region Set  Values

        internal enum RandomiseLevel { Low, Medium, High }

        internal static RandomiseLevel _randomiseLevel;


		internal static void Add_Preset_Colors()
        {
            _colorSets.Add(new Color_Set(//Black and White
               new ColorRanges(
                   new List<ColorRange>() {
                        new ColorRange(0, Color.Black),
                        new ColorRange(32, Color.WhiteSmoke),
                        new ColorRange(32, Color.Black),
                        new ColorRange(64, Color.WhiteSmoke),
                        new ColorRange(64, Color.Black),
                        new ColorRange(64, Color.WhiteSmoke),
                        new ColorRange(128, Color.Black),
                        new ColorRange(128, Color.WhiteSmoke)})));

            _colorSets.Add(new Color_Set(
                new ColorSpreadList(
                    new List<Color>() {
                       Color.Black, Color.WhiteSmoke })));

            _colorSets.Add(new Color_Set(//Black and Red
               new ColorRanges(
                   new List<ColorRange>() {
                        new ColorRange(0, Color.Black),
                        new ColorRange(32, Color.Red),
                        new ColorRange(32, Color.Black),
                        new ColorRange(64, Color.Yellow),
                        new ColorRange(64, Color.Black),
                        new ColorRange(64, Color.Red),
                        new ColorRange(128, Color.Black),
                        new ColorRange(128, Color.Yellow)})));

            _colorSets.Add(new Color_Set(//Black and Red
               new ColorRanges(
                   new List<ColorRange>() {
                        new ColorRange(0, Color.Black),
                        new ColorRange(32, Color.Red),
                        new ColorRange(256, Color.Yellow),
                        new ColorRange(1024, Color.White) })));

            _colorSets.Add(new Color_Set(
                new ColorSpreadList(
                    new List<Color>() {
                       Color.Black, Color.Red, Color.Yellow })));

            _colorSets.Add(new Color_Set(//Black and Blue
               new ColorRanges(
                   new List<ColorRange>() {
                        new ColorRange(0, Color.Black),
                        new ColorRange(32, Color.Blue),
                        new ColorRange(32, Color.Black),
                        new ColorRange(64, Color.Blue),
                        new ColorRange(64, Color.Black),
                        new ColorRange(64, Color.Blue),
                        new ColorRange(128, Color.Black),
                        new ColorRange(128, Color.Blue)})));

            _colorSets.Add(new Color_Set(
                new ColorSpreadList(
                    new List<Color>() {
                       Color.Black, Color.Blue })));

            _colorSets.Add(new Color_Set(//Black and Green
               new ColorRanges(
                   new List<ColorRange>() {
                        new ColorRange(0, Color.Black),
                        new ColorRange(32, Color.Green),
                        new ColorRange(32, Color.Black),
                        new ColorRange(64, Color.Green),
                        new ColorRange(64, Color.Black),
                        new ColorRange(64, Color.Green),
                        new ColorRange(128, Color.Black),
                        new ColorRange(128, Color.Green)})));

            _colorSets.Add(new Color_Set(
                new ColorSpreadList(
                    new List<Color>() {
                       Color.Black, Color.Green })));

            _colorSets.Add(new Color_Set(//Black and White 2
              new ColorRanges(
                  new List<ColorRange>() {
                        new ColorRange(0, Color.Black),
                        new ColorRange(32, Color.WhiteSmoke),
                        new ColorRange(64, Color.Black),
                        new ColorRange(128, Color.WhiteSmoke),
                        new ColorRange(256, Color.Black),
                        new ColorRange(512, Color.WhiteSmoke),
                        new ColorRange(512, Color.Black),
                        new ColorRange(512, Color.WhiteSmoke)})));


            _colorSets.Add(new Color_Set(//Black - Red - Yellow - White - Yellow ....
                new ColorRanges(
                    new List<ColorRange>() {
                        new ColorRange(0, Color.Black),
                        new ColorRange(32, Color.Red),
                        new ColorRange(48, Color.Yellow),
                        new ColorRange(24, Color.White),
                        new ColorRange(24, Color.Yellow),
                        new ColorRange(48, Color.Red),
                        new ColorRange(48, Color.Black),
                        new ColorRange(64, Color.Red),
                        new ColorRange(96, Color.Yellow),
                        new ColorRange(32, Color.White),
                        new ColorRange(64, Color.Yellow),
                        new ColorRange(128, Color.Red),
                        new ColorRange(128, Color.Black),
                        new ColorRange(128, Color.Red),
                        new ColorRange(256, Color.Yellow),
                        new ColorRange(64, Color.White)})));



            _colorSets.Add(new Color_Set(//Red - Gold - Green
                new ColorRanges(
                    new List<ColorRange>() {
                        new ColorRange(0, new Color(16, 0, 0)),
                        new ColorRange(64, Color.Red),
                        new ColorRange(2, new Color(0, 2, 0)),
                        new ColorRange(64, Color.Green),
                        new ColorRange(2, new Color(0, 0, 2)),
                        new ColorRange(64, Color.Blue)
                    })));


            _colorSets.Add(new Color_Set(
                new ColorSpreadList(
                    new List<Color>() {
                       Color.DarkSlateGray, new Color(0, 32, 84), new Color(0, 12, 4),
                       new Color(46, 22, 137), Color.SaddleBrown, Color.DarkCyan,
                       Color.Salmon, Color.WhiteSmoke})));


            _colorSets.Add(new Color_Set(
                new ColorSpreadList(
                    new List<Color>() {
                       Color.DarkRed, new Color(0, 2, 4), Color.Silver, Color.LimeGreen,
                       Color.DeepSkyBlue, Color.Crimson, Color.Orange, Color.DarkOrange,
                       Color.Yellow})));


            _colorSets.Add(new Color_Set(
                new ColorRanges(
                    new List<ColorRange>() {
                        new ColorRange(0, new Color(0, 4, 32)),
                        new ColorRange(32, new Color(76, 31, 42)),
                        new ColorRange(32, new Color(46, 97, 14)),
                        new ColorRange(64, Color.MediumPurple),
                        new ColorRange(64, Color.ForestGreen),
                        new ColorRange(128, Color.DeepSkyBlue),
                        new ColorRange(128, Color.IndianRed),
                        new ColorRange(32, new Color(0, 2, 6)),
                        new ColorRange(128, Color.OrangeRed),
                        new ColorRange(32, Color.Red),
                        new ColorRange(32, Color.Yellow)})));


            _colorSets.Add(new Color_Set(
                 new ColorRanges(
                     new List<ColorRange>() {
                        new ColorRange(0, new Color(16, 0, 0)),
                        new ColorRange(64, Color.OrangeRed),
                        new ColorRange(64, Color.DarkSlateGray),
                        new ColorRange(32, Color.DarkRed),
                        new ColorRange(64, Color.Orange),
                        new ColorRange(128, Color.LightGoldenrodYellow),
                        new ColorRange(64, new Color(16, 0, 0))})));


            _currentColorSet = _colorSets[_currentColorSetID];

            _colorSpread = _currentColorSet._colorSpread;
        }


		internal static void Preset_Values()
        {
            _square = new SquareValues(
                _rand.NextDouble(),
                _rand.NextDouble(),
                0.2, 0.1, -0.9, -0.59, 0.05, -0.34);
           
            _itrigValues = new IconTrigValues(_square.trig);
        }


        internal static void RandomAll()
        {
            double _a1Test, _a2Test, _a3Test;

            _rand = new Random();

            Clear_Data();


			switch (_quiltType)
			{
				case QuiltType.Square:
                    _square = new SquareValues(0.1, 0.334, 0.2, 0.1, -0.9, -0.59, 0.05, -0.34);

                    Generate_Random_Set();

                    break;
				case QuiltType.Hexagon:
                    _square = new SquareValues(0.1, 0.3, -0.1, -0.076, 0.0, -0.59, 0.0, 0.0);

                    Generate_Random_Hexa_Set();

                    _hexValues = new HexagonValues(_square.beta, _square.gamma);

                    break;
				case QuiltType.Icon:
					do
					{
                        _square = new SquareValues(0.1, -0.1, 0.3, 0.65, 0.43, 0.4, 0.0, 0.9, 24);

                        Generate_Random_Set();

                        _a1Test = _square.alpha * _square.alpha + _square.gamma * _square.gamma;
                        _a2Test = _square.beta * _square.beta + _square.lambda * _square.lambda;
                        _a3Test = _square.alpha * _square.lambda - _square.beta * _square.gamma;
                    } while (
                        _a1Test > 1 || _a2Test > 1 || 
                        (_a1Test + _a2Test) > 1 - Math.Pow(_a3Test, 2));

                    break;
				default:
					break;
			}

        }


        internal static void Generate_Random_Set()
        {
            _randomiseLevel = (RandomiseLevel)_rand.Next(3);

            switch (_randomiseLevel)
            {
                case RandomiseLevel.Low:

                    for (int i = 0; i < 7; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                if (TrueNotFalse)
                                    _square.alpha += Get_6DP_Random_Value(-2.0, 4.0);
                                break;
                            case 1:
                                if (TrueNotFalse)
                                    _square.beta += Get_6DP_Random_Value(-1.0, 2.0);
                                break;
                            case 2:
                                if (TrueNotFalse)
                                    _square.gamma += Get_6DP_Random_Value(-1.0, 2.0);
                                break;
                            case 3:
                                if (TrueNotFalse)
                                    _square.lambda += Get_6DP_Random_Value(-1.0, 2.0);
                                break;
                            case 4:
                                if (TrueNotFalse)
                                    _square.ma += Get_6DP_Random_Value(-1.0, 2.0);
                                break;
                            case 5:
                                if (TrueNotFalse)
                                    _square.omega += Get_6DP_Random_Value(-1.0, 2.0);
                                break;
                            case 6:
                                if (TrueNotFalse)
                                    _square.trig += (int)Get_6DP_Random_Value(-3, 6);

                                _itrigValues = new IconTrigValues(_square.trig);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case RandomiseLevel.Medium:
                    for (int i = 0; i < 7; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                _square.alpha += Get_6DP_Random_Value(-2.0, 4.0);
                                break;
                            case 1:
                                _square.beta += Get_6DP_Random_Value(-1.0, 2.0);
                                break;
                            case 2:
                                _square.gamma += Get_6DP_Random_Value(-1.0, 2.0);
                                break;
                            case 3:
                                _square.lambda += Get_6DP_Random_Value(-1.0, 2.0);
                                break;
                            case 4:
                                _square.ma += Get_6DP_Random_Value(-1.0, 2.0);
                                break;
                            case 5:
                                _square.omega += Get_6DP_Random_Value(-1.0, 2.0);
                                break;
                            case 6:
                                _square.trig += (int)Get_6DP_Random_Value(-6, 12);

                                _itrigValues = new IconTrigValues(_square.trig);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case RandomiseLevel.High:
                    for (int i = 0; i < 7; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                _square.alpha += Get_6DP_Random_Value(-3.0, 6.0);
                                break;
                            case 1:
                                _square.beta += Get_6DP_Random_Value(-2.0, 4.0);
                                break;
                            case 2:
                                _square.gamma += Get_6DP_Random_Value(-2.0, 4.0);
                                break;
                            case 3:
                                _square.lambda += Get_6DP_Random_Value(-2.0, 4.0);
                                break;
                            case 4:
                                _square.ma += Get_6DP_Random_Value(-2.0, 4.0);
                                break;
                            case 5:
                                _square.omega += Get_6DP_Random_Value(-2.0, 4.0);
                                break;
                            case 6:
                                _square.trig += (int)Get_6DP_Random_Value(-6, 18);

                                _itrigValues = new IconTrigValues(_square.trig);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        internal static void Generate_Random_Hexa_Set()
        {
            _randomiseLevel = (RandomiseLevel)_rand.Next(2);

            switch (_randomiseLevel)
            {
                case RandomiseLevel.Low:

                    for (int i = 0; i < 6; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                if (TrueNotFalse)
                                    _square.alpha += Get_6DP_Random_Value(-0.5, 1.0);
                                break;
                            case 1:
                                if (TrueNotFalse)
                                    _square.beta += Get_6DP_Random_Value(-0.5, 1.0);
                                break;
                            case 2:
                                if (TrueNotFalse)
                                    _square.gamma += Get_6DP_Random_Value(-0.25, 0.5);
                                break;
                            case 3:
                                if (TrueNotFalse)
                                    _square.lambda += Get_6DP_Random_Value(-0.5, 1.0);
                                break;
                            case 4:
                                if (TrueNotFalse)
                                    _square.ma += Get_6DP_Random_Value(-0.25, 0.5);
                                break;
                            case 5:
                                if (TrueNotFalse)
                                    _square.omega += Get_6DP_Random_Value(-0.25, 0.5);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case RandomiseLevel.Medium:
                    for (int i = 0; i < 7; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                if (TrueNotFalse)
                                    _square.alpha += Get_6DP_Random_Value(-1.0, 2.0);
                                break;
                            case 1:
                                if (TrueNotFalse)
                                    _square.beta += Get_6DP_Random_Value(-1.0, 2.0);
                                break;
                            case 2:
                                if (TrueNotFalse)
                                    _square.gamma += Get_6DP_Random_Value(-0.5, 1.0);
                                break;
                            case 3:
                                if (TrueNotFalse)
                                    _square.lambda += Get_6DP_Random_Value(-1.0, 2.0);
                                break;
                            case 4:
                                if (TrueNotFalse)
                                    _square.ma += Get_6DP_Random_Value(-0.5, 1.0);
                                break;
                            case 5:
                                if (TrueNotFalse)
                                    _square.omega += Get_6DP_Random_Value(-0.5, 1.0);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case RandomiseLevel.High:
                    for (int i = 0; i < 7; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                _square.alpha += Get_6DP_Random_Value(-3.0, 6.0);
                                break;
                            case 1:
                                _square.beta += Get_6DP_Random_Value(-2.0, 4.0);
                                break;
                            case 2:
                                _square.gamma += Get_6DP_Random_Value(-2.0, 4.0);
                                break;
                            case 3:
                                _square.lambda += Get_6DP_Random_Value(-2.0, 4.0);
                                break;
                            case 4:
                                _square.ma += Get_6DP_Random_Value(-2.0, 4.0);
                                break;
                            case 5:
                                _square.omega += Get_6DP_Random_Value(-2.0, 4.0);
                                break;
                            case 6:
                                _square.trig += (int)Get_6DP_Random_Value(3, 49);

                                _itrigValues = new IconTrigValues(_square.trig);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }


        internal static double Get_6DP_Random_Value(double lhs = 1.0, double range = 0.0)
        {
            return lhs + (double)_rand.NextDouble() * range;
        }


        internal static void Set_Trig_Values(int value)
        {
                _square.trig = value;

                _itrigValues.c = new double[_square.trig];
                _itrigValues.s = new double[_square.trig];

                double mult = 1.0d / (double)_square.trig;

                for (int i = 0; i < _square.trig; i++)
                {
                    _itrigValues.c[i] = Math.Cos(2 * Math.PI * i * mult);
                    _itrigValues.s[i] = Math.Sin(2 * Math.PI * i * mult);
                }

        }



        private static void Clear_Data()
        {
            _maxHits = _iterCounter = _iterations = 0;
            
            _field = new int[_fieldWidth, _fieldHeight];

            _stride = _fieldWidth * 4;

            Array.Clear(_pixelData, 0, _pixelData.Length);

            Hits.ClearList();

            Create_Image.Set_Min_Hits_to_Zero();

            Create_Image.Update_Image_Full();
        }

        #endregion
    }
}