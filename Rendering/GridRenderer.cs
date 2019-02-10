using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Terraria3D
{
	public class GridRenderer
	{
		private GraphicsDevice _graphicsDevice => Terraria.Main.graphics.GraphicsDevice;

		private Effect _effect;
		private Texture _noiseTexture;
		private VertexPositionNormalTexture[] _grid;
		private VertexBuffer _gridBuffer;
		private float _width;
		private float _height;

		public GridRenderer(Effect effect, Texture noiseTexture, int width, int height, int targetWidth, int targetHeight)
		{
			_effect = effect;
			_noiseTexture = noiseTexture;
			_width = width;
			_height = height;

			SetGridSize(width, height, targetWidth, targetHeight);
			_effect.Parameters["NoiseTexture"].SetValue(_noiseTexture);
		}

		public void Dispose()
		{
			_effect?.Dispose();
			_noiseTexture?.Dispose();
			_gridBuffer?.Dispose();
		}

		public void SetGridSize(int width, int height, int targetWidth, int targetHeight)
		{
			_grid = PixelGrid.Create(width, height, targetWidth, targetHeight);
			_gridBuffer?.Dispose();
			_gridBuffer = new VertexBuffer(_graphicsDevice, VertexPositionNormalTexture.VertexDeclaration, _grid.Length, BufferUsage.WriteOnly);
			_gridBuffer.SetData(_grid);
		}

		public void Draw(Texture2D texture, Camera camera, float depth, float noiseAmount = 1, bool ao = true, Matrix? modelMatrix = null)
		{
			if (modelMatrix == null)
				modelMatrix = Matrix.Identity;


			_effect.Parameters["View"].SetValue(camera.View);
			_effect.Parameters["Projection"].SetValue(camera.Projection);
			_effect.Parameters["_MainTex"].SetValue(texture);
			_effect.Parameters["PixelOffset"].SetValue(new Vector2(1f / texture.Width, 1f / texture.Height));
			_effect.Parameters["World"].SetValue(modelMatrix.Value);
			_effect.Parameters["Depth"].SetValue(depth);
			_effect.Parameters["NoiseAmount"].SetValue(noiseAmount);
			_effect.Parameters["CameraPosition"].SetValue(new Vector2(-Main.screenPosition.X, Main.screenPosition.Y));
			if (Renderers.SM3Enabled)
				_effect.Parameters["AO"].SetValue(ao);

			// Read from z buffer when determining the what pixel to render on top
			_graphicsDevice.DepthStencilState = DepthStencilState.Default;
			_graphicsDevice.BlendState = BlendState.AlphaBlend;

			foreach (var pass in _effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				_graphicsDevice.SetVertexBuffer(_gridBuffer);
				_graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _grid.Length / 3);
			}
		}
	}
}