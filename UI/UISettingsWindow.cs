using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria3D.UI.Elements;

namespace Terraria3D
{
    public class UISettingsWindow : UIWindow
    {
        private UIListScrollView _scrollView = new UIListScrollView();

        public UISettingsWindow(string text)
        {
            Width.Set(600, 0);
            Height.Set(400, 0);
            _scrollView.Width.Set(500, 0);
            _scrollView.Height.Set(300, 0);
            _scrollView.HAlign = 0.5f;
            
            Append(_scrollView);

            for (int i = 0; i < 30; i++)
                _scrollView.List.Add(new UILayerEntry(i));

            Recalculate();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            // Block cursor from clicking world.
            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
                Main.LocalPlayer.mouseInterface = true;
            base.DrawSelf(spriteBatch);
        }
    }
}
