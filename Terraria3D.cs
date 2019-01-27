using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Terraria3D
{
    class Terraria3D : Mod
    {
        public static Terraria3D Instance { get; private set; }

        private UISettingsWindow _settingsWinow = new UISettingsWindow("3D Settings");
        private Scene3D _scene = new Scene3D();
        private Layer3D[] _layers;

        public override void PostSetupContent()
        {
            Instance = this;
            InitLayers();
        }

        public override void PreDrawScene()
        {
            Rendering.CacheDraws();
            if (Main.gameMenu) return;
            foreach (var layer in _layers)
                layer.RenderToTarget();
        }

        private void InitLayers()
        {
            if(_layers != null)
            {
                foreach (var layer in _layers)
                    layer.Dispose();
            }
            _layers = new Layer3D[]
            {
                new Layer3D()
                {
                    // Background
                    ZPos = 16,
                    Depth = 4,
                    RenderFunction = () =>
                    {
                        Rendering.DrawBackgroundWater();
                        Rendering.DrawSceneBackground();
                        Rendering.DrawWalls();
                    }
                },
                // Solid tiles
                new Layer3D()
                {
                    RenderFunction = () =>
                    {
                        Rendering.DrawSolidTiles();
                        Rendering.DrawForegroundWater();
                    }
                },
                // Non Solid tiles
                new Layer3D()
                {
                    ZPos = 12,
                    Depth = 4,
                    RenderFunction = () =>
                    {
                        Rendering.DrawNonSolidTiles();
                        Rendering.DrawWaterFalls();

                    }
                },
                //Player
                new Layer3D()
                {
                    ZPos = 6,
                    Depth = 8,
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
                    ZPos = 8,
                    Depth = 1,
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
                    ZPos = 7,
                    Depth = 3,
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
                    ZPos = -4,
                    Depth = 4,
                    RenderFunction = () => Rendering.DrawWires()
                }

            };
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            Input.Update();
            if (Main.keyState.IsKeyDown(Keys.P))
                InitLayers();
            _scene.Update();
            //_settingsWinow.Draw(spriteBatch);
        }

        public override void PostDrawScene()
        {
            _scene.Draw(_layers);
        }
    }
}