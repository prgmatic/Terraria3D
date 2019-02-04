namespace Terraria3D
{
    public static partial class Hooks
    {
        public static void Initialize()
        {
            ApplyDrawHooks();
            ApplyMouseHook();
            ApplyDrawTileHooks();
        }
    }
}
