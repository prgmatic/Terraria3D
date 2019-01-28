using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;

namespace Terraria3D
{
    public class UISettingsWindow : UIWindow
    {
        public UISettingsWindow(string text)
        {
            Width.Set(600, 0);
            Height.Set(400, 0);
            //this.Left.Set(-Width.Pixels * 0.5f, 0.5f);
            //this.Top.Set(-Height.Pixels * 0.5f, 0.5f);
            Recalculate();
            OnClick += (evt, listeningElement) => System.Diagnostics.Trace.WriteLine("Click");
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
