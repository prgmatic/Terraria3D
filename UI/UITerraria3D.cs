using Microsoft.Xna.Framework;
using Terraria.UI;

namespace Terraria3D
{
    public static class UITerraria3D
    {
        public static bool Visible { get; private set; } = true;

        private static UserInterface _interface = new UserInterface();
        private static UIState _state = new UIState();
        private static UISettingsWindow _settingsWindow = new UISettingsWindow("Settings");

        static UITerraria3D()
        {
            _interface.SetState(_state);
            _state.Append(_settingsWindow);
            _state.Activate();
        }

        public static void Update(GameTime gameTime)
            => _interface.Update(gameTime);

        public static void Draw()
            => _interface.Draw(Terraria.Main.spriteBatch, new GameTime());
    }
}
