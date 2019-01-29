using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ModLoader;

namespace Terraria3D
{
    public class Terraria3D : Mod
    {
        public static Terraria3D Instance { get; private set; }
        public Scene3D Scene { get; set; }
        public LayerManager LayerManager { get; set; }

        public override void Load()
        {
            Instance = this;
            Loading.Load(this);
            Main.OnPostDraw += (gt) =>
            {
                //Main.spriteBatch.Begin();
                //UITerraria3D.Update(gt);
                //UITerraria3D.Draw();
                //Main.spriteBatch.End();
            };
        }
        public override void Unload() => Loading.Unload(this);

        // Drawing
        public void RenderLayersTargets() => Scene.RenderLayers(LayerManager.Layers);
        public void DrawScene() => Scene.DrawToScreen(LayerManager.Layers);

        // UI
        public override void UpdateUI(GameTime gameTime)
        {
            //UITerraria3D.Update(gameTime);
            Scene.Update(gameTime);
            if (Main.keyState.IsKeyDown(Keys.P))
                LayerManager.Rebuild();
        }
        //public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) => UITerraria3D.ModifyInterfaceLayers(layers);
    }
}