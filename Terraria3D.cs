using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            Input.Update();
            if (Main.keyState.IsKeyDown(Keys.P))
                Layers.PopulateLayers(ref _layers);
            Scene.Update();
            //_settingsWinow.Draw(spriteBatch);
        }

        public void RenderLayersTargets()
        {
            Rendering.CacheDraws();
            if (Main.gameMenu) return;
            foreach (var layer in _layers)
                layer.RenderToTarget();
        }

        public void DrawScene()
        {
            Scene.Draw(_layers);
            Cursor3D.Get3DScreenPos(Scene.Camera, new Vector2(Main.mouseX, Main.mouseY), Scene.ModelTransform.LocalToWorld);
        }

        public override void UpdateUI(GameTime gameTime) => UITerraria3D.Update(gameTime);

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (inventoryIndex != -1)
            {
                layers.Insert(inventoryIndex + 1, new LegacyGameInterfaceLayer(
                    "ExampleMod: Example Person UI", () => 
                    {
                        if (UITerraria3D.Visible)
                            UITerraria3D.Draw();
                        return true;
                    }, InterfaceScaleType.UI)
                );
            }
        }
    }
}