using static XamalTiler.Game1;
using static XamalTiler.Create_Image;
using static XamalTiler.ScreenShot;
using static XamalTiler.My_Layouts;
using static XamalTiler.Colour_Class;
using System.ComponentModel.Design;

namespace XamalTiler
{
	internal static class Menu_Class
	{

		internal static void Update_Main_Menu() 
		{
			switch (_buttonHit)
			{
				case 0:
					_progState = Iterate;

					_quiltType = QuiltType.Square;

					_iterateState = IterateState.StartIterate;

					My_Layouts.Activate_Layout(3);

					Initialise.RandomAll();

					break;
				case 5:
					_progState = Iterate;

					_quiltType = QuiltType.Icon;

					_iterateState = IterateState.StartIterate;

					My_Layouts.Activate_Layout(3);

					Initialise.RandomAll();

					break;
				case 6:
					_progState = Iterate;

					_quiltType = QuiltType.Hexagon;

					_iterateState = IterateState.StartIterate;

					My_Layouts.Activate_Layout(3);

					Initialise.RandomAll();

					break;
				case 1:
					_progState = Iterate;

					_iterateState = IterateState.StartIterate;

					My_Layouts.Activate_Layout(3);

					break;
				case 3:
					Create_Image.Find_Min_Hits();

					My_Layouts.Activate_Layout(1);

					if (Create_Image._minHits > 0)
						My_Layouts.Show_Button(4);
					else
						My_Layouts.Grey_Button(4);
					break;
				case 7:
					if (--_colorSetID == -1) _colorSetID = _colorSets.Count - 1;

					Set_CurrentColourSet(_colorSetID);

					Draw_SpreadRenderTarget();

					Create_Image.Update_Image_Full();

					_usingRandomSpread = false;
					_drawImageTarget = true;

					break;
				case 8:
					if (++_colorSetID == _colorSets.Count) _colorSetID = 0;

					Set_CurrentColourSet(_colorSetID);

					Draw_SpreadRenderTarget();

					Create_Image.Update_Image_Full();

					_usingRandomSpread = false;
					_drawImageTarget = true;

					break;
				case 9:

					break;
				case 10:
					DistributionClass._power -=1.0f;
					if (DistributionClass._power < 0) DistributionClass._power = 0;
					break;
				case 11:
					DistributionClass._power += 1.0f;
					break;
				default:
					break;
				
			}

			if (Create_Image._maxHits > 0)
			{
				//DistributionClass.Update();
			}
		}



		internal static void Update_Iterate_Menu()
		{
			if (_buttonHit == -1) return;

			switch (_buttonHit)
			{
				case 0:
					_progState = ProgramState.Menu;

					_iterateState = IterateState.StoppingIterate;

				//	_task.Wait();

					Activate_Layout(0);

					if (_fatalValues)
					{
						Hide_Button(1);
						Hide_Button(2);
						Show_Button(3);
						Show_Button(7);
						Show_Button(8);
						//Show_Button(9);
						//Show_Button(10);
						//Show_Button(11);
						//Show_Button(12);
						_fatalValues = false;
					}
					else
					{
						Show_Button(1);
						Show_Button(2);
						Show_Button(3);
						Show_Button(7);
						Show_Button(8);
						Show_Button(9);
						//Show_Button(10);
						//Show_Button(11);
						//Show_Button(12);
					}

					if (Create_Image._maxHits > 0)
					{
						//DistributionClass.Update();
					}

					break;
				case 4:
					if (--_colorSetID == -1) _colorSetID = _colorSets.Count - 1;

					_currentColorSetID = _colorSetID;

					UpDate_Draw_Style_Image();

					_usingRandomSpread = false;
					_drawImageTarget = true;

					break;
				case 5:
					if (++_colorSetID == _colorSets.Count) _colorSetID = 0;

					_currentColorSetID = _colorSetID;

					UpDate_Draw_Style_Image();

					_usingRandomSpread = false;
					_drawImageTarget = true;

					break;
				default:
					break;
			}

		}


	
		internal static void Update_Analyse_Menu()
		{
			int index;

			if (_buttonHit == -1) return;

			switch (_buttonHit)
			{
				case 0:
					_imageDrawStyle = ImageDrawStyle.Basic;
					My_Layouts.Change_Button_Text(3, "Sharp Image");

					_colourType = ColourType.Linear;
					My_Layouts.Change_Button_Text(2, "Linear");

					UpDate_Draw_Style_Image();

					My_Layouts.Activate_Layout(0);
					break;
				case 2:
					switch (_colourType)
					{
						case ColourType.Linear:
							_colourType = ColourType.Stretch;
							My_Layouts.Change_Button_Text(2, "Stretch"); 
							break;
						case ColourType.Stretch:
							_colourType = ColourType.SquareRoot;
							My_Layouts.Change_Button_Text(2, "SquareRoot");
							break;
						case ColourType.SquareRoot:
							_colourType = ColourType.Linear;
							My_Layouts.Change_Button_Text(2, "Linear");
							break;
						default:
							break;
					}

					UpDate_Draw_Style_Image();
					break;
				case 4:
					Create_Image.Shift_Minimum_Hit_To_Zero();

					My_Layouts.Grey_Button(4);

					UpDate_Draw_Style_Image();
					break;
				case 5:

					Create_Image.Draw_FullScreen();

					My_Layouts.Activate_Layout(2);
					break;
				case 7:
					if (--_colorSetID == -1) _colorSetID = _colorSets.Count - 1;

					_currentColorSetID = _colorSetID;

					UpDate_Draw_Style_Image();

					_usingRandomSpread = false;
					break;
				case 8:
					if (++_colorSetID == _colorSets.Count) _colorSetID = 0;

					_currentColorSetID = _colorSetID;

					UpDate_Draw_Style_Image();

					_usingRandomSpread = false;
					break;
				case 9:
					NewRandom_ColourSeries();

					UpDate_Draw_Style_Image();
					break;
				case 10:
					switch (_colourType)
					{
						case ColourType.Linear:
							_colourType = ColourType.SquareRoot;
							My_Layouts.Change_Button_Text(2, "SquareRoot");
							break;
						case ColourType.Stretch:
							_colourType = ColourType.Linear;
							My_Layouts.Change_Button_Text(2, "Linear");
							break;
						case ColourType.SquareRoot:
							_colourType = ColourType.Stretch;
							My_Layouts.Change_Button_Text(2, "Stretch");
							break;
						default:
							break;
					}

					UpDate_Draw_Style_Image();
					break;
				case 11:
					switch (_colourType)
					{
						case ColourType.Linear:
							_colourType = ColourType.Stretch;
							My_Layouts.Change_Button_Text(2, "Stretch");
							break;
						case ColourType.Stretch:
							_colourType = ColourType.SquareRoot;
							My_Layouts.Change_Button_Text(2, "SquareRoot");
							break;
						case ColourType.SquareRoot:
							_colourType = ColourType.Linear;
							My_Layouts.Change_Button_Text(2, "Linear");
							break;
						default:
							break;
					}

					UpDate_Draw_Style_Image();
					break;
				case 12:
					NewRandom_ColourSeries2();

					UpDate_Draw_Style_Image();
					break;
				case 13:
					switch (_imageDrawStyle)
					{
						case ImageDrawStyle.Basic:
							_imageDrawStyle = ImageDrawStyle.Average;
							My_Layouts.Change_Button_Text(3, "Average Image");
							break;
						case ImageDrawStyle.Smooth:
							_imageDrawStyle = ImageDrawStyle.Basic;
							My_Layouts.Change_Button_Text(3, "Sharp Image");
							break;
						case ImageDrawStyle.Smooth2:
							_imageDrawStyle = ImageDrawStyle.Smooth;
							My_Layouts.Change_Button_Text(3, "Smooth Image");
							break;
						case ImageDrawStyle.Average:
							_imageDrawStyle = ImageDrawStyle.Smooth2;
							My_Layouts.Change_Button_Text(3, "Smooth2 Image");
							break;
						default:
							break;
					}

					UpDate_Draw_Style_Image();
					break;
				case 14:
					switch (_imageDrawStyle)
					{
						case ImageDrawStyle.Basic:
							_imageDrawStyle = ImageDrawStyle.Smooth;
							My_Layouts.Change_Button_Text(3, "Smooth Image");
							break;
						case ImageDrawStyle.Smooth:
							_imageDrawStyle = ImageDrawStyle.Smooth2;
							My_Layouts.Change_Button_Text(3, "Smooth2 Image");
							break;
						case ImageDrawStyle.Smooth2:
							_imageDrawStyle = ImageDrawStyle.Average;
							My_Layouts.Change_Button_Text(3, "Average Image");
							break;
						case ImageDrawStyle.Average:
							_imageDrawStyle = ImageDrawStyle.Basic;
							My_Layouts.Change_Button_Text(3, "Sharp Image");
							break;
						default:
							break;
					}

					UpDate_Draw_Style_Image();
					break;
				case 16:
					index = _colorSetNewID;

					if (--_colorSetNewID < 0) _colorSetNewID = _colorSetsNew.Count - 1;

					if (!_usingRandomSpread || index != _colorSetNewID)
					{
						Set_RandomCol_Current(_colorSetNewID);
					
						UpDate_Draw_Style_Image();
						//Create_Image.Update_Image_Full();

						//_drawImageTarget = true;
					}
					break;
				case 17:
					index = _colorSetNewID;

					if (++_colorSetNewID > _colorSetsNew.Count - 1) _colorSetNewID = 0;

					if (!_usingRandomSpread || index != _colorSetNewID)
					{
						Set_RandomCol_Current(_colorSetNewID);

						UpDate_Draw_Style_Image();

						//_usingRandomSpread = true;
						//_drawImageTarget = true;
					}
					break;
				case 18:
					if (Del_New_RandomColorSet()) 
						UpDate_Draw_Style_Image();
					break;
				default:
					break;
			}
			
		}



		internal static void Update_FullScreen_Menu()
		{
			if (_saveImageState == SaveImageState.Saving)
			{
				Save_Screenshot();

				return;
			}
			else if (_saveImageState == SaveImageState.SaveWallpaper)
			{
				Save_Wallpaper();

				return;
			}

			if (_buttonHit == -1) return;

			switch (_buttonHit)
			{
				case 1:
					if (_saveImageState == SaveImageState.Ready)
					{
						_drawImageTarget = true;
						My_Layouts.Activate_Layout(1);
					}
					break;
				case 2:
					ScreenShot.Take_Screenshot();
					break;
				case 3:
					ScreenShot.Set_Wallpaper();
					break;
				default:
					break;
			}
		}


		private static void UpDate_Draw_Style_Image()
		{
			switch (_imageDrawStyle)
			{
				case ImageDrawStyle.Basic:
					Update_Image_Full();
					break;
				case ImageDrawStyle.Smooth:
					Update_Image_Full_Smooth();
					break;
				case ImageDrawStyle.Smooth2:
					Update_Image_Full_Smooth2();
					break;
				case ImageDrawStyle.Average:
					Update_Image_Full_Average();
					break;
				default:
					break;
			}

			_drawImageTarget = true;
		}
	

	}
}