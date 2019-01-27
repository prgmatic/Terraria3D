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
                // Background
                ZPos = 16,
                Depth = 4,
                RenderFunction = () =>
                {
                    Rendering.DrawBackgroundWater();
                    Rendering.DrawSceneBackground();
                    Rendering.DrawWalls();
                }
            },
            new Layer3D()
            {
                RenderFunction = () =>
                {
                    Rendering.DrawSolidTiles();
                }
            },
            new Layer3D()
            {
                RenderFunction = () =>
                {
                    Rendering.DrawNonSolidTiles();
                }
            },
            new Layer3D()
            {
                ZPos = 6,
                Depth = 10,
                NoiseAmount = 0,
                RenderFunction = () =>
                {
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
            Rendering.CacheDraws();
            if (Main.gameMenu) return;
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
        }
    }
}