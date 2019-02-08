namespace Terraria3D
{
    public static class Loading
    {
        public static void Load(Terraria3D instance)
        {
            var mode = Reflection.CurrentGraphicsProfile;
            Renderers.Load();
            instance.Scene = new Scene3D();
            instance.LayerManager = new LayerManager();
            UITerraria3D.Load();
            Hooks.Initialize();
			InputTerraria3D.Load();
        }

        public static void Unload(Terraria3D instance)
        {
            UITerraria3D.Unload();
            instance.Scene = null;
            instance.LayerManager?.Dispose();
            instance.LayerManager = null;
            Renderers.Unload();
			InputTerraria3D.Unload();
        }
    }
}
