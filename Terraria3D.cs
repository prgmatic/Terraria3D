using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terraria3D
{
    class Terraria3D : Mod
    {
        public static Terraria3D Instance { get; private set; }

        private Scene3D _scene = new Scene3D();
        private Layer3D[] _layers = new Layer3D[]
        {
            new Layer3D()
            {
                RenderFunction = () =>
                {
                    Rendering.DrawBackgroundWater();
                    Rendering.DrawSolidTiles();
                    Rendering.DrawPlayers();
                }
            }
        };

        public override void PostSetupContent()
        {
            Instance = this;
        }

        public override void PreDrawScene()
        {
            if (Main.gameMenu) return;
            //Main.graphics.GraphicsDevice.SetRenderTarget(Renderers.ScreenTarget);
            //Main.graphics.GraphicsDevice.Clear(Color.Transparent);
            //_scene.Draw(_layers);
            //Main.graphics.GraphicsDevice.SetRenderTarget(null);
            foreach (var layer in _layers)
                layer.RenderToTarget();
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            _scene.Update();
        }

        public override void PostDrawScene()
        {
            _scene.Draw(_layers);
            //Main.spriteBatch.Begin();
            //Main.spriteBatch.Draw(Renderers.ScreenTarget, Vector2.Zero, Color.White);
            //Main.spriteBatch.End();
        }
    }
}