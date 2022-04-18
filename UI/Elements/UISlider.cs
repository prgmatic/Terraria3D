using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using On.Terraria.GameContent;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Terraria3D.UI.Elements;

public class UISlider : UIElement
{
    protected static Texture2D _handleTexture = Terraria.GameContent.TextureAssets.ColorSlider.Value;
    protected static Texture2D _barTexture = Main.Assets.Request<Texture2D>("Images/UI/ScrollbarInner", AssetRequestMode.ImmediateLoad).Value;

    public delegate void UISliderValueEvent(UISlider sender, float value);
    public event UISliderValueEvent ValueChanged;

    public float MinValue { get; } = 0;
    public float MaxValue { get; } = 1;
    public float Value
    {
        get { return _value; }
        set { _value = MathHelper.Clamp(value, MinValue, MaxValue); }
    }
    private float _value = 0;
    private bool _isDragging = false;

    public UISlider(float min = 0, float max = 1)
    {
        MinValue = min;
        MaxValue = max;
        Height.Set(20, 0);
        MaxHeight.Set(20, 0);
        Width.Set(100, 0);
        PaddingTop = 5f;
        PaddingBottom = 5f;
    }

    private void DrawBar(SpriteBatch spriteBatch, Texture2D texture, Rectangle dimensions, Color color)
    {
        var rot = MathHelper.ToRadians(-90);
        spriteBatch.Draw(texture, new Rectangle(dimensions.X - 6, dimensions.Y, dimensions.Height, 6), new Rectangle?(new Rectangle(0, 0, texture.Width, 6)), color, rot, Vector2.UnitX * dimensions.Height, SpriteEffects.None, 0);
        spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y, dimensions.Height, dimensions.Width), new Rectangle?(new Rectangle(0, 6, texture.Width, 4)), color, rot, Vector2.UnitX * dimensions.Height, SpriteEffects.None, 0);
        spriteBatch.Draw(texture, new Rectangle(dimensions.X + dimensions.Width, dimensions.Y, dimensions.Height, 6), new Rectangle?(new Rectangle(0, texture.Height - 6, texture.Width, 6)), color, rot, Vector2.UnitX * dimensions.Height, SpriteEffects.None, 0);
    }

    private void DrawHandle(SpriteBatch spriteBatch, Rectangle dimensions)
    {
        var x = (_value - MinValue) / (MaxValue - MinValue);
        x = MathHelper.Lerp(dimensions.Left, dimensions.Right, x);
        var pos = new Vector2(x - _handleTexture.Width * 0.5f, dimensions.Y - 2);
        spriteBatch.Draw(_handleTexture, pos, Color.White);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        if (_isDragging)
            MoveSliderUnderMouse();

        var dimensions = GetDimensions().ToRectangle();

        DrawBar(spriteBatch, _barTexture, dimensions, Color.White * (_isDragging || ContainsPoint(Main.MouseScreen) ? 1f : 0.85f));
        DrawHandle(spriteBatch, dimensions);
    }

    public override void MouseDown(UIMouseEvent evt)
    {
        base.MouseDown(evt);
        _isDragging = true;
        MoveSliderUnderMouse();
    }

    public override void MouseUp(UIMouseEvent evt)
    {
        base.MouseUp(evt);
        _isDragging = false;
    }

    private void MoveSliderUnderMouse()
    {
        var prevValue = _value;
        var pos = UserInterface.ActiveInstance.MousePosition.X - GetInnerDimensions().X;
        Value = MathHelper.Lerp(MinValue, MaxValue, pos / Width.Pixels);
        if (prevValue != _value)
            ValueChanged?.Invoke(this, _value);
    }
}