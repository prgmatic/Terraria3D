using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;

namespace Terraria3D
{
    public class CameraDriver
    {
        public static void Drive(Camera camera, float moveSpeed, float lookSensitivty, float deltaTime)
        {
            var keyState = Terraria.Main.keyState;

            var x = 0;
            var y = 0;
            var z = 0;

            if (keyState.IsKeyDown(Keys.NumPad4)) x -= 1;
            if (keyState.IsKeyDown(Keys.NumPad6)) x += 1;
            if (keyState.IsKeyDown(Keys.NumPad8)) z += 1;
            if (keyState.IsKeyDown(Keys.NumPad5)) z -= 1;
            if (keyState.IsKeyDown(Keys.NumPad9)) y += 1;
            if (keyState.IsKeyDown(Keys.NumPad7)) y -= 1;

            camera.Transfrom.Position += camera.Transfrom.Forward * z * moveSpeed * deltaTime;
            camera.Transfrom.Position += camera.Transfrom.Right * x * moveSpeed * deltaTime;
            camera.Transfrom.Position += camera.Transfrom.Up * y * moveSpeed * deltaTime;

            if (Terraria.Main.mouseRight)
            {
                var mouseDelta = new Vector2(Main.mouseX - Main.lastMouseX, Main.mouseY - Main.lastMouseY);
                camera.Transfrom.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.Right, MathHelper.ToRadians(-mouseDelta.Y * lookSensitivty / 360));
                camera.Transfrom.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(-mouseDelta.X * lookSensitivty / 360));
            }
        }
    }
}
