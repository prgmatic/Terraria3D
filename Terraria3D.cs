using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Terraria3D
{
    class Terraria3D : Mod
    {
        public static Terraria3D Instance { get; private set; }

        public Scene3D Scene { get; private set; } = new Scene3D();

        private UISettingsWindow _settingsWinow = new UISettingsWindow("3D Settings");
        private Layer3D[] _layers;

        public override void Load()
        {
            Instance = this;
            Layers.PopulateLayers(ref _layers);
            Hooks.Initialize();
            Main.OnPostDraw += (gt) =>
            {
                //Main.spriteBatch.Begin();
                //UITerraria3D.Update(gt);
                //UITerraria3D.Draw();
                //Main.spriteBatch.End();
            };
        }

        // Drawing
        public void RenderLayersTargets() => Scene.RenderLayers(_layers);
        public void DrawScene() => Scene.DrawToScreen(_layers);

        // UI
        public override void UpdateUI(GameTime gameTime)
        {
            UITerraria3D.Update(gameTime);
            Scene.Update(gameTime);
            if (Main.keyState.IsKeyDown(Keys.P))
                Layers.PopulateLayers(ref _layers);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) => UITerraria3D.ModifyInterfaceLayers(layers);
    }
}