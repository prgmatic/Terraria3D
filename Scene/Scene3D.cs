using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;

namespace Terraria3D
{
    public class Scene3D
    {
        public Camera Camera { get; private set; } = new Camera();
        public Transfrom ModelTransform { get; private set; } = new Transfrom();
        
        public Scene3D()
        {
            Camera.Transfrom.Position = Vector3.Backward * 0.2f;
        }

        public void Update()
        {
            // Why doesn't this game use delta time? :\
            CameraDriver.Drive(Camera, 0.2f, 5, 1f / 60);
        }

        public void Draw(Layer3D[] layers)
        {
            DrawExtrusions(layers);
            DrawCaps(layers);
        }

        private void DrawExtrusions(Layer3D[] layers)
        {
            ModelTransform.Position = new Vector3(-Screen.Width * 0.5f, -Screen.Height * 0.5f, 0) * ModelTransform.Scale.X;
            ModelTransform.Scale = Vector3.One / Screen.Height;
            foreach (var layer in layers)
                layer.DrawExtrusion(Camera, ModelTransform.LocalToWorld);
        }

        private void DrawCaps(Layer3D[] layers)
        {
            var matrix = Matrix.CreateScale((float)Screen.Width / Screen.Height, 1, 1f / Screen.Height);
            foreach (var layer in layers.OrderBy(l => -l.ZPos))
                layer.DrawCap(Camera, matrix);
        }
    }
}
