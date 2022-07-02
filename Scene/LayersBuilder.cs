

namespace Terraria3D;

public static class LayerBuilder
{
    public static void PopulateLayers(ref Layer3D[] layers)
    {
        if (layers != null)
        {
            foreach (var layer in layers)
                layer?.Dispose();
        }
        layers = new Layer3D[]
        {
            new Layer3D()
            {
                Name = "Background",
                ZPos = 4,
                Depth = 4,
                RenderFunction = () =>
                {
                    Rendering.DrawBlack();
                    Rendering.DrawBackgroundWater();
                    Rendering.DrawSceneBackground();
                    Rendering.DrawWalls();
                    
                }
            },
            // Solid tiles
            new Layer3D()
            {
                Name = "Solid Tiles",
                Depth = 32,
                InputPlane = Layer3D.InputPlaneType.SolidTiles,
                RenderFunction = () =>
                {
                    Rendering.DrawSolidTiles();
                    //TileRenderReflection.DrawSolidLayers();
                }
            },
            // Non Solid tiles
            new Layer3D()
            {
                Name = "Non Solid Tiles",
                Depth = 8,
                InputPlane = Layer3D.InputPlaneType.NoneSolidTiles,
                RenderFunction = () =>
                {
                    Rendering.DrawNonSolidTiles();
                    Rendering.DrawWaterFalls();
                    TileDrawReflection.DrawNonSolidLayers();

                }
            },
            //Player
            new Layer3D()
            {
                Name = "Characters",
                ZPos = -18,
                Depth = 6,
                NoiseAmount = 0,
                RenderFunction = () =>
                {
                    Rendering.DrawFirstFractals();
                    Rendering.DrawMoonMoon();
                    Rendering.DrawNPCsBehindTiles();
                    Rendering.SortDrawCacheWorm();
                    Rendering.DrawWallOfFlesh();
                    Rendering.DrawNPCsBehindNonSoldTiles();
                    Rendering.DrawNPCsInfrontOfTiles();
                    Rendering.DrawPlayersBehindNPCs();
                    Rendering.DrawPlayersAfterProjs();
                    Rendering.DrawNPCsOverPlayer();
                    Rendering.DrawProjsOverPlayers();
                }
            },
            // Proj
            new Layer3D()
            {
                Name = "Projectiles",
                ZPos = -20,
                Depth = 2,
                NoiseAmount = 0,
                RenderFunction = () =>
                {
                    Rendering.DrawProjsBehindNPCsAndTiles();
                    Rendering.DrawProjsBehindNPCs();
                    Rendering.DrawProjsBehindProjectiles();
                    Rendering.DrawProjectiles();
                    Rendering.DrawInfernoRings();
                    Rendering.DrawProjsOverWireUI();
                    Rendering.DrawNPCProjectiles();
                }
            },
            // Items Gore
            new Layer3D()
            {
                Name = "Gore - Weather - Items",
                ZPos = -12,
                Depth = 6,
                NoiseAmount = 0,
                RenderFunction = () =>
                {
                    Rendering.DrawGoreBehind();
                    Rendering.DrawGore();
                    Rendering.DrawDust();
                    Rendering.DrawRain();
                    Rendering.DrawMoonLordDeath();
                    Rendering.DrawMoonlordDeathFront();
                    Rendering.DrawItems();
                }
            },
            new Layer3D()
            {
                Name = "Water Foreground",
                Depth = 32,
                RenderFunction = Rendering.DrawForegroundWater
            },
            new Layer3D()
            {
                Name = "Wires - UI",
                ZPos = -32,
                Depth = 4,
                RenderFunction = () =>
                {
                    Rendering.DrawWires();
                    Rendering.DrawHitTileAnimation();
                    Rendering.DrawItemText();
                    Rendering.DrawCombatText();
                    Rendering.DrawChatOverPlayerHeads();
                    InterfaceRendering.RenderGameInterfaces();
                }
            }
        };
    }
}