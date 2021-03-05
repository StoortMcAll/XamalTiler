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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XamalTiler
{

    internal static class MyFonts
    {
        #region Variable Declaration

        internal const int FontCount = 1;

        const string FontName = "ButtonFont";

        static readonly bool[] _isYCentral = new bool[FontCount] {  true };
        static readonly string[] _fontSizes = new string[FontCount] { "30" };

        internal static MyFont _currentMyFont;
        internal static SpriteFont _currentSpriteFont;
        internal static int _currentMyFontID;
        internal static Texture2D[] _undoArrows = new Texture2D[2];

        internal static MyFont[] _myFonts;

        #endregion


        internal static void Initialise(ContentManager content)
        {
            _myFonts = new MyFont[FontCount];

            for (int i = 0; i < FontCount; i++)
                _myFonts[i] =
                    new MyFont(
                        i,
                        content.Load<SpriteFont>(FontName + _fontSizes[i]),
                        _isYCentral[i]);
        }



        //internal static float Best_Fit_To_CurrentFont(float buttonHeight)
        //{
        //    int id = 0;

        //    Set_CurrentFont(id);

        //    float fontscale = 1.0f;

        //    while (buttonHeight - 4 < _currentMyFont._font.MeasureString("Test Gj_ght").Y * fontscale)
        //        fontscale *= 0.99f;

        //    return fontscale;
        //}


        internal static MyFont Set_CurrentFont(int ID)
        {
            if (ID < 0 || ID > FontCount - 1) ID = 0;

            _currentMyFontID = ID;
            _currentMyFont = _myFonts[ID];

            _currentSpriteFont = _currentMyFont._font;

            return _currentMyFont;
        }


        internal class MyFont
        {
            #region Variable Declaration

            internal int _ID;

            internal SpriteFont _font;

            internal int _YDivisor;

            internal float _fontHeight;

            #endregion


            internal MyFont(int id, SpriteFont font, bool isYCentral)
            {
                _ID = id;

                _font = font;

                if (isYCentral) _YDivisor = 2;
                else _YDivisor = 3;

                _fontHeight = _font.MeasureString("QQQQ").Y * (2.0f / _YDivisor);
            }

            internal Vector2 FindLocationInRect(Rectangle rect, string text)
            {
                Vector2 size = _font.MeasureString(text);

                return new Vector2(rect.Center.X - size.X / 2, rect.Center.Y - size.Y / _YDivisor);
            }
        }
    }

}