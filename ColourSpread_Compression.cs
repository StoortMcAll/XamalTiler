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
using zzz;
using static XamalTiler.Game1;
using static XamalTiler.My_Layouts;


namespace XamalTiler
{
	internal static partial class Colour_Class
	{
		const float RangeMin = 0.9f, RangeMax = 0.99999f;

		static int[] _vertices;

		internal static float _sinOffset = 0.99999f, _0to1 = 1.0f;

		internal static void Draw_SpreadRenderTarget()
		{
			Set_Vertices();

			_graphicsDevice.SetRenderTarget(_spreadRenderTarget);
			_graphicsDevice.Clear(Color.Black);

			float hitscale = (_currentColorSet._colorSpreadCount - 1) / (float)_spreadRenderTarget.Width;

			Rectangle rect = new Rectangle(0, 0, 1, _spreadRenderTarget.Width);

			_spriteBatch.Begin();

			for (int x = 0; x < _spreadRenderTarget.Width - 1; x++)
			{
				rect.X = x;

				_spriteBatch.Draw(_1by1, rect, _currentColorSet._colorSpread[_vertices[(int)(hitscale * x)]]);
			}

			_spriteBatch.End();
		}


		private static void Set_Vertices()
		{

			float[] scale = new float[_currentColorSet._colorSpreadCount];

			_vertices = new int[_currentColorSet._colorSpreadCount];

			float val, xval = _sinOffset;

			scale[0] = 0;

			for (int i = 1; i < _currentColorSet._colorSpreadCount; i++)
			{
				val = _currentColorSet._colorSpreadCount - (float)(xval * _currentColorSet._colorSpreadCount);
				xval *= _sinOffset;
				scale[i] = val;
			}

			_vertices = ResizeVerticesToWorldCoords(scale, _currentColorSet._colorSpreadCount);
		}

		private static int[] ResizeVerticesToWorldCoords(float[] vectors, int range)
		{
			float minvals = 0;

			float maxvals = 0;

			foreach (var vert in vectors)
			{
				if (vert < minvals) minvals = vert;
				else if (vert > maxvals) maxvals = vert;
			}

			float length = (maxvals - minvals);

			for (int i = 0; i < vectors.Length; i++)
			{
				vectors[i] -= minvals;

				vectors[i] *= (range - 1) / length;

				_vertices[i] = (int)vectors[i];
			}

			return _vertices;
		}


		/// <summary>
		/// ColourRange Compression
		/// </summary>
		/// <param name="value">0 <= value <= 1.0</param>
		internal static bool Adjust_Spread(float value)
		{
			_0to1 += value;

			if (_0to1 < 0.0f) _0to1 = 0;
			else if (_0to1 > 1.0f) _0to1 = 1.0f;

			float tempsin  = MathHelper.Lerp(RangeMin, RangeMax, _0to1);

			if (tempsin == _sinOffset) return false;

			_sinOffset = tempsin;

			return true;
		}
	}
}