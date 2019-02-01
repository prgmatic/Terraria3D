using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;

namespace Terraria3D
{
    public class Scene3D
    {
        public bool Enable { get; private set; } = true;
        public Camera Camera { get; private set; } = new Camera();
        public Transfrom ModelTransform { get; private set; } = new Transfrom();

        private bool _canSkipDrawing => Main.gameMenu || Main.mapFullscreen;

        public Scene3D()
        {
            Camera.Transfrom.Position = Vector3.Backward * 0.6f;
        }

        public void Update(GameTime gameTime)
        {
            if (UITerraria3D.CameraContolsEnabled)
                CameraDriver.Drive(Camera, 0.2f, 5f, (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void RenderLayers(Layer3D[] layers)
        {
            if (!Enable || _canSkipDrawing) return;
            // Disable zoom
            var oldZoom = Main.GameZoomTarget;
            Utils.SetZoom(1);
            Rendering.PreRenderSetup();
            foreach (var layer in layers)
                layer.RenderToTarget();
            // Restore zoom
            Utils.SetZoom(oldZoom);
        }

        public void DrawToScreen(Layer3D[] layers)
        {
            if (!Enable || _canSkipDrawing) return;
            DrawExtrusionAndCap(layers);
        }

        private void DrawExtrusions(Layer3D[] layers)
        {
            foreach (var layer in layers)
                layer.DrawExtrusion(Camera, _extrusionMatrix);
        }

        private void DrawCaps(Layer3D[] layers)
        {
            foreach (var layer in layers.OrderBy(l => l.Depth - l.ZPos))
                layer.DrawCap(Camera, _capMatrix);
        }

        private void DrawExtrusionAndCap(Layer3D[] layers)
        {
            foreach (var layer in layers.OrderBy(l => l.Depth - l.ZPos))
            {
                layer.DrawExtrusion(Camera, _extrusionMatrix);
                layer.DrawCap(Camera, _capMatrix);
            }
        }

        private Matrix _extrusionMatrix
        {
            get
            {
                ModelTransform.Scale = Vector3.One / Screen.Height;
                ModelTransform.Position = new Vector3(-Screen.Width * 0.5f, -Screen.Height * 0.5f, 0) * ModelTransform.Scale.X;
                return ModelTransform.LocalToWorld;
            }
        }

        private Matrix _capMatrix
        {
            get
            {
                float s = (float)RTManager.Height / Screen.Height;
                var scale = Matrix.CreateScale((float)RTManager.Width / RTManager.Height, 1, 1) *
                            Matrix.CreateScale(s, s, 1f / Screen.Height);
                var translation = Matrix.CreateTranslation(0.5f - ((float)Screen.Width / RTManager.Width) * 0.5f,
                                                          -0.5f + ((float)Screen.Height / RTManager.Height) * 0.5f, 0);
                return translation * scale;
            }
        }
    }
}
