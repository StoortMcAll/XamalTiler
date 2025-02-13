﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using static XamalTiler.Game1;
using static XamalTiler.Iterate_Class;
using static XamalTiler.ScreenShot;
using static XamalTiler.Colour_Class;


namespace XamalTiler
{
	internal static class Create_Image
	{

        #region Variable Declaration

        internal enum ImageDrawStyle { Basic, Smooth, Smooth2, Average }
      
        internal static ImageDrawStyle _imageDrawStyle = ImageDrawStyle.Basic;
        internal static ColourType _colourType = ColourType.Linear;

        internal static byte[] _pixelData;

        internal static long _minHits, _maxHits, tempID = -1;

        internal static int[] _fPosx, _fPosy;

        internal static bool _updateAll;

        internal static GraphicsDevice _graphicsDevice;
        internal static SpriteBatch _spriteBatch;


        internal static List<Rectangle> _outRects = new List<Rectangle>();

        #endregion


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

                    _currentColorSet = _colorSets[_currentColorSetID];

                    _colorSpread = _currentColorSet._colorSpread;
                }
                _tileTexture.SetData(_pixelData);

                _drawImageTarget = true;
            }
        }

        internal static void Set_Min_Hits_to_Zero()
		{
            _minHits = 0;
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


        internal static void Update_Image_Full()
		{
            _currentColorSet = _colorSets[_currentColorSetID];

            _colorSpread = _currentColorSet._colorSpread;

            _currentSpreadCount = _currentColorSet._colorSpreadCount - 1;

            float dhits = _maxHits == 0 ? 1 : _currentSpreadCount / (float)_maxHits;

            int index = 0;

            for (int y = 0; y < _fieldHeight; y++)
            {
                for (int x = 0; x < _fieldWidth; x++)
                {
                    int hits = _field[x, y];

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

                    //var index = y * stride + x * 4;

                    _pixelData[index++] = _colorSpread[hits].R;
                    _pixelData[index++] = _colorSpread[hits].G;
                    _pixelData[index++] = _colorSpread[hits].B;
                    _pixelData[index++] = 255;
                }
            }

            _tileTexture.SetData(_pixelData);

        }

        internal static void Update_Image_Full_Smooth()
        {
            int index = 0;

            _currentColorSet = _colorSets[_currentColorSetID];

            _colorSpread = _currentColorSet._colorSpread;

            _currentSpreadCount = _currentColorSet._colorSpreadCount - 1;

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

                    //var index = _fPosx[y] * stride + _fPosx[x] * 4;

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

            _currentColorSet = _colorSets[_currentColorSetID];

            _colorSpread = _currentColorSet._colorSpread;

            _currentSpreadCount = _currentColorSet._colorSpreadCount - 1;

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
                    int hits = 0; 

                    hits += (int)(_field[_fPosx[x - 1], _fPosx[y]]); // / 2); // 50 %
                    hits += (int)(_field[_fPosx[x + 1], _fPosx[y]]); // / 2); // 50 %
                    hits += (int)(_field[_fPosx[x], _fPosx[y - 1]]); // / 2); // 50 %
                    hits += (int)(_field[_fPosx[x], _fPosx[y + 1]]); // / 2); // 50 %

                    hits += (int)(_field[_fPosx[x - 1], _fPosx[y - 1]]); // / 4); // 25 %
                    hits += (int)(_field[_fPosx[x + 1], _fPosx[y - 1]]); // / 4); // 25 %
                    hits += (int)(_field[_fPosx[x - 1], _fPosx[y + 1]]); // / 4); // 25 %
                    hits += (int)(_field[_fPosx[x + 1], _fPosx[y + 1]]); // / 4); // 25 %

                    hits /= 8;

                    hits += _field[_fPosx[x], _fPosx[y]]; // 100 %

                    hits /= 2;

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

                    //var index = _fPosx[y] * stride + _fPosx[x] * 4;

                    _pixelData[index++] = _colorSpread[hits].R;
                    _pixelData[index++] = _colorSpread[hits].G;
                    _pixelData[index++] = _colorSpread[hits].B;
                    _pixelData[index++] = 255;
                }
            }

            _tileTexture.SetData(_pixelData);
        }


        internal static void Update_Image_Full_Average()
        {
            int index = 0;

            int r, g, b, dxc;

            Color color;

            _currentColorSet = _colorSets[_currentColorSetID];

            _colorSpread = _currentColorSet._colorSpread;

            _currentSpreadCount = _currentColorSet._colorSpreadCount - 1;

            float dhits = _maxHits == 0 ? 1 : _currentSpreadCount / (float)_maxHits;

            _fPosx = new int[_fieldWidth + 2];
            _fPosy = new int[_fieldHeight + 2];

            _fPosx[0] = _fieldWidth - 1;
            _fPosx[_fieldWidth + 1] = 0;
            for (int x = 1; x < _fieldWidth + 1; x++) _fPosx[x] = x - 1;

            _fPosy[0] = _fieldHeight - 1;
            _fPosy[_fieldHeight + 1] = 0;

            int[] hitstore = new int[9];

            int[] dxcol = new int[9] { 1, 2, 2, 2, 2, 4, 4, 4, 4 };

            for (int y = 1; y < _fieldHeight + 1; y++) _fPosy[y] = y - 1;

            for (int y = 1; y < _fieldHeight + 1; y++)
            {
                for (int x = 1; x < _fieldWidth + 1; x++)
                {
					hitstore[0] = _field[_fPosx[x], _fPosx[y]]; // 100 %

					hitstore[1] = (int)(_field[_fPosx[x - 1], _fPosx[y]]); // 100 %
					hitstore[2] = (int)(_field[_fPosx[x + 1], _fPosx[y]]); // 100 %
					hitstore[3] = (int)(_field[_fPosx[x], _fPosx[y - 1]]); // 100 %
					hitstore[4] = (int)(_field[_fPosx[x], _fPosx[y + 1]]); // 100 %

					hitstore[5] = (int)(_field[_fPosx[x - 1], _fPosx[y - 1]]); // 100 %
					hitstore[6] = (int)(_field[_fPosx[x + 1], _fPosx[y - 1]]); // 100 %
					hitstore[7] = (int)(_field[_fPosx[x - 1], _fPosx[y + 1]]); // 100 %
					hitstore[8] = (int)(_field[_fPosx[x + 1], _fPosx[y + 1]]); // 100 %

					for (int i = 0; i < 9; i++)
                    {

                        switch (_colourType)
                        {
                            case ColourType.Linear:
                                break;
                            case ColourType.SquareRoot:
                                hitstore[i] = (int)Math.Sqrt(hitstore[i]);
                                break;
                            case ColourType.Stretch:
                                hitstore[i] = (int)(hitstore[i] * dhits);
                                break;
                            default:
                                break;
                        }

                        if (hitstore[i] > _currentSpreadCount)
                            hitstore[i] = _currentSpreadCount;
                    }

                    r = _colorSpread[hitstore[0]].R;
                    g = _colorSpread[hitstore[0]].G;
                    b = _colorSpread[hitstore[0]].B;

                    for (int i = 1; i < 9; i++)
					{
                        dxc = dxcol[i];
                        color = _colorSpread[hitstore[i]];
                        r += color.R / dxc;
                        g += color.G / dxc;
                        b += color.B / dxc;
                    }

                    r /= 4;g /= 4;b /= 4;
                   

                    if (r < 0) r = 0;
                    else if (r > 255) r = 255;
                    if (g < 0) g = 0;
                    else if (g > 255) g = 255;
                    if (b < 0) b = 0;
                    else if (b > 255) b = 255;

                    _pixelData[index++] = (byte)r;
                    _pixelData[index++] = (byte)g;
                    _pixelData[index++] = (byte)b;
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

            _outRects.Clear();

            Rectangle outRect = _tileTexture.Bounds;
            outRect.Size = (outRect.Size.ToVector2() * _imageScale).ToPoint();
            outRect.Location = _imageOffset.ToPoint();

            _spriteBatch.Begin();

            do
            {
                outRect.X = (int)_imageOffset.X - 10;

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
    }
}