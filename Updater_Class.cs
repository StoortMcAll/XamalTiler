using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static XamalTiler.Colour_Class;
using static XamalTiler.Create_Image;


namespace XamalTiler
{
	internal static class Updater_Class
	{
		#region Variable Declaration

		public enum UpdaterState { Waiting, Start, Updating, Finalise }


		public struct SpreadVals
		{
			public float _sinValue;


			public SpreadVals(float sinValue)
			{
				_sinValue = sinValue;

				_spreadTargetState._hasQueue = true;
			}

			public SpreadVals(SpreadVals spreadQueue) : this()
			{
				_sinValue = spreadQueue._sinValue;
			}

			public float SinValue { set { _sinValue = value; } get { return _sinValue; } }
		}

		public struct Updater
		{
			public bool _hasQueue;

			public bool HasQueue { set { _hasQueue = value; } get { return _hasQueue; } }

			public UpdaterState _updaterState;

			public UpdaterState State { set { _updaterState = value; } get { return _updaterState; } }

			public Updater(UpdaterState updaterState)
			{
				_updaterState = updaterState;

				_hasQueue = false;
			}
		}


		
		internal static Updater _colourSchemeState = new Updater(UpdaterState.Waiting);
		internal static Updater _imageTargetState = new Updater(UpdaterState.Waiting);

		internal static Updater _spreadTargetState = new Updater(UpdaterState.Waiting);
		internal static SpreadVals _spreadCurrent, _spreadQueue = new SpreadVals(1.0f);
		internal static Color[] _adjustedColourAsnc;
		internal static int _adjustedColourCount;
		internal static Task<Color[]> _spreadTask = SpreadTarget_TaskWrapper();
		internal static Texture2D _textureAsync;


		#endregion


		public static void Do_Updater()
		{
			Do_SpreadTarget();
		}


		private static void Do_SpreadTarget()
		{
			if (_spreadTargetState.State == UpdaterState.Waiting)
			{
				if (_spreadTargetState._hasQueue)
				{
					_spreadTargetState.State = UpdaterState.Start;
					Do_SpreadTarget();
				}

				return;
			}

			if (_spreadTargetState.State == UpdaterState.Start)
			{
				_spreadCurrent = new SpreadVals(_spreadQueue);

				_spreadTargetState._hasQueue = false;

				if (_spreadTask.Status == TaskStatus.Created)
				{
					_adjustedColourCount = _currentSpreadCount;
					_adjustSpreadColors = new Color[_adjustedColourCount];

					for (int i = 0; i < _adjustedColourCount; i++)
						_adjustSpreadColors[i] = _currentColorSet._colorSpread[i];

					_textureAsync = new Texture2D(_graphicsDevice, _adjustedColourCount, 1);

					_spreadTask.Start();
				}
			}
			else if (_spreadTargetState.State == UpdaterState.Finalise)
			{
				if (_spreadTask.IsCompleted)
				{
					_adjustSpreadColors = new Color[_adjustedColourCount];

					for (int i = 0; i < _adjustedColourCount; i++)
					{
						_adjustSpreadColors[i] = _spreadTask.Result[i];
					}
				}

			}
		}



		internal static Task<Color[]> SpreadTarget_TaskWrapper()
		{
			_adjustedColourAsnc = new Color[_adjustedColourCount];

			_spreadTargetState.State = UpdaterState.Updating;

			return Task.Run(() => SpreadTarget_Task(_currentColorSet._colorSpread));
		}

		internal static Color[] SpreadTarget_Task(List<Color> colors)
		{
			var adjustVertices = new int[colors.Count];

			Color[] newcols = new Color[colors.Count];

			for (int i = 0; i < adjustVertices.Length; i++)
			{
				newcols[i] = colors[i];
			}

			_textureAsync.SetData(newcols);

			_spreadTargetState.State = UpdaterState.Finalise;

			return newcols;
		}


	}
}