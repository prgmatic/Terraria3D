using Terraria.GameContent.UI.Elements;

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
        }

        public override void Recalculate()
        {
            base.Recalculate();
        }

        public override void OnActivate()
        {
            base.OnActivate();

            //Recalculate();
        }
    }
}
