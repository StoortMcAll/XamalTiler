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
		private readonly bool _setImmersive = false;
		private Game1 _game;
		private View _view;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			
			//if ((ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
			//|| (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted))
			//{
			//	ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage }, 0);
			//}

			GetDisplaySize(out int width, out int height);
		
			_game = new Game1(width, height);

			_view = (View)_game.Services.GetService(typeof(View));
			SetContentView(_view);
			_game.Run();
		}

		private void GetDisplaySize(out int width, out int height)
		{
			Android.Graphics.Rect
				realrect = new Android.Graphics.Rect();

			WindowManager.DefaultDisplay.GetRectSize(realrect);


			Android.Graphics.Point
				realsize = new Android.Graphics.Point();

			WindowManager.DefaultDisplay.GetRealSize(realsize);

			if (_setImmersive) { width = realsize.X; height = realsize.Y; }
			else
				width = realrect.Width(); height = realrect.Height();
		}

		public override void OnConfigurationChanged(Configuration newConfig)
		{
			GetDisplaySize(out int width, out int height);
			_game.ConfigurationChanged(width, height);

			base.OnConfigurationChanged(newConfig);
		}

		public override void OnWindowFocusChanged(bool hasFocus)
		{
			base.OnWindowFocusChanged(hasFocus);

			if (_setImmersive)
			{
				if (hasFocus)
					SetImmersive();
			}
		}

		private void SetImmersive()
		{
			if (_view != null && Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
			{
				_view.SystemUiVisibility = (StatusBarVisibility)
				  (SystemUiFlags.LayoutStable |
				   SystemUiFlags.LayoutHideNavigation |
				   SystemUiFlags.LayoutFullscreen |
				   SystemUiFlags.HideNavigation |
				   SystemUiFlags.Fullscreen |
				   SystemUiFlags.ImmersiveSticky);
			}
		}
	}
}

