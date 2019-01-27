using Terraria;
using Microsoft.Xna.Framework;

namespace Terraria3D
{
    public static class Cursor3D
    {
        private static Plane _tilePlane = new Plane(Vector3.Backward, 0);

        public static Vector2 Get3DScreenPos()
            => Get3DScreenPos(Terraria3D.Instance.Scene.Camera, Terraria3D.Instance.Scene.ModelTransform.LocalToWorld);

        public static Vector2 Get3DScreenPos(Camera camera, Matrix modelMatrix)
        {
            var mousePos = new Vector2(Main.mouseX, Main.mouseY);
            return Get3DScreenPos(camera, mousePos, modelMatrix);
        }

        public static Vector2 Get3DScreenPos(Camera camera, Vector2 mousePos, Matrix modelMatrix)
        {
            // Find intersection with front plane
            _tilePlane.D = 0;
            var ray = camera.ScreenPointToRay(new Vector2(Main.mouseX, Main.mouseY));
            var invMatrix = Matrix.Invert(modelMatrix);

            var intersectPos = GetPlaneIntersectOnScreen(ray, _tilePlane, invMatrix);
            if (intersectPos.HasValue)
            {
                var iPos = new Vector2(intersectPos.Value.X, intersectPos.Value.Y);

                // Check tile under current position. If there is no collision,
                // hop to the back plane
                Vector2 tilePos = (Main.screenPosition + iPos) / 16;
                if (TileOnScreen(tilePos))
                {
                    if (!TileIsCollider(tilePos))
                    {
                        _tilePlane.D = Vector3.Transform(Vector3.Backward * 16, modelMatrix).Z;
                        intersectPos = GetPlaneIntersectOnScreen(ray, _tilePlane, invMatrix);
                        if (intersectPos.HasValue)
                            iPos = new Vector2(intersectPos.Value.X, intersectPos.Value.Y);
                    }
                }
                return iPos;
            }
            return Vector2.Zero;
        }

        // Apply matrix and flip y
        private static Vector2? GetPlaneIntersectOnScreen(Ray ray, Plane plane, Matrix? matrix)
        {
            var intersect = GetPlaneIntersectPosition(ray, plane);
            if (intersect.HasValue)
            {
                var r = new Vector2(intersect.Value.X, intersect.Value.Y);
                if (matrix.HasValue)
                    r = Vector2.Transform(r, matrix.Value);
                r.Y = Screen.Height - r.Y;
                return r;
            }
            return null;
        }

        private static Vector3? GetPlaneIntersectPosition(Ray ray, Plane plane)
        {
            var dist = ray.Intersects(_tilePlane);
            if (dist.HasValue)
                return ray.Position + ray.Direction * dist.Value;
            return null;
        }

        private static bool TileOnScreen(Vector2 tilePos)
        {
            return tilePos.X > 0 && tilePos.X < Main.maxTilesX &&
                   tilePos.Y > 0 && tilePos.Y < Main.maxTilesY;
        }

        private static bool TileIsCollider(Vector2 tilePos)
        {
            Tile tile = Main.tile[(int)tilePos.X, (int)tilePos.Y];
            return tile != null && tile.collisionType > 0;
        }
    }
}
