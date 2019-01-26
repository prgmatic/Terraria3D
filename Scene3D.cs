using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Terraria3D
{
    public class Scene3D
    {
        public Camera _camera = new Camera();
        private Transfrom _transform = new Transfrom();
        private RenderTarget2D _renderTarget;
        
        public Scene3D()
        {
            _camera.Transfrom.Position = Vector3.Backward * 2;
            ResetRenderTarget();
            Main.OnResolutionChanged += (size) => ResetRenderTarget();
        }

        private void ResetRenderTarget()
        {
            if (_renderTarget != null)
                _renderTarget.Dispose();
            _renderTarget = Utils.CreateRenderTarget(Screen.Width, Screen.Height);
        }

        public void Update()
        {
            // Why doesn't this game use delta time? :\
            CameraDriver.Drive(_camera, 1, 5, 1f / 60);
        }

        public void Draw(Layer3D[] layers)
        {
            _transform.Position = new Vector3(-_renderTarget.Width * 0.5f, -_renderTarget.Height * 0.5f, 0) * _transform.Scale.X;
            var scale = Vector3.One / _renderTarget.Height;
            scale.Z = 0.1f;
            _transform.Scale = scale;
            foreach (var layer in layers)
                layer.DrawExtrusion(_camera, _transform.LocalToWorld);
        }

        public void TestDraw()
        {
            _transform.Position = new Vector3(-_renderTarget.Width * 0.5f, -_renderTarget.Height * 0.5f, 0) * _transform.Scale.X;
            var scale = Vector3.One / _renderTarget.Height;
            scale.Z = 0.1f;
            _transform.Scale = scale;
            Renderers.GridRenderer.Draw(Main.instance.tileTarget, _camera, _transform.LocalToWorld);
        }

        public void DrawCap()
        {
            var matrix = Matrix.CreateScale((float)_renderTarget.Width / _renderTarget.Height, 1, 1);
            Renderers.CapRenderer.Draw(Main.instance.tileTarget, _camera, matrix);
        }
    }
}
