using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace Terraria3D
{
    public class Terraria3D : Mod
    {
        public static bool Enabled { get; set; } = true;
        public static Terraria3D Instance { get; private set; }
        public Scene3D Scene { get; set; }
        public LayerManager LayerManager { get; set; }

		public async void Toggle()
		{
			if (Scene.DollyController.DollyInProgress) return;
			if(Enabled)
			{
				await Scene.DollyController.TransitionOutAsync();
				Enabled = false;
			}
			else
			{
				Enabled = true;
				Scene.DollyController.TransitionIn();
			}

		}

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

        public override bool LoadResource(string path, int length, Func<Stream> getStream)
        {
            if (Main.dedServ || (!Renderers.SM3Enabled && path.StartsWith("Effects/HiDef"))) return false;
            return base.LoadResource(path, length, getStream);
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
		public override void OnEnterWorld(Player player)
		{
			// Hack for overhaul to stop black tiles from persisting.
			Settings.Load();
			if (Main.instance.blackTarget != null && !Main.instance.blackTarget.IsDisposed)
			{
				Main.graphics.GraphicsDevice.SetRenderTarget(Main.instance.blackTarget);
				Main.graphics.GraphicsDevice.Clear(Color.Transparent);
				Main.graphics.GraphicsDevice.SetRenderTarget(null);
			}
		}
		public override TagCompound Save()
		{
			Settings.Save();
			return base.Save();
		}
	}
}