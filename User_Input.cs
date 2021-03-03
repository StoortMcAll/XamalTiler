//using Android.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using zzz;
using static XamalTiler.Game1;
using static XamalTiler.My_Layouts;

namespace XamalTiler
{
	internal static class User_Input
    {
		#region Variable Declaration

		internal static bool _isBbuttonActive, _updateImageTex;
		static int _buttonID = -1;
		internal static Vector2[] _startVectors = new Vector2[2] { Vector2.Zero, Vector2.Zero };
        
		static float _imageScaleTemp;
		
		internal static GestureSample _gestureSample;
		static internal float _length2 = 0.0f, _length = 0.0f, _dif = 0.0f;
		
		static internal Vector2 _pointLocal = Vector2.Zero, _canvasLocal;

		#endregion


		internal static int Update_Input()
		{
			if (_isBbuttonActive)
			{
				if (_userInputType == UserInputType.Release || !TouchPanel.IsGestureAvailable)
				{
					_isBbuttonActive = false;
					_buttonID = -1;

					return _buttonID;
				}

				while (TouchPanel.IsGestureAvailable)
				{
					_gestureSample = TouchPanel.ReadGesture();

					if (_isBbuttonActive)
					{

						switch (_userInputType)
						{
							case UserInputType.Press:
								if (_gestureSample.GestureType != GestureType.Hold)
									_isBbuttonActive = false;
								_buttonID = -1;
								break;

							case UserInputType.Release:
								_isBbuttonActive = false;
								_buttonID = -1;
								break;

							case UserInputType.PinchDrag:
								_startVectors[0] = _gestureSample.Position;

								switch (_gestureSample.GestureType)
								{
									case GestureType.FreeDrag:
										if (_activeGestureType == GestureType.FreeDrag)
										{
											_canvasLocal = WindowPoint_To_Canvas(_startVectors[0]);

											Set_ImageOffset();
									
											_pointLocal = _canvasLocal;

											_drawImageTarget = true;
										}
										else
										{
											_isBbuttonActive = false;
											_buttonID = -1;
										}

										break;

									case GestureType.Pinch:
										if (_activeGestureType == GestureType.Pinch)
										{
											_startVectors[1] = _gestureSample.Position2;
											_length2 = (_startVectors[0] - _startVectors[1]).Length();

											if (_length2 != 0) _dif = _length2 / _length;


											Find_Point_Location(Gesture_Centre(_startVectors));

											//_canvasLocal = WindowPoint_To_Canvas(Gesture_Centre(_startVectors));

											_imageScale = _imageScaleTemp * _dif;

											if (_imageScale < 0.25f) _imageScale = 0.25f;
											else if (_imageScale > 12.0f) _imageScale = 12.0f;

											_imageOffset = _canvasLocal - _pointLocal * _imageScale;

											_canvasLocal = _pointLocal = Vector2.Zero;

											Set_ImageOffset();

											_pointLocal = _canvasLocal;

											_drawImageTarget = true;
										}
										else
										{
											_isBbuttonActive = false;
											_buttonID = -1;
										}

										break;
									
									default:
										_buttonID = -1;
										_isBbuttonActive = false;
										break;
								}
								break;

							case UserInputType.Drag:
								if (_gestureSample.GestureType == GestureType.FreeDrag)
								{
									_canvasLocal = WindowPoint_To_Canvas(_gestureSample.Position);

									_dif = (_canvasLocal.X  - _pointLocal.X) / (float)_spreadRenderTarget.Width;

									if (Colour_Class.Adjust_Spread(_dif))
									{
										_pointLocal = _canvasLocal;

										_updateImageTex = true;
										_drawImageTarget = true;
									}
								}
								else
								{
									if (_buttonID == 20) _drawImageTarget = true;
									
									_isBbuttonActive = false;
									_buttonID = -1;
								}

								

								break;
							default:
								break;
						}

					}
				}

				if (_updateImageTex)
				{
					Colour_Class.Draw_SpreadRenderTarget();

					if (_currentLayoutID == 1)
						Menu_Class.UpDate_Draw_Style_Image();
					else
						Create_Image.Update_Image_Full();

					_updateImageTex = false;
				}
			}
			else
			{
				if (!TouchPanel.IsGestureAvailable)
				{
					_buttonID = -1;
					_isBbuttonActive = false;
					_activeGestureType = GestureType.None;

					return _buttonID;
				}

				while (TouchPanel.IsGestureAvailable)
				{
					_gestureSample = TouchPanel.ReadGesture();
				
					_startVectors[0] = _gestureSample.Position;
					_startVectors[1] = _gestureSample.Position2;

					if (_buttonID == -1)
					{
						_buttonID = Button_Contains(_startVectors[0]);

						if (_buttonID > -1)
						{
							Set_CurrentButton(_buttonID);

							_userInputType = _currentButton._inputType;
						
							switch (_gestureSample.GestureType)
							{
								case GestureType.Tap:
									if (_userInputType == UserInputType.Release)
									{
										_isBbuttonActive = true;

										_activeGestureType = GestureType.Tap;
									}
									else
									{
										_buttonID = -1;

										_isBbuttonActive = false;
									}
									break;

								case GestureType.FreeDrag:
									if (_userInputType == UserInputType.PinchDrag)
									{
										_isBbuttonActive = true;

										_activeGestureType = GestureType.FreeDrag;


										_pointLocal = WindowPoint_To_Canvas(_startVectors[0]);

										//Find_Point_Location(_startVectors[0]);

										//User_Input.Update_Input();

										return _buttonID;
									}
									else if (_userInputType == UserInputType.Drag)
									{
										_isBbuttonActive = true;

										_activeGestureType = GestureType.HorizontalDrag;

										_pointLocal = WindowPoint_To_Canvas(_startVectors[0]);

										//User_Input.Update_Input();

										return _buttonID;
									}
									else
									{
										_buttonID = -1;
										_isBbuttonActive = false;
									}
									break;

								case GestureType.Pinch:
									if (_userInputType == UserInputType.PinchDrag)
									{
										_isBbuttonActive = true;

										_activeGestureType = GestureType.Pinch;

										_pointLocal = WindowPoint_To_Canvas(Gesture_Centre(_startVectors));
										//Find_Point_Location(Gesture_Centre(_startVectors));

										_imageScaleTemp = _imageScale;

										_length = (_startVectors[0] - _startVectors[1]).Length();
										if (_length == 0) _length = 0.1f;

										//User_Input.Update_Input();

										return _buttonID;
									}
									else
									{
										_buttonID = -1;
										_isBbuttonActive = false;
									}
									break;
								case GestureType.DragComplete:
									if (_userInputType == UserInputType.Drag)
									{
										_isBbuttonActive = true;

										_activeGestureType = GestureType.HorizontalDrag;

										Find_Point_Location(_startVectors[0]);

										//User_Input.Update_Input();

										return _buttonID;
									}
									break;

								default:
									_buttonID = -1;
									_isBbuttonActive = false;
									break;
							}
						}
					}
					else
					{
						_buttonID = -1;
						_isBbuttonActive = false;
					}
				}
			}

			return _buttonID;
		}


		internal static void Set_ImageOffset()
		{
			if (_imageScale <= 0) return;

			_imageOffset += (_canvasLocal - _pointLocal);// * (1.0f / _imageScale);
			
			if (_imageOffset.X > 0)
			{
				while (_imageOffset.X > 0)
				{
					_imageOffset.X -= _tileTexture.Width * _imageScale;
				}
			}
			else if (_imageOffset.X < -_tileTexture.Width * _imageScale)
			{
				while (_imageOffset.X < -_tileTexture.Width * _imageScale)
				{
					_imageOffset.X += _tileTexture.Width * _imageScale;
				}
			}

			if (_imageOffset.Y > 0)
			{
				while (_imageOffset.Y > 0)
				{
					_imageOffset.Y -= _tileTexture.Height * _imageScale;
				}
			}
			else if (_imageOffset.Y < -_tileTexture.Height * _imageScale)
			{
				while (_imageOffset.Y < -_tileTexture.Height * _imageScale)
				{
					_imageOffset.Y += _tileTexture.Height * _imageScale;
				}
			}
			
		}


		internal static Vector2 Gesture_Centre(Vector2[] locii)
		{
			return locii[0] + (locii[1] - locii[0]) / 2.0f;
		}


		internal static void Find_Point_Location(Vector2 mouseLocal)
		{
			_canvasLocal = WindowPoint_To_Canvas(mouseLocal);

			foreach (var rect in Create_Image._outRects)
			{
				if (rect.Contains(_canvasLocal))
				{
					_pointLocal = _canvasLocal - rect.Location.ToVector2();
					_pointLocal *= 1.0f / _imageScale;

					break;
				}
			}

		}

		internal static Vector2 WindowPoint_To_Canvas(Vector2 winLocation)
		{
			Rectangle buttonrect = My_Layouts.Button_Canvas(_buttonID);

			winLocation -= buttonrect.Location.ToVector2();
			winLocation.X *= _imageRenderTarget.Width / (float)buttonrect.Width;
			winLocation.Y *= _imageRenderTarget.Height / (float)buttonrect.Width;
			
			return winLocation;
		}


	}
}