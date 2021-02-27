using Android;
using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;

namespace XamalTiler
{
	[Activity(Label = "XamalTiler"
		, MainLauncher = true
		, Icon = "@drawable/icon"
		, Theme = "@style/Theme.Splash"
		, AlwaysRetainTaskState = true
		, LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
		, ScreenOrientation = ScreenOrientation.FullUser
		, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout)]
	
	public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
	{
		private Game1 _game;
		private View _view;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			
			if ((ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
			|| (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted))
			{
				ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage }, 0);
			}

			GetDisplaySize(out int width, out int height);
			GetFullSize(out int fwidth, out int fheight);
		
			_game = new Game1(width, height, fwidth, fheight);

			_view = (View)_game.Services.GetService(typeof(View));
			SetContentView(_view);
			_game.Run();
		}

		private void GetDisplaySize(out int width, out int height)
		{
			Android.Graphics.Rect
				realrect = new Android.Graphics.Rect();

			WindowManager.DefaultDisplay.GetRectSize(realrect);

			width = realrect.Width(); height = realrect.Height();
		}

		private void GetFullSize(out int width, out int height)
		{
			Android.Graphics.Point
				realsize = new Android.Graphics.Point();

			WindowManager.DefaultDisplay.GetRealSize(realsize);

			width = realsize.X; height = realsize.Y;
		}

		public override void OnConfigurationChanged(Configuration newConfig)
		{
			GetDisplaySize(out int width, out int height);
			_game.ConfigurationChanged(width, height);

			base.OnConfigurationChanged(newConfig);
		}
	}
}

