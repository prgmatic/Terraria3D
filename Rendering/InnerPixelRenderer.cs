using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Terraria3D
{
    public class InnerPixelRenderer
    {
        private GraphicsDevice _graphicsDevice => Main.graphics.GraphicsDevice;
        private Effect _effect;

        public InnerPixelRenderer(Effect effect) { _effect = effect; }

        public void Draw(RenderTarget2D target, Texture2D texture)
        {
            _graphicsDevice.SetRenderTarget(target);
            _graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null);
            _effect.Parameters["PixelOffset"].SetValue(new Vector2(1f / texture.Width, 1f / texture.Height));

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Main.spriteBatch.Draw(texture, Vector2.Zero, Color.White);
            }
            Main.spriteBatch.End();
            _graphicsDevice.SetRenderTarget(null);
        }
    }
}