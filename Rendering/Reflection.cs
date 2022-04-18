using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Terraria3D;

public static class Reflection
{
    private static FieldInfo _currentGraphicsProfile => GetField("_currentGraphicsProfile", BindingFlags.Static);
    // private static FieldInfo _gameInterfaceLayers => GetField("_gameInterfaceLayers", BindingFlags.Instance);
    //
    private static MethodInfo _drawWaters         = GetMethod("drawWaters",         BindingFlags.Instance);
    private static MethodInfo _drawBackground     = GetMethod("DrawBackground",     BindingFlags.Instance);
    private static MethodInfo _cacheNPCDraws      = GetMethod("CacheNPCDraws",      BindingFlags.Instance);
    private static MethodInfo _cacheProjDraws     = GetMethod("CacheProjDraws",     BindingFlags.Instance);
    private static MethodInfo _drawCachedNPCs     = GetMethod("DrawCachedNPCs",     BindingFlags.Instance);
    private static MethodInfo _drawCachedProjs    = GetMethod("DrawCachedProjs",    BindingFlags.Instance);
    private static MethodInfo _drawBlack          = GetMethod("DrawBlack",          BindingFlags.Instance);
    private static MethodInfo _drawWalls          = GetMethod("DrawWalls",          BindingFlags.Instance);
    // private static MethodInfo _drawWoF            = GetMethod("DrawWoF",            BindingFlags.Instance);
    // private static MethodInfo _drawGoreBehind     = GetMethod("DrawGoreBehind",     BindingFlags.Instance);
    private static MethodInfo _drawTiles          = GetMethod("DrawTiles",          BindingFlags.Instance);
    private static MethodInfo _drawNPCs           = GetMethod("DrawNPCs",           BindingFlags.Instance);
    // private static MethodInfo _sortDrawCacheWorms = GetMethod("SortDrawCacheWorms", BindingFlags.Instance);
    // private static MethodInfo _drawProjectiles    = GetMethod("DrawProjectiles",    BindingFlags.Instance);
    private static MethodInfo _drawPlayersBehindNPCs = GetMethod("DrawPlayers_BehindNPCs",        BindingFlags.Instance);
    private static MethodInfo _drawPlayersAfterProj  = GetMethod("DrawPlayers_AfterProjectiles",        BindingFlags.Instance);
    // private static MethodInfo _drawItems          = GetMethod("DrawItems",          BindingFlags.Instance);
    // private static MethodInfo _drawRain           = GetMethod("DrawRain",           BindingFlags.Instance);
    // private static MethodInfo _drawGore           = GetMethod("DrawGore",           BindingFlags.Instance);
    // private static MethodInfo _drawDust           = GetMethod("DrawDust",           BindingFlags.Instance);
    // private static MethodInfo _drawWires          = GetMethod("DrawWires",          BindingFlags.Instance);
    private static MethodInfo _drawTileEntities   = GetMethod("DrawTileEntities",   BindingFlags.Instance);
    
    
    // private static MethodInfo _modifyInterfaceLayers = typeof(Mod).Assembly.GetType("Terraria.ModLoader.ModHooks")
    //     .GetMethod("ModifyInterfaceLayers", BindingFlags.NonPublic | BindingFlags.Static);
    
    public static GraphicsProfile CurrentGraphicsProfile => (GraphicsProfile)_currentGraphicsProfile.GetValue(null);
    // public static List<GameInterfaceLayer> GameInterfaceLayers => (List<GameInterfaceLayer>)_gameInterfaceLayers.GetValue(Main.instance);
    
    public static void DrawWaters(bool bg = false, int styleOverride = -1, bool allowUpdate = true)
        => _drawWaters.Invoke(Main.instance, new object[] { bg, styleOverride, allowUpdate });
    
    public static void DrawBackground() => _drawBackground.Invoke(Main.instance, null);
    public static void CacheNPCDraws() => _cacheNPCDraws.Invoke(Main.instance, null);
    public static void CacheProjDraws() => _cacheProjDraws.Invoke(Main.instance, null);
    public static void DrawCachedNPCs(List<int> npcCache, bool behindTiles) => _drawCachedNPCs.Invoke(Main.instance, new object[] { npcCache, behindTiles });
    public static void DrawBlack(bool force = false) => _drawBlack.Invoke(Main.instance, new object[] { force });
    public static void DrawWalls() => _drawWalls.Invoke(Main.instance, null);
    // public static void DrawWoF() => _drawWoF.Invoke(Main.instance, null);
    // public static void DrawGoreBehind() => _drawGoreBehind.Invoke(Main.instance, null);
    public static void DrawTiles(bool solidOnly = true, int waterStyleOverride = -1) => _drawTiles.Invoke(Main.instance, new object[] { solidOnly, waterStyleOverride });
    public static void DrawCachedProjs(List<int> projCache, bool startSpriteBatch = true) => _drawCachedProjs.Invoke(Main.instance, new object[] { projCache, startSpriteBatch });
    public static void DrawNPCs(bool behindTiles) => _drawNPCs.Invoke(Main.instance, new object[] { behindTiles });
    // public static void SortDrawCashWorms() => _sortDrawCacheWorms.Invoke(Main.instance, null);
    // public static void DrawProjectiles() => _drawProjectiles.Invoke(Main.instance, null);
    public static void DrawPlayersBehindNPCs() => _drawPlayersBehindNPCs.Invoke(Main.instance, null);
    public static void DrawPlayersAfterProj() => _drawPlayersAfterProj.Invoke(Main.instance, null);
    // public static void DrawItems() => _drawItems.Invoke(Main.instance, null);
    // public static void DrawRain() => _drawRain.Invoke(Main.instance, null);
    // public static void DrawGore() => _drawGore.Invoke(Main.instance, null);
    // public static void DrawDust() => _drawDust.Invoke(Main.instance, null);
    // public static void DrawWires() => _drawWires.Invoke(Main.instance, null);
    
    // public static void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) => _modifyInterfaceLayers.Invoke(null, new object[] { layers });

    public static void DrawTileEntities(bool solidLayers) =>
        _drawTileEntities.Invoke(Main.instance, new object[] {solidLayers, false, false});
    
    private static FieldInfo GetField(string fieldName, BindingFlags bindingFlags)
        => typeof(Main).GetField(fieldName, BindingFlags.NonPublic | bindingFlags);
    
    private static MethodInfo GetMethod(string methodName, BindingFlags bindingFlags)
        => typeof(Main).GetMethod(methodName, BindingFlags.NonPublic | bindingFlags);
}