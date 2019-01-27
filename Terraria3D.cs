using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ModLoader;

namespace Terraria3D
{
    class Terraria3D : Mod
    {
        public static Terraria3D Instance { get; private set; }

        private UISettingsWindow _settingsWinow = new UISettingsWindow("3D Settings");
        private Scene3D _scene = new Scene3D();
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
            _scene.Update();
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
            _scene.Draw(_layers);
        }
    }
}