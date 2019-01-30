using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;

namespace Terraria3D
{
    public class Terraria3D : Mod
    {
        public static bool Enabled { get; set; } = true;
        public static Terraria3D Instance { get; private set; }
        public Scene3D Scene { get; set; }
        public LayerManager LayerManager { get; set; }

        public override void Load()
        {
            Instance = this;
            Enabled = true;
            Loading.Load(this);
        }
        public override void Unload() => Loading.Unload(this);

        // Drawing
        public void RenderLayersTargets() => Scene.RenderLayers(LayerManager.Layers);
        public void DrawScene() => Scene.DrawToScreen(LayerManager.Layers);

        // UI
        public override void UpdateUI(GameTime gameTime)
        {
            UITerraria3D.Update(gameTime);
            Scene.Update(gameTime);
            if (Main.keyState.IsKeyDown(Keys.P))
                LayerManager.Rebuild();
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) => UITerraria3D.ModifyInterfaceLayers(layers);
    }

    public class PlayerHooks : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet) => UITerraria3D.ProcessInput();
    }
}