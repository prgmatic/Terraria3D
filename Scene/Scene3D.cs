using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;

namespace Terraria3D
{
    public class Scene3D
    {
        public Camera Camera { get; private set; } = new Camera();
		public CameraDriver CameraDriver { get; private set; }
		public DollyController DollyController { get; private set; }
        public Transfrom ModelTransform { get; private set; } = new Transfrom();
        public bool AmbientOcclusion { get; set; } = true;
		public Camera ActiveCamera => DollyController.DollyInProgress ? DollyController.TransitionCamera : Camera;

		private bool _canSkipDrawing => Main.gameMenu || Main.mapFullscreen;

        public Scene3D()
        {
			CameraDriver = new CameraDriver(Camera);
			DollyController = new DollyController(Camera);
        }

        public void Update(GameTime gameTime)
        {
			if (!Terraria3D.Enabled) return;
			DollyController.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            if (InputTerraria3D.CameraControlsEnabled && !DollyController.DollyInProgress)
                CameraDriver.Drive((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void RenderLayers(Layer3D[] layers)
        {
            if (!Terraria3D.Enabled || _canSkipDrawing) return;
            // Disable zoom
            Utils.SetZoom(1);
            Rendering.PreRenderSetup();
            foreach (var layer in layers)
                layer.RenderToTarget();
            // Restore zoom
            Utils.RestoreOldZoom();
        }

        public void DrawToScreen(Layer3D[] layers)
        {
            if (!Terraria3D.Enabled || _canSkipDrawing) return;
            DrawExtrusionAndCap(layers);
        }

        private void DrawExtrusions(Layer3D[] layers)
        {
            foreach (var layer in layers)
                layer.DrawExtrusion(ActiveCamera, AmbientOcclusion, _extrusionMatrix);
        }

        private void DrawCaps(Layer3D[] layers)
        {
            foreach (var layer in layers.OrderBy(l => l.Depth - l.ZPos))
                layer.DrawCap(ActiveCamera, _capMatrix);
        }

        private void DrawExtrusionAndCap(Layer3D[] layers)
        {
            foreach (var layer in layers.OrderBy(l => l.Depth - l.ZPos))
            {
                layer.DrawExtrusion(ActiveCamera, AmbientOcclusion, _extrusionMatrix);
                layer.DrawCap(ActiveCamera, _capMatrix);
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
