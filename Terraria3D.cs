using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace Terraria3D
{
    class Terraria3D : Mod
    {
        public static Terraria3D Instance { get; private set; }
        public static ExtrudeTest ExtrudeTest { get; private set; }
        public static RenderTarget2D RenderTarget { get; private set; }
        public static RenderTarget2D DummyTarget { get; set; }
        private Texture2D _texture;
        private GraphicsDevice _graphics => Main.graphics.GraphicsDevice;

        public override void PostSetupContent()
        {
            Instance = this;
            Screen.Initialize(Main.graphics.GraphicsDevice);
            var effect = GetEffect("Effects/Grid");
            _texture = GetTexture("Images/Player");
            ExtrudeTest = new ExtrudeTest(effect, Main.instance.tileTarget.Width, Main.instance.tileTarget.Height);
            RenderTarget = new RenderTarget2D(_graphics, Main.instance.tileTarget.Width,
                                                         Main.instance.tileTarget.Height,
                                                         false, _graphics.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            DummyTarget = new RenderTarget2D(_graphics, 16, 16);
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            ExtrudeTest.Update(1f / 60);
        }

        public override void PreDrawScene()
        {
            _graphics.SetRenderTarget(DummyTarget);
        }

        public override void PostDrawScene()
        {
            Main.graphics.GraphicsDevice.SetRenderTarget(null);
            Main.spriteBatch.Draw(RenderTarget, Vector2.Zero, Color.White);
            if (Terraria3D.ExtrudeTest != null)
            {
                Terraria3D.ExtrudeTest.Draw(Main.graphics.GraphicsDevice, Main.instance.blackTarget);
                Terraria3D.ExtrudeTest.Draw(Main.graphics.GraphicsDevice, Main.instance.wallTarget);
                Terraria3D.ExtrudeTest.Draw(Main.graphics.GraphicsDevice, Main.instance.tileTarget);
                Terraria3D.ExtrudeTest.Draw(Main.graphics.GraphicsDevice, Main.instance.tile2Target);
                var offset = Matrix.CreateTranslation(new Vector3(Main.offScreenRange, -Main.offScreenRange, 0));
                Terraria3D.ExtrudeTest.Draw(Main.graphics.GraphicsDevice, RenderTarget, offset);
            }
        }

        class Terraria3DModWorld : ModWorld
        {
            public override void PostDrawTiles()
            {
                //if (Main.graphics.GraphicsDevice.GetRenderTargets().Length > 0)
                //    Terraria3D.OldTarget = Main.graphics.GraphicsDevice.GetRenderTargets()[0].RenderTarget as RenderTarget2D;
                //else
                //    Terraria3D.OldTarget = null;
                //Main.NewText(Main.graphics.GraphicsDevice.GetRenderTargets().Length.ToString());
                Main.graphics.GraphicsDevice.SetRenderTarget(null);
                Main.graphics.GraphicsDevice.SetRenderTarget(Terraria3D.RenderTarget);
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);

                //Main.


            }

        }
    }
