using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using static XamalTiler.Game1;
using static XamalTiler.Iterate_Class;
using static XamalTiler.ScreenShot;
using static XamalTiler.Colour_Class;
using zzz;

namespace XamalTiler
{
	internal static class Create_Image
	{

        #region Variable Declaration

        internal enum ImageDrawStyle { Sharp, LowBlur, MidBlur, HiBlur } 
      
        internal static ImageDrawStyle _imageDrawStyle = ImageDrawStyle.Sharp;
        internal static ColourType _colourType = ColourType.Linear;

        internal static byte[] _pixelData, _pixelDataStore;

        internal static long _minHits, _maxHits, tempID = -1;

        internal static int[] _fPosx, _fPosy;

        internal static bool _updateAll;

        internal static GraphicsDevice _graphicsDevice;

        internal static SpriteBatch _spriteBatch;

        internal static List<Rectangle> _outRects = new List<Rectangle>();


        internal static Task<byte[]> rutDel = Run_UpdateImage_Task();
        internal static bool _fieldLock = false, _initRUTDelegate = false;


        #endregion



        internal static Task<byte[]> Run_UpdateImage_Task()
		{
            _fieldLock = true;
            return Task.Run(() => Update_Image_Full_Task());
        }

        internal static byte[] Update_Image_Full_Task()
        {
            Color color;
            int index = 0;

            int hits;
            float dhits = _maxHits == 0 ? 1 : _currentSpreadCount / (float)_maxHits;

            for (int y = 0; y < _fieldHeight; y++)
            {
                for (int x = 0; x < _fieldWidth; x++)
                {
                    hits = _field[x, y];

                    switch (_colourType)
                    {
                        case ColourType.Linear:
                            break;
                        case ColourType.SquareRoot:
                            hits = (int)Math.Sqrt(hits);
                            break;
                        case ColourType.Stretch:
                            hits = (int)(hits * dhits);
                            break;
                        default:
                            break;
                    }

                    if (hits > _currentSpreadCount)
                        hits = _currentSpreadCount;

                    color = _adjustSpreadColors[hits];

                    _pixelData[index++] = color.R;
                    _pixelData[index++] = color.G;
                    _pixelData[index++] = color.B;
                    _pixelData[index++] = 255;
                }
            }

            _tileTexture.SetData(_pixelData);

            return _pixelDataStore;

        }



        internal static void Update_Image_Full()
        {
            Color color;
            int index = 0;

            Set_CurrentColourSet(_currentColorSetID);
            Colour_Class.Set_Vertices();

            int hits;
            float dhits = _maxHits == 0 ? 1 : _currentSpreadCount / (float)_maxHits;

            for (int y = 0; y < _fieldHeight; y++)
            {
                for (int x = 0; x < _fieldWidth; x++)
                {
                    hits = _field[x, y];

                    switch (_colourType)
                    {
                        case ColourType.Linear:
                            break;
                        case ColourType.SquareRoot:
                            hits = (int)Math.Sqrt(hits);
                            break;
                        case ColourType.Stretch:
                            hits = (int)(hits * dhits);
                            break;
                        default:
                            break;
                    }

                    if (hits > _currentSpreadCount)
                        hits = _currentSpreadCount;

                    color = _adjustSpreadColors[hits];

                    _pixelData[index++] = color.R;
                    _pixelData[index++] = color.G;
                    _pixelData[index++] = color.B;
                    _pixelData[index++] = 255;
                }
            }

            _tileTexture.SetData(_pixelData);

        }


        internal static void Update_Image_Full_Smooth()
        {
            int index = 0;

            Set_CurrentColourSet(_currentColorSetID);
            Set_Vertices();

            float dhits = _maxHits == 0 ? 1 : _currentSpreadCount / (float)_maxHits;

            _fPosx = new int[_fieldWidth + 2];
            _fPosy = new int[_fieldHeight + 2];

            _fPosx[0] = _fieldWidth - 1;
            _fPosx[_fieldWidth + 1] = 0;
            for (int x = 1; x < _fieldWidth + 1; x++) _fPosx[x] = x - 1;

            _fPosy[0] = _fieldHeight - 1;
            _fPosy[_fieldHeight + 1] = 0;
            for (int y = 1; y < _fieldHeight + 1; y++) _fPosy[y] = y - 1;

            for (int y = 1; y < _fieldHeight + 1; y++)
            {
                for (int x = 1; x < _fieldWidth + 1; x++)
                {
                    int hits = _field[_fPosx[x], _fPosx[y]]; // 100 %

                    hits += (int)(_field[_fPosx[x - 1], _fPosx[y]] / 2); // 50 %
                    hits += (int)(_field[_fPosx[x + 1], _fPosx[y]] / 2); // 50 %
                    hits += (int)(_field[_fPosx[x], _fPosx[y - 1]] / 2); // 50 %
                    hits += (int)(_field[_fPosx[x], _fPosx[y + 1]] / 2); // 50 %

                    hits += (int)(_field[_fPosx[x - 1], _fPosx[y - 1]] / 4); // 25 %
                    hits += (int)(_field[_fPosx[x + 1], _fPosx[y - 1]] / 4); // 25 %
                    hits += (int)(_field[_fPosx[x - 1], _fPosx[y + 1]] / 4); // 25 %
                    hits += (int)(_field[_fPosx[x + 1], _fPosx[y + 1]] / 4); // 25 %

                    hits /= 4;

                    switch (_colourType)
                    {
                        case ColourType.Linear:
                            break;
                        case ColourType.SquareRoot:
                            hits = (int)Math.Sqrt(hits);
                            break;
                        case ColourType.Stretch:
                            hits = (int)(hits * dhits);
                            break;
                        default:
                            break;
                    }

                    if (hits > _currentSpreadCount)
                        hits = _currentSpreadCount;

                    hits = _adjustSpreadVertices[hits];
                    
                    _pixelData[index++] = _colorSpread[hits].R;
                    _pixelData[index++] = _colorSpread[hits].G;
                    _pixelData[index++] = _colorSpread[hits].B;
                    _pixelData[index++] = 255;
                }
            }

            _tileTexture.SetData(_pixelData);
        }

        internal static void Update_Image_Full_Smooth2()
        {
            int index = 0;

            Set_CurrentColourSet(_currentColorSetID);
            Colour_Class.Set_Vertices();

            float dhits = _maxHits == 0 ? 1 : _currentSpreadCount / (float)_maxHits;

            _fPosx = new int[_fieldWidth + 2];
            _fPosy = new int[_fieldHeight + 2];

            _fPosx[0] = _fieldWidth - 1;
            _fPosx[_fieldWidth + 1] = 0;
            for (int x = 1; x < _fieldWidth + 1; x++) _fPosx[x] = x - 1;

            _fPosy[0] = _fieldHeight - 1;
            _fPosy[_fieldHeight + 1] = 0;
            for (int y = 1; y < _fieldHeight + 1; y++) _fPosy[y] = y - 1;

            for (int y = 1; y < _fieldHeight + 1; y++)
            {
                for (int x = 1; x < _fieldWidth + 1; x++)
                {
                    int hits = (int)
                        (
                            (
                            _field[_fPosx[x - 1], _fPosx[y]] +
                            _field[_fPosx[x + 1], _fPosx[y]] +
                            _field[_fPosx[x], _fPosx[y - 1]] +
                            _field[_fPosx[x], _fPosx[y + 1]]
                            )
                        / 4.0f
                        ) * 2;

                    hits += (int)
                        (
                            (
                            _field[_fPosx[x - 1], _fPosx[y - 1]] +
                            _field[_fPosx[x + 1], _fPosx[y - 1]] +
                            _field[_fPosx[x - 1], _fPosx[y + 1]] +
                            _field[_fPosx[x + 1], _fPosx[y + 1]]
                            )
                        / 4.0f
                        );

                   

                    hits += _field[_fPosx[x], _fPosx[y]] * 4;

                    hits /= 7;

                    switch (_colourType)
                    {
                        case ColourType.Linear:
                            break;
                        case ColourType.SquareRoot:
                            hits = (int)Math.Sqrt(hits);
                            break;
                        case ColourType.Stretch:
                            hits = (int)(hits * dhits);
                            break;
                        default:
                            break;
                    }

                    if (hits > _currentSpreadCount)
                        hits = _currentSpreadCount;

                    hits = _adjustSpreadVertices[hits];

                    _pixelData[index++] = _colorSpread[hits].R;
                    _pixelData[index++] = _colorSpread[hits].G;
                    _pixelData[index++] = _colorSpread[hits].B;
                    _pixelData[index++] = 255;
                }
            }

            _tileTexture.SetData(_pixelData);
        }

        internal static void Update_Image_Low_Blur()
        {
            int index = 0, temphit;

            Color color;

            Set_CurrentColourSet(_currentColorSetID);
            Colour_Class.Set_Vertices();

            float dhits = _maxHits == 0 ? 1 : _currentSpreadCount / (float)_maxHits;

            _fPosx = new int[_fieldWidth + 2];
            _fPosy = new int[_fieldHeight + 2];

            _fPosx[0] = _fieldWidth - 1;
            _fPosx[_fieldWidth + 1] = 0;
            for (int x = 1; x < _fieldWidth + 1; x++) _fPosx[x] = x - 1;

            _fPosy[0] = _fieldHeight - 1;
            _fPosy[_fieldHeight + 1] = 0;

            for (int y = 1; y < _fieldHeight + 1; y++) _fPosy[y] = y - 1;

            for (int y = 1; y < _fieldHeight + 1; y++)
            {
                for (int x = 1; x < _fieldWidth + 1; x++)
                {
                    temphit = (int)(_field[_fPosx[x - 1], _fPosx[y]]); // 100 %
                    temphit += (int)(_field[_fPosx[x + 1], _fPosx[y]]); // 100 %
                    temphit += (int)(_field[_fPosx[x], _fPosx[y - 1]]); // 100 %
                    temphit += (int)(_field[_fPosx[x], _fPosx[y + 1]]); // 100 %

                    temphit /= 4;

                    temphit += (_field[_fPosx[x], _fPosx[y]] - temphit) / 2;

                    switch (_colourType)
                    {
                        case ColourType.Linear:
                            break;
                        case ColourType.SquareRoot:
                            temphit = (int)Math.Sqrt(temphit);
                            break;
                        case ColourType.Stretch:
                            temphit = (int)(temphit * dhits);
                            break;
                        default:
                            break;
                    }

                    if (temphit > _currentSpreadCount) temphit = _currentSpreadCount;

                    color = _colorSpread[_adjustSpreadVertices[temphit]];

                    _pixelData[index++] = color.R;
                    _pixelData[index++] = color.G;
                    _pixelData[index++] = color.B;
                    _pixelData[index++] = 255;

                }
            }
            _tileTexture.SetData(_pixelData);
        }


        internal static void Draw_Target()
        {
            Rectangle outRect;
          
            _outRects.Clear();

            _graphicsDevice.SetRenderTarget(_imageRenderTarget);
            _graphicsDevice.Clear(Color.TransparentBlack);

            outRect = _tileTexture.Bounds;
            outRect.Size = (outRect.Size.ToVector2() * _imageScale).ToPoint();
            outRect.Location = _imageOffset.ToPoint();

            _spriteBatch.Begin();

            do
            {
                outRect.X = (int)_imageOffset.X;

                do
                {
                    _outRects.Add(outRect);

                    _spriteBatch.Draw(_tileTexture, outRect, Color.White);

                    outRect.X += outRect.Width;
                } while (outRect.X < _imageRenderTarget.Width);

                outRect.Y += outRect.Height;
            } while (outRect.Y < _imageRenderTarget.Height);

            _spriteBatch.End();

            _drawImageTarget = false;
        }

        internal static void Draw_FullScreen()
        {
            if (_saveImageState != SaveImageState.Ready) return;
           
            _graphicsDevice.SetRenderTarget(_fullScreenTarget);
            _graphicsDevice.Clear(Color.TransparentBlack);

         //   Vector2 fscale = new Vector2(_fullScreenTarget.Width / (float)_imageRenderTarget.Width) * _imageScale;
          
            _outRects.Clear();

            Rectangle outRect = _tileTexture.Bounds;
            outRect.Size = (outRect.Size.ToVector2() * _imageScale).ToPoint();
            outRect.Location = _imageOffset.ToPoint();

            _spriteBatch.Begin();

            do
            {
                outRect.X = (int)_imageOffset.X;

                do
                {

                    _outRects.Add(outRect);

                    _spriteBatch.Draw(_tileTexture, outRect, Color.White);

                    outRect.X += outRect.Width;
                } while (outRect.X < _fullScreenTarget.Width);

                outRect.Y += outRect.Height;
            } while (outRect.Y < _fullScreenTarget.Height);

            _spriteBatch.End();

            _drawImageTarget = false;

        }

        internal static void Draw_Screenshot()
        {
            _graphicsDevice.SetRenderTarget(_screenshotTarget);
            _graphicsDevice.Clear(Color.TransparentBlack);

            Rectangle outRect = _tileSaveTexture.Bounds;
            outRect.Size = (outRect.Size.ToVector2() * _imageScale).ToPoint();
            outRect.Location = _imageOffset.ToPoint();

            _spriteBatch.Begin();

            do
            {
                outRect.X = (int)_imageOffset.X - 10;

                do
                {
                    _spriteBatch.Draw(_tileSaveTexture, outRect, Color.White);

                    outRect.X += outRect.Width;
                } while (outRect.X < _screenshotTarget.Width);

                outRect.Y += outRect.Height;
            } while (outRect.Y < _screenshotTarget.Height);

            _spriteBatch.End();

            _graphicsDevice.SetRenderTarget(null);
        }



        internal static void Poll_FieldData()
        {
            Hits.Add_FieldData_To_PixelData();

            Hits.ClearList();

            _isDataPolled = true;
        }


        internal static void Set_Texture_PixelData()
        {
            if (_updateAll)
            {
                Update_Image_Full();

                _updateAll = false;
            }
            else
            {
                if (tempID != _currentColorSetID)
                {
                    tempID = _currentColorSetID;

                    Set_CurrentColourSet(_currentColorSetID);
                    if (_currentSpreadCount != _adjustSpreadCount)
                        Set_Vertices();
                }
                _tileTexture.SetData(_pixelData);

                _drawImageTarget = true;
            }
        }


        internal static void Find_Min_Hits()
		{
            int hits;
            _minHits = 9999;

            for (int y = 0; y < _fieldHeight; y++)
            {
                for (int x = 0; x < _fieldWidth; x++)
                {
                    hits = _field[x, y];

                    if (hits < _minHits) _minHits = hits;
                }
            }
        }

        internal static void Find_Max_Hits()
        {
            int hits;
            _maxHits = 0;

            for (int y = 0; y < _fieldHeight; y++)
            {
                for (int x = 0; x < _fieldWidth; x++)
                {
                    hits = _field[x, y];

                    if (hits > _maxHits) _maxHits = hits;
                }
            }

            Hits._maxHitsCounted = true;
        }

        internal static void Find_Total_Hits()
        {
            int hits = 0;
            
            for (int y = 0; y < _fieldHeight; y++)
            {
                for (int x = 0; x < _fieldWidth; x++)
                {
                    hits += _field[x, y];
                }
            }

            //if (hits != _iterCounter)
            //    hits -= _iterCounter;
        }

        internal static void Shift_Minimum_Hit_To_Zero()
		{
            for (int y = 0; y < _fieldHeight; y++)
            {
                for (int x = 0; x < _fieldWidth; x++)
                {
                    _field[x, y] -= (int)_minHits;
                }
            }

            Set_Min_Hits_to_Zero();

            Find_Max_Hits();
        }

        internal static void Set_Min_Hits_to_Zero()
		{
            _minHits = 0;
		}

    
    }
}