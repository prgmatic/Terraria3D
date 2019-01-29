using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;

namespace Terraria3D.UI.Elements
{
    public class UILayerEntry : UIPanel
    {
        private static Texture2D _dividerTexture = TextureManager.Load("Images/UI/Divider");
        private static Texture2D _innerPanelTexture = TextureManager.Load("Images/UI/InnerPanelBackground");

        public Layer3D Layer
        {
            get { return _layer; }
            set { SetLayer(value); }
        }

        private Layer3D _layer;

        private UIText _layerName = new UIText(string.Empty);
        private UISlider _depthSlider = new UISlider(0, 128);
        private UISlider _zPosSlider = new UISlider(-64, 64);
        private UISlider _noiseSlider = new UISlider();

        private UIText _depthText = new UIText("Depth");
        private UIText _zPosText = new UIText("ZPos");
        private UIText _noiseText = new UIText("Noise");

        private UIText _innerPixelText = new UIText("Inner Pixel");

        private UITextPanel<string> _enableButton = new UITextPanel<string>("Enabled");
        private UITextPanel<string> _innerPixelButton = new UITextPanel<string>("On");

        private int _index;

        public UILayerEntry(int index)
        {
            _index = index;

            Width.Set(0, 1);
            SetPadding(6f);

            _layerName.Left.Pixels = 5;
            _layerName.Top.Pixels = 5;

            _depthSlider.Left.Pixels = 65;
            _depthSlider.Top.Pixels = 40;
            _depthSlider.Width.Pixels = 200;

            _zPosSlider.Left = _depthSlider.Left;
            _zPosSlider.Top.Pixels = _depthSlider.Top.Pixels + 25;
            _zPosSlider.Width = _depthSlider.Width;

            _noiseSlider.Left = _depthSlider.Left;
            _noiseSlider.Top.Pixels = _zPosSlider.Top.Pixels + 25;
            _noiseSlider.Width = _depthSlider.Width;

            _enableButton.HAlign = 1;
            _enableButton.VAlign = 1;
            _enableButton.Top.Pixels = -40;
            _enableButton.Left.Pixels = -70;

            _innerPixelButton.HAlign = 1;
            _innerPixelButton.VAlign = 1;
            _innerPixelButton.Top.Pixels = -5;
            _innerPixelButton.Left.Pixels = -5;

            _innerPixelText.HAlign = 1;
            _innerPixelText.VAlign = 1;
            _innerPixelText.Top.Pixels = _innerPixelButton.Top.Pixels + -10;
            _innerPixelText.Left.Pixels = -60;

            Height.Set(_noiseSlider.Top.Pixels + _noiseSlider.Height.Pixels + 20, 0);

            _depthText.Left = _layerName.Left;
            _depthText.Top = _depthSlider.Top;

            _zPosText.Left = _layerName.Left;
            _zPosText.Top = _zPosSlider.Top;

            _noiseText.Left = _layerName.Left;
            _noiseText.Top = _noiseSlider.Top;

            Append(_layerName);
            Append(_depthSlider);
            Append(_zPosSlider);
            Append(_noiseSlider);
            Append(_enableButton);
            Append(_innerPixelButton);
            Append(_innerPixelText);
            Append(_depthText);
            Append(_zPosText);
            Append(_noiseText);

            _depthSlider.ValueChanged += (sender, value) => { if (_layer != null) _layer.Depth = value; };
            _zPosSlider .ValueChanged += (sender, value) => { if (_layer != null) _layer.ZPos = value; };
            _noiseSlider.ValueChanged += (sender, value) => { if (_layer != null) _layer.NoiseAmount = value; };
            _enableButton.OnClick += (evt, listener) =>
            {
                _layer.Enabled = !_layer.Enabled;
                _enableButton.SetText(_layer.Enabled ? "Enabled" : "Disabled");
            };
            _innerPixelButton.OnClick += (evt, listener) =>
            {
                _layer.UseInnerPixel = !_layer.UseInnerPixel;
                _innerPixelButton.SetText(_layer.UseInnerPixel ? "On" : "Off");
            };
        }

        public void SetLayer(Layer3D layer)
        {
            _layer = layer;
            if (layer != null)
            {
                _layerName.SetText(layer.Name);
                _depthSlider.Value = layer.Depth;
                _zPosSlider.Value = layer.ZPos;
                _noiseSlider.Value = layer.NoiseAmount;
                _enableButton.SetText(_layer.Enabled ? "Enabled" : "Disabled");
                _innerPixelButton.SetText(_layer.UseInnerPixel ? "On" : "Off");
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            var innerDimensions = GetInnerDimensions();
            //draw divider
            Vector2 drawPos = new Vector2(innerDimensions.X + 5f, innerDimensions.Y + 30f);
            spriteBatch.Draw(_dividerTexture, drawPos, null, Color.White, 0f, Vector2.Zero, new Vector2((innerDimensions.Width - 10f) / 8f, 1f), SpriteEffects.None, 0f);
        }

        public override int CompareTo(object obj)
        {
            var other = obj as UILayerEntry;
            if (other != null)
                return _index - other._index;
            return 0;
        }
        
    }
}
