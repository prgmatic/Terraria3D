using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
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
			if (Main.dedServ) return;
            Instance = this;
            Enabled = true;
            Loading.Load(this);
        }
        public override void Unload()
        {
            if (Main.dedServ) return;
            Instance = null;
            Enabled = false;
            Loading.Unload(this);
        }

        public override void LoadResourceFromStream(string path, int len, BinaryReader reader)
        {
            if (Main.dedServ) return;
			// Huge thanks to 0x0ade for this fix. It allows uses with displays 1920x1200 or
			// small to run SM3 shaders :D
			// Apply all XNA fixes before loading any resource from this mod.
			// XNAFixes.Apply() only runs once internally, and it needs to run earlier than Mod.Load()
			XNAHacks.Apply();
            if (Main.graphics.GraphicsProfile == GraphicsProfile.Reach && path.StartsWith("Effects/HiDef/")) return;
            base.LoadResourceFromStream(path, len, reader);
        }

        // Drawing
        public void RenderLayersTargets() => Scene.RenderLayers(LayerManager.Layers);
        public void DrawScene() => Scene.DrawToScreen(LayerManager.Layers);

        // UI
        public override void UpdateUI(GameTime gameTime)
        {
			InputTerraria3D.Update(gameTime);
            UITerraria3D.Update(gameTime);
            Scene.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) => UITerraria3D.ModifyInterfaceLayers(layers);
    }

	public class PlayerHooks : ModPlayer
	{
		public override void ProcessTriggers(TriggersSet triggersSet) => InputTerraria3D.ProcessInput();
		public override void SetControls() => InputTerraria3D.SetControls(player);
	}
}