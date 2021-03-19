using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static XamalTiler.Game1;
 

namespace XamalTiler
{
	internal static class ScreenShot
	{

		internal enum SaveImageState { Ready, Requested, Saving, SaveWallpaper }

		internal static SaveImageState _saveImageState = SaveImageState.Ready;


		internal static void Take_Screenshot()
		{
            if (_saveImageState != SaveImageState.Ready) return;

            _saveImageState = SaveImageState.Requested;

            Color[] colors = new Color[_tileTexture.Width * _tileTexture.Height];

            _tileTexture.GetData(colors);

            _tileSaveTexture.SetData(colors);

            Create_Image.Draw_Screenshot();

            _saveImageState = SaveImageState.Saving;
        }


        internal static void Set_Wallpaper()
        {
            if (_saveImageState != SaveImageState.Ready) return;

            _saveImageState = SaveImageState.Requested;

            Color[] colors = new Color[_tileTexture.Width * _tileTexture.Height];

            _tileTexture.GetData(colors);

            _tileSaveTexture.SetData(colors);

            Create_Image.Draw_Screenshot();

            _saveImageState = SaveImageState.SaveWallpaper;
        }
            

        internal static void Save_Screenshot()
        {
            Save_Image_Async();

            _saveImageState = SaveImageState.Ready;
        }


        internal static void Save_Wallpaper()
        {
            Create_Image.Draw_Screenshot();

            Android.App.WallpaperManager manager = Android.App.WallpaperManager.GetInstance(
                Android.App.Application.Context);

            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null))
            {
                string isofile = "Temp.png";

                using (var stream = isoStore.CreateFile(isofile))
				{
                    _screenshotTarget.SaveAsPng(stream, _screenshotTarget.Width, _screenshotTarget.Height);
                }

                if (isoStore.FileExists(isofile))
				{
                    using (var stream = isoStore.OpenFile(isofile, FileMode.Open))
                    {
                        Android.Graphics.Bitmap bm = Android.Graphics.BitmapFactory.DecodeStream(stream);
                        manager.SetBitmap(bm);
                    }

                    isoStore.DeleteFile(isofile);
                }
            }


   //             var finalPath2 = Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures);

   //         var curdirect = Directory.GetCurrentDirectory();
   //         var envdirect = Environment.CurrentDirectory;

   //         IEnumerable<string> vs = Directory.EnumerateDirectories(curdirect);

   //         IEnumerable<string> cd = Directory.EnumerateDirectories(envdirect);

   //         if (Directory.Exists(finalPath2))
   //             finalPath2 += "/Temp.png";
   //         else
			//{

			//}
        

   //         using (var fileStream = new FileStream(finalPath2, FileMode.Create))
   //         {
   //             _screenshotTarget.SaveAsPng(fileStream, _screenshotTarget.Width, _screenshotTarget.Height);
   //         }

   //         if (File.Exists(finalPath2))
   //         {
   //             Android.Graphics.Bitmap bm = Android.Graphics.BitmapFactory.DecodeFile(finalPath2);
   //             manager.SetBitmap(bm);

   //             File.Delete(finalPath2);
   //         }

            _saveImageState = SaveImageState.Ready;
        }



        internal static void Save_Image_Async()
        {
			#region Test_Directorys

			string loc = "/storage/emulated/0/";
            if (!Test_Directoy_Exists(loc))
            {
                _saveImageState = SaveImageState.Ready;
                return;
            }

            loc += "Pictures/";
            if (!Test_Directoy_Exists(loc))
            {
                _saveImageState = SaveImageState.Ready;
                return;
            }

            loc += "ChaoTiles/";
            if (!Test_Directoy_Exists(loc))
            {
                Directory.CreateDirectory(loc);

                if (!Test_Directoy_Exists(loc))
                {
                    _saveImageState = SaveImageState.Ready;
                    return;
                }
            }

            string screensloc = loc + "Backdrops/";
            string tilesloc = loc + "Tiles/";

            bool dobackdrop = false, dotile = false;

            if (!Test_Directoy_Exists(screensloc))
            {
                Directory.CreateDirectory(screensloc);

                if (Test_Directoy_Exists(screensloc)) dobackdrop = true;
            }
            else dobackdrop = true;
            if (!Test_Directoy_Exists(tilesloc))
            {
                Directory.CreateDirectory(tilesloc);

                if (Test_Directoy_Exists(tilesloc)) dotile = true;
            }
            else dotile = true;

			#endregion

			DateTime dt = System.DateTime.Now;
            string filename = dt.ToLocalTime().ToString();

            if (dobackdrop)
            {
                filename = filename.Replace('/', '-');
                filename = filename.Replace(' ', '-');
                filename = filename.Replace(':', '-');
                filename = screensloc + "Screens-" + filename + ".png";

                using (var fileStream = new FileStream(filename, FileMode.Create))
                    _screenshotTarget.SaveAsPng(fileStream, _screenshotTarget.Width, _screenshotTarget.Height);

             //   Android.Graphics.Bitmap bm = Android.Graphics.BitmapFactory.DecodeFile(filename);

               // Android.App.WallpaperManager manager = Android.App.WallpaperManager.GetInstance(
                   // Android.App.Application.Context);

              //  manager.SetBitmap(bm);
            }


            //}


            filename = dt.ToLocalTime().ToString();

            if (dotile)
            {
                filename = filename.Replace('/', '-');
                filename = filename.Replace(' ', '-');
                filename = filename.Replace(':', '-');
                filename = tilesloc + "Tile-" + filename + ".png";

                using (var fileStream = new FileStream(filename, FileMode.Create))
                    _tileSaveTexture.SaveAsPng(fileStream, _tileSaveTexture.Width, _tileSaveTexture.Height);
            }


            _saveImageState = SaveImageState.Ready;
        }

        private static bool Test_Directoy_Exists(string loc)
        {
            if (Directory.Exists(loc)) return true;

            return false;
        }

    }
}
