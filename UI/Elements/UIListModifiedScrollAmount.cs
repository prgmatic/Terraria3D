using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Terraria3D.UI.Elements
{
    public class UIListModifiedScrollAmount : UIList
    {
        public float ScrollModifier { get; set; } = 1;

        public override void ScrollWheel(UIScrollWheelEvent evt)
        {
            evt = new UIScrollWheelEvent(evt.Target, evt.MousePosition, (int)(evt.ScrollWheelValue * ScrollModifier));
            base.ScrollWheel(evt);
        }
    }
}
