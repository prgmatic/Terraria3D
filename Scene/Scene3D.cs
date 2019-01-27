using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;

namespace Terraria3D
{
    public class Scene3D
    {
        public Camera _camera = new Camera();
        private Transfrom _transform = new Transfrom();
        
        public Scene3D()
        {
            _camera.Transfrom.Position = Vector3.Backward * 2;
        }

        public void Update()
        {
            // Why doesn't this game use delta time? :\
            CameraDriver.Drive(_camera, 0.2f, 5, 1f / 60);
        }

        public void Draw(Layer3D[] layers)
        {
            DrawExtrusions(layers);
            DrawCaps(layers);
        }

        private void DrawExtrusions(Layer3D[] layers)
        {
            _transform.Position = new Vector3(-Screen.Width * 0.5f, -Screen.Height * 0.5f, 0) * _transform.Scale.X;
            _transform.Scale = Vector3.One / Screen.Height;
            foreach (var layer in layers)
                layer.DrawExtrusion(_camera, _transform.LocalToWorld);
        }

        private void DrawCaps(Layer3D[] layers)
        {
            var matrix = Matrix.CreateScale((float)Screen.Width / Screen.Height, 1, 1f / Screen.Height);
            foreach (var layer in layers.OrderBy(l => -l.ZPos))
                layer.DrawCap(_camera, matrix);
        }
    }
}
