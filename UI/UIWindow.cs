using Microsoft.Xna.Framework;
using System.Reflection;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace Terraria3D;

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
        Activate();
    }

    void UIWindow_OnMouseDown(UIMouseEvent evt, UIElement listeningElement)
    {
        if (evt.Target == this)
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
            Left.Set(Left.Pixels + PlayerInput.MouseX - _prevMousePos.X, 0);
            Top.Set(Top.Pixels + PlayerInput.MouseY - _prevMousePos.Y, 0);

            var parentDimensions = Parent.GetDimensions().ToRectangle();
            Left.Pixels = MathHelper.Clamp(Left.Pixels, 0, parentDimensions.Right - Width.Pixels);
            Top.Pixels = MathHelper.Clamp(Top.Pixels, 0, parentDimensions.Bottom - Height.Pixels);
        }
        _prevMousePos = new Vector2(PlayerInput.MouseX, PlayerInput.MouseY);
        base.Recalculate();
    }
}