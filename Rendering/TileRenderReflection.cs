using System.Reflection;
using Terraria;
using Terraria.GameContent.Drawing;

namespace Terraria3D;

public static class TileRenderReflection
{
    private static MethodInfo _drawMultiTileVines = GetMethod("DrawMultiTileVines", BindingFlags.Instance);
    private static MethodInfo _drawMultiTileGrass = GetMethod("DrawMultiTileGrass", BindingFlags.Instance);
    private static MethodInfo _drawVoidLenses = GetMethod("DrawVoidLenses", BindingFlags.Instance);
    private static MethodInfo _drawTeleportationPylons = GetMethod("DrawTeleportationPylons", BindingFlags.Instance);
    private static MethodInfo _drawMasterTrophies = GetMethod("DrawMasterTrophies", BindingFlags.Instance);
    private static MethodInfo _drawGrass = GetMethod("DrawGrass", BindingFlags.Instance);
    private static MethodInfo _drawVines = GetMethod("DrawVines", BindingFlags.Instance);
    private static MethodInfo _drawTrees = GetMethod("DrawTrees", BindingFlags.Instance);
    private static MethodInfo _drawReverseVines = GetMethod("DrawReverseVines", BindingFlags.Instance);
    private static MethodInfo _drawHatRacks = GetMethod("DrawEntities_HatRacks", BindingFlags.Instance);
    private static MethodInfo _drawDisplayDolls = GetMethod("DrawEntities_DisplayDolls", BindingFlags.Instance);

    public static void DrawNonSolidLayers()
    {
        DrawMultiTileVines();
        DrawMultiTileGrass();
        DrawVoidLenses();
        DrawTeleportationPylons();
        DrawMasterTrophies();
        DrawGrass();
        DrawTrees();
        DrawVines();
        DrawReverseVines();
    }

    public static void DrawSolidLayers()
    {
        DrawHatRacks();
        DrawDisplayDolls();
    }
    
    private static void DrawMultiTileVines() => _drawMultiTileVines.Invoke(Main.instance.TilesRenderer, null);
    private static void DrawMultiTileGrass() => _drawMultiTileGrass.Invoke(Main.instance.TilesRenderer, null);
    private static void DrawVoidLenses() => _drawVoidLenses.Invoke(Main.instance.TilesRenderer, null);
    private static void DrawTeleportationPylons() => _drawTeleportationPylons.Invoke(Main.instance.TilesRenderer, null);
    private static void DrawMasterTrophies() => _drawMasterTrophies.Invoke(Main.instance.TilesRenderer, null);
    private static void DrawGrass() => _drawGrass.Invoke(Main.instance.TilesRenderer, null);
    private static void DrawVines() => _drawVines.Invoke(Main.instance.TilesRenderer, null);
    private static void DrawTrees() => _drawTrees.Invoke(Main.instance.TilesRenderer, null);
    private static void DrawReverseVines() => _drawReverseVines.Invoke(Main.instance.TilesRenderer, null);
    private static void DrawHatRacks() => _drawHatRacks.Invoke(Main.instance.TilesRenderer, null);
    private static void DrawDisplayDolls() => _drawDisplayDolls.Invoke(Main.instance.TilesRenderer, null);

    private static MethodInfo GetMethod(string methodName, BindingFlags bindingFlags)
        => typeof(TileDrawing).GetMethod(methodName, BindingFlags.NonPublic | bindingFlags);
}