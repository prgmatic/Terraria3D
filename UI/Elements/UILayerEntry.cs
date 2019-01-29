using Terraria.GameContent.UI.Elements;

namespace Terraria3D.UI.Elements
{
    public class UILayerEntry : UIPanel
    {
        UISlider _depthSlider = new UISlider(-64, 64);
        UISlider _zPosSlider = new UISlider(-64, 64);
        UISlider _noiseSlider = new UISlider();

        private int _index;

        public UILayerEntry(int index)
        {
            _index = index;

            Width.Set(0, 1);
            Height.Set(100, 0);

            _depthSlider.Left.Set(100, 0);
            _depthSlider.Width.Set(200, 0);

            _zPosSlider.Left.Set(_depthSlider.Left.Pixels, 0);
            _zPosSlider.Top.Set(25, 0);
            _zPosSlider.Width.Set(_depthSlider.Width.Pixels, 0);

            _noiseSlider.Left.Set(_depthSlider.Left.Pixels, 0);
            _noiseSlider.Top.Set(50, 0);

            Append(_depthSlider);
            Append(_zPosSlider);
            Append(_noiseSlider);
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
