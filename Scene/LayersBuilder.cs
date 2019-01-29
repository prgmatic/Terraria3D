
namespace Terraria3D
{
    public static class LayerBuilder
    {
        public static void PopulateLayers(ref Layer3D[] layers)
        {
            if (layers != null)
            {
                foreach (var layer in layers)
                    layer.Dispose();
            }
            layers = new Layer3D[]
            {
                new Layer3D()
                {
                    // Background
                    ZPos = 32,
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
                    Depth = 32,
                    RenderFunction = () =>
                    {
                        Rendering.DrawSolidTiles();
                    }
                },
                new Layer3D()
                {
                    Depth = 32,
                    RenderFunction = () =>
                    {
                        Rendering.DrawForegroundWater();
                    }
                },
                // Non Solid tiles
                new Layer3D()
                {
                    ZPos = 24,
                    Depth = 8,
                    RenderFunction = () =>
                    {
                        Rendering.DrawNonSolidTiles();
                        Rendering.DrawWaterFalls();

                    }
                },
                //Player
                new Layer3D()
                {
                    ZPos = 12,
                    Depth = 16,
                    NoiseAmount = 0,
                    RenderFunction = () =>
                    {
                        Rendering.DrawPlayers();
                        Rendering.DrawNPCsBehindTiles();
                        Rendering.DrawNPCsBehindNonSoldTiles();
                        Rendering.DrawNPCsInfrontOfTiles();
                        Rendering.DrawNPCsOverPlayer();
                        Rendering.DrawWallOfFlesh();
                        Rendering.SortDrawCacheWorm();
                    }
                },
                // Proj
                new Layer3D()
                {
                    ZPos = 16,
                    Depth = 2,
                    NoiseAmount = 0,
                    RenderFunction = () =>
                    {
                        Rendering.DrawProjectiles();
                        Rendering.DrawProjsBehindNPCsAndTiles();
                        Rendering.DrawProjsBehindProjectiles();
                        Rendering.DrawProjsOverWireUI();
                        Rendering.DrawNPCProjectiles();
                        Rendering.DrawInfernoRings();

                    }
                },
                // Items Gore
                new Layer3D()
                {
                    ZPos = 14,
                    Depth = 6,
                    NoiseAmount = 0,
                    RenderFunction = () =>
                    {
                        Rendering.DrawGore();
                        Rendering.DrawBackGore();
                        Rendering.DrawDust();
                        Rendering.DrawRain();
                        Rendering.DrawSandstorm();
                        Rendering.DrawMoonLordDeath();
                        Rendering.DrawMoonlordDeathFront();
                        Rendering.DrawItems();
                        Rendering.DrawHitTileAnimation();
                        Rendering.DrawItemText();
                        Rendering.DrawCombatText();
                        Rendering.DrawChatOverPlayerHeads();
                    }
                },
                new Layer3D()
                {
                    ZPos = -8,
                    Depth = 8,
                    RenderFunction = () => Rendering.DrawWires()
                }
            };
        }
    }
}
