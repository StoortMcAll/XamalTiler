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

		internal static bool _isBbuttonActive;
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
				if (_userInputType == UserInputType.Release)
				{
					_isBbuttonActive = false;
					_buttonID = -1;
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
								switch (_gestureSample.GestureType)
								{
									case GestureType.FreeDrag:
										if (_activeGestureType == GestureType.FreeDrag)
										{
											_startVectors[0] = _gestureSample.Position;

											_canvasLocal = WindowPoint_To_Canvas(_startVectors[0]);

											Set_ImageOffset();

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
											_startVectors[0] = _gestureSample.Position;
											_startVectors[1] = _gestureSample.Position2;
											_length2 = (_startVectors[0] - _startVectors[1]).Length();

											_dif = _length2 / _length;

											_imageScale = _imageScaleTemp * _dif;

											if (_imageScale < 0.25f) _imageScale = 0.25f;
											else if (_imageScale > 8.0f) _imageScale = 8.0f;

											_canvasLocal = WindowPoint_To_Canvas(Gesture_Centre(_startVectors));

											Set_ImageOffset();

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
									_startVectors[0] = _gestureSample.Position;

									_dif = (_startVectors[0].X  - _pointLocal.X) / (float)_spreadRenderTarget.Width;

									if (Colour_Class.Adjust_Spread(_dif))
										Colour_Class.Draw_SpreadRenderTarget();
									
									_pointLocal = _startVectors[0];
								}
								else
								{
									_isBbuttonActive = false;
									_buttonID = -1;
								}
								break;
							default:
								break;
						}

					}
				}
			}
			else
			{
				if (!TouchPanel.IsGestureAvailable) _activeGestureType = GestureType.None;

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

										Find_Point_Location(_startVectors[0]);

										User_Input.Update_Input();

										return _buttonID;
									}
									else if (_userInputType == UserInputType.Drag)
									{
										_isBbuttonActive = true;

										_activeGestureType = GestureType.HorizontalDrag;

										_pointLocal = WindowPoint_To_Canvas(_startVectors[0]);

										User_Input.Update_Input();

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

										Find_Point_Location(Gesture_Centre(_startVectors));

										_imageScaleTemp = _imageScale;

										_length = (_startVectors[0] - _startVectors[1]).Length();

										User_Input.Update_Input();

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

										User_Input.Update_Input();

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
				}
			}

			return _buttonID;
		}


		internal static void Set_ImageOffset()
		{
			_imageOffset = _canvasLocal - _pointLocal * _imageScale;

			while (_imageOffset.X > 0) _imageOffset.X -= _tileTexture.Width * _imageScale;
			while (_imageOffset.Y > 0) _imageOffset.Y -= _tileTexture.Height * _imageScale;

			while (_imageOffset.X < -_tileTexture.Width * _imageScale)
				_imageOffset.X += _tileTexture.Width * _imageScale;
			while (_imageOffset.Y < -_tileTexture.Height * _imageScale)
				_imageOffset.Y += _tileTexture.Height * _imageScale;

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