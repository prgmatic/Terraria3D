using Terraria;

namespace Terraria3D
{
    public static partial class Hooks
    {
        private static void ApplyMouseHook()
        {
            // TODO: Hook should happen after this function is called, not in it
            // Can't hook DoUpdate, guessing because I don't have the v0.16
            // TerrariaHooks.dll
            IL.Terraria.GameContent.PortalHelper.UpdatePortalPoints += (il) =>
            {
                il.At(0).EmitDelegate(() =>
                {
                    var screenPos = Cursor3D.Get3DScreenPos();
                    Main.mouseX = (int)screenPos.X;
                    Main.mouseY = (int)screenPos.Y;
                });
            };
        }
    }
}
