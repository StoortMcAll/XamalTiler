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

using static XamalTiler.Create_Image;

namespace XamalTiler
{
	public static class DistributionClass
	{

		public static VertexDeclaration vertexDeclaration;

		public static VertexBuffer vertexBuffer;

		public static Matrix viewMatrix;
		public static Matrix projectionMatrix;
		public static BasicEffect basicEffect;


		public static VertexPositionColor[] _vpColor;


		public static Vector3[] _vectors, _worldSpace;

		public static Point _size;

		public static float _power = 0.0f;

		public static void Initialise()
		{
			_size = new Point(256, 100);

			basicEffect = new BasicEffect(_graphicsDevice);
			basicEffect.VertexColorEnabled = true;
			basicEffect.View = Matrix.CreateLookAt(new Vector3(0, 0, 4), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
			basicEffect.World = Matrix.Identity;
			//basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f,
			//	1.0f,
			//	// (float)_canvas.Width / (float)_canvas.Height,
			//	0.1f,
			//	1000);

			basicEffect.Projection = Matrix.CreateOrthographic(2, 2, 0.1f, 1000);

			_vpColor = new VertexPositionColor[_size.X * 2];

			vertexBuffer = new VertexBuffer(_graphicsDevice,
				typeof(VertexPositionColor),
				_size.X * 2, BufferUsage.WriteOnly);
		}


		public static void Update()
		{
			_size = new Point(_size.X,
				(int)(Create_Image._maxHits < 2048 ? Create_Image._maxHits : 2048));

			_vectors = new Vector3[_size.X];
			_worldSpace = new Vector3[_size.X];

			Set_PointList();

			Calc_Vectors();

			ToWorldSpace();

			Set_VertexPosColor();
		}

		public static void Set_PointList()
		{
			float result, count = 0.0f;

			if (_power == 0)
			{
				for (int i = 0; i < _size.X; i++)
				{
					_vectors[i] = new Vector3(i, i, 0);
				}
			}
			else
			{
				for (int i = 0; i < _size.X; i++)
				{
					result = (float)(Math.Pow(2.0, count));// + _midButValue2 / 10.0f, count));

					_vectors[i] = new Vector3(result, i, 0);// / _canvas.Width;
															  //count *= (1.039f + _midButValue / 1000.0f);

					count += _power / 1000.0f;
				}
			}
		}

		public static void Calc_Vectors()
		{
			float result, count = 0.0f;

			if (_power == 0)
			{
				for (int i = 0; i < _size.X; i++)
				{
					_vectors[i] = new Vector3(i, i, 0);
				}
			}
			else
			{
				for (int i = 0; i < _size.X; i++)
				{
					result = (float)(Math.Pow(2.0, count));// + _midButValue2 / 10.0f, count));

					_vectors[i] = new Vector3(result, i, 0);// / _canvas.Width;
															  //count *= (1.039f + _midButValue / 1000.0f);

					count += _power / 1000.0f;
				}
			}

		}

		public static void ToWorldSpace()
		{
			int count = _vectors.Length;

			_worldSpace = new Vector3[count];

			Vector4 minmax = FindMinMax(_vectors);

			Vector4 scale = new Vector4(
				-minmax.X, -minmax.Y,
				minmax.Z - minmax.X,
				minmax.W - minmax.Y);

			Vector3 local;

			for (int i = 0; i < count; i++)
			{
				local = new Vector3(
					(scale.X + _vectors[i].X) / scale.Z,
					(scale.Y + _vectors[i].Y) / scale.W,
					0);

				_worldSpace[i] = new Vector3(
					local.X * 2.0f - 1.0f,
					(local.Y) * 2.0f - 1.0f,
					0
					);
			}

		}


		public static Vector4 FindMinMax(Vector3[] vectors)
		{
			Vector3 local = vectors[0];
			Vector4 minmax = new Vector4(local.X, local.Y, local.X, local.Y);

			for (int i = 0; i < vectors.Length; i++)
			{
				local = vectors[i];

				if (local.X < minmax.X) minmax.X = local.X;
				else if (local.X > minmax.X) minmax.Z = local.X;

				if (local.Y < minmax.Y) minmax.Y = local.Y;
				else if (local.Y > minmax.Y) minmax.W = local.Y;
			}

			return minmax;
		}



		public static void Set_VertexPosColor()
		{
			int index = 0;
			for (int i = 0; i < _size.X; i++)
			{
				_vpColor[index++] = new VertexPositionColor(
				   _worldSpace[i] * 0.99f,
					Color.Red);
				_vpColor[index++] = new VertexPositionColor(
				   (_worldSpace[i] + new Vector3(-0.02f, 0.02f, 0.0f)) * 0.99f,
					Color.Red);
			}

			vertexBuffer.SetData(_vpColor, 0, _size.X * 2);
		}


		public static void Draw_Target()
		{
			if (Game1._progState != Game1.ProgramState.Menu ||
				Create_Image._maxHits == 0)
				return;

			_graphicsDevice.SetRenderTarget(Game1._spreadRenderTarget);

			_graphicsDevice.Clear(Color.DarkBlue);

			RasterizerState rasterizerState = new RasterizerState();
			rasterizerState.CullMode = CullMode.None;
			_graphicsDevice.RasterizerState = rasterizerState;

			_graphicsDevice.SetVertexBuffer(vertexBuffer);


			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();

				_graphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, _size.X * 2- 2);
			}

			_graphicsDevice.SetRenderTarget(null);
		}


	}
}