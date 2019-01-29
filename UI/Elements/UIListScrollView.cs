using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ModLoader.UI;

namespace Terraria3D.UI.Elements
{
    public class UIListScrollView : UIPanel
    {
        public UIListModifiedScrollAmount List { get; private set; } = new UIListModifiedScrollAmount();
        public UIScrollbar ScrollBar { get; private set; } = new UIScrollbar();

        public UIListScrollView()
        {
            List.ScrollModifier = 0.5f;

            ScrollBar.Height.Set(0, 1);
            ScrollBar.HAlign = 1f;

            List.SetScrollbar(ScrollBar);
            List.Width.Set(-30, 1);
            List.Height.Set(0, 1);

            Append(List);
            Append(ScrollBar);
        }
    }
}
