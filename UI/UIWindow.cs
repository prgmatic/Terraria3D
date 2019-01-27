using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Terraria3D
{
    public class UIWindow : UIPanel
    {
        public bool Draggable { get; set; }
        private bool _dragging = false;
        private Vector2 _prevMousePos;


        public UIWindow()
        {
            Draggable = true;
            OnMouseDown += UIWindow_OnMouseDown;
            OnMouseUp += UIWindow_OnMouseUp;
        }


        void UIWindow_OnMouseDown(UIMouseEvent evt, UIElement listeningElement)
        {
            _dragging = true;
        }
        void UIWindow_OnMouseUp(UIMouseEvent evt, UIElement listeningElement)
        {
            _dragging = false;
        }

        public override void Recalculate()
        {
            if (_dragging)
            {
                Left.Set(Left.Pixels + Input.MousePosition.X - _prevMousePos.X, 0);
                Top.Set(Top.Pixels + Input.MousePosition.Y - _prevMousePos.Y, 0);
            }
            _prevMousePos = Input.MousePosition;
            base.Recalculate();
        }
    }
}
