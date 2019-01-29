namespace Terraria3D
{
    public static class Loading
    {
        public static void Load(Terraria3D instance)
        {
            Renderers.Load();
            instance.Scene = new Scene3D();
            instance.LayerManager = new LayerManager();
            UITerraria3D.Load();
            Hooks.Initialize();
        }

        public static void Unload(Terraria3D instance)
        {
            UITerraria3D.Unload();
            instance.Scene = null;
            instance.LayerManager.Dispose();
            instance.LayerManager = null;
            Renderers.Unload();
        }
    }
}
