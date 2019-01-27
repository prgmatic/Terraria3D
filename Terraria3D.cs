using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoMod.RuntimeDetour.HookGen;
using Terraria;
using Terraria.ModLoader;
using Mono.Cecil;
using Terraria.UI;
using System.Diagnostics;
using System.Text;
using System.IO;
using System;
using Mono.Cecil.Cil;

namespace Terraria3D
{
    class Terraria3D : Mod
    {
        public static Terraria3D Instance { get; private set; }

        private UISettingsWindow _settingsWinow = new UISettingsWindow("3D Settings");
        private Scene3D _scene = new Scene3D();
        private Layer3D[] _layers;

        public override void Load()
        {
            Instance = this;
            InitLayers();
            Hooks.Initialize();
        }

        private void InitLayers()
        {
            if (_layers != null)
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
                    }
                },
                new Layer3D()
                {
                    RenderFunction = () =>
                    {
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

        public void RenderLayersTargets()
        {
            Rendering.CacheDraws();
            if (Main.gameMenu) return;
            foreach (var layer in _layers)
                layer.RenderToTarget();
        }

        public void DrawScene()
        {
            for (int i = 0; i < 20; i++)
            {
                _scene.Draw(_layers);

            }
        }
    }
}