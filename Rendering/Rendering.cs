using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.GameContent.UI;
using Terraria.GameContent;
using ReLogic.Graphics;

namespace Terraria3D;

public static class Rendering
{
    public static bool RenderHalfBlockWaterHack { get; set; } = false;

    public static void PreRenderSetup()
    {
        DoLighting();
        UpdateState();
        CacheDraws();
    }

    public static void UpdateState()
    {
        if (Main.magmaBGFrameCounter >= 8)
        {
            Main.magmaBGFrameCounter = 0;
            Main.magmaBGFrame++;
            if (Main.magmaBGFrame >= 3)
            {
                Main.magmaBGFrame = 0;
            }
        }

        if (!Main.gamePaused)
        {
            Main.essScale += (float) Main.essDir * 0.01f;
            if (Main.essScale > 1f)
            {
                Main.essDir = -1;
                Main.essScale = 1f;
            }

            if ((double) Main.essScale < 0.7)
            {
                Main.essDir = 1;
                Main.essScale = 0.7f;
            }
        }
    }

    public static void DoLighting()
    {
        var firstTileX = (int) Math.Floor((double) (Main.screenPosition.X / 16f)) - 1;
        var lastTileX = (int) Math.Floor((double) ((Main.screenPosition.X + (float) Main.screenWidth) / 16f)) + 2;
        var firstTileY = (int) Math.Floor((double) (Main.screenPosition.Y / 16f)) - 1;
        var lastTileY = (int) Math.Floor((double) ((Main.screenPosition.Y + (float) Main.screenHeight) / 16f)) + 2;
        if (!Main.drawSkip)
        {
            Lighting.LightTiles(firstTileX, lastTileX, firstTileY, lastTileY);
        }
    }

    public static void DrawBackgroundWater()
    {
        if (Main.drawToScreen)
            Reflection.DrawWaters(true);
        else
        {
            Main.spriteBatch.Draw(Main.instance.backWaterTarget, Main.sceneBackgroundPos - Main.screenPosition,
                Microsoft.Xna.Framework.Color.White);
            TimeLogger.DetailedDrawTime(11);
        }
    }

    public static void DrawSceneBackground()
    {
        float x =
            (Main.sceneBackgroundPos.X - Main.screenPosition.X + (float) Main.offScreenRange) * Main.caveParallax -
            (float) Main.offScreenRange;
        if (Main.drawToScreen)
        {
            Main.tileBatch.Begin();
            Reflection.DrawBackground();
            Main.tileBatch.End();
        }
        else
            Main.spriteBatch.Draw(Main.instance.backgroundTarget,
                new Vector2(x, Main.sceneBackgroundPos.Y - Main.screenPosition.Y), Color.White);
    }

    //public static void DrawSandstorm() => Sandstorm.DrawGrains(Main.spriteBatch);

    public static void CacheDraws()
    {
        Reflection.CacheNPCDraws();
        Reflection.CacheProjDraws();
    }

    public static void DrawMoonMoon()
        => Reflection.DrawCachedNPCs(Main.instance.DrawCacheNPCsMoonMoon, true);

    public static void DrawFirstFractals()
        => Reflection.DrawCachedNPCs(Main.instance.DrawCacheFirstFractals, true);

    public static void DrawBlack()
    {
        if (Main.drawToScreen)
            Reflection.DrawBlack();
        else
            Main.spriteBatch.Draw(Main.instance.blackTarget, Main.sceneTilePos - Main.screenPosition, Color.White);
    }

    public static void DrawWalls()
    {
        if (Main.drawToScreen)
        {
            Main.tileBatch.Begin();
            Reflection.DrawWalls();
            Main.tileBatch.End();
        }
        else
            Main.spriteBatch.Draw(Main.instance.wallTarget, Main.sceneWallPos - Main.screenPosition, Color.White);
    }

    public static void DrawWallOfFlesh() => Reflection.DrawWoF();

    public static void DrawGoreBehind()
    {
        if (Main.drawBackGore)
            Reflection.DrawGoreBehind();
    }

    public static void DrawMoonLordDeath()
    {
        MoonlordDeathDrama.DrawPieces(Main.spriteBatch);
        MoonlordDeathDrama.DrawExplosions(Main.spriteBatch);
    }

    public static void DrawNPCsBehindNonSoldTiles()
        => Reflection.DrawCachedNPCs(Main.instance.DrawCacheNPCsBehindNonSolidTiles, true);

    public static void DrawWallTilesNPC()
        => Reflection.DrawWallsTilesNPCs();

    public static void DrawNonSolidTiles()
    {
        if (Main.drawToScreen)
        {
            Main.instance.TilesRenderer.PreDrawTiles(false, false, true);
            Reflection.DrawTiles(false);
        }
        else
            Main.spriteBatch.Draw(Main.instance.tile2Target, Main.sceneTile2Pos - Main.screenPosition, Color.White);
    }

    public static void DrawWaterFalls()
    {
        Main.instance.waterfallManager.Draw(Main.spriteBatch);
    }

    public static void DrawProjsBehindNPCsAndTiles() =>
        Reflection.DrawCachedProjs(Main.instance.DrawCacheProjsBehindNPCsAndTiles, false);

    public static void DrawNPCsBehindTiles() => Reflection.DrawNPCs(true);

    public static void DrawSolidTiles()
    {
        if (Main.drawToScreen)
        {
            Main.instance.TilesRenderer.PreDrawTiles(false, false, true);
            Reflection.DrawTiles(true);
        }
        else
            Main.spriteBatch.Draw(Main.instance.tileTarget, Main.sceneTilePos - Main.screenPosition, Color.White);
    }

    public static void DrawHitTileAnimation() =>
        Main.player[Main.myPlayer].hitTile.DrawFreshAnimations(Main.spriteBatch);

    public static void DrawTileEntities(bool solidLayer)
    {
    }

    public static void DrawNPCsInfrontOfTiles() => Reflection.DrawNPCs(false);

    public static void DrawProjsOverPlayers() => Reflection.DrawCachedProjs(Main.instance.DrawCacheProjsOverPlayers, false);

    public static void DrawNPCProjectiles() => Reflection.DrawCachedNPCs(Main.instance.DrawCacheNPCProjectiles, false);

    public static void SortDrawCacheWorm() => Reflection.SortDrawCashWorms();

    public static void DrawProjsBehindNPCs() =>
        Reflection.DrawCachedNPCs(Main.instance.DrawCacheProjsBehindNPCs, false);

    public static void DrawProjsBehindProjectiles() =>
        Reflection.DrawCachedProjs(Main.instance.DrawCacheProjsBehindProjectiles, false);

    public static void DrawProjectiles()
    {
        Main.spriteBatch.End();
        Reflection.DrawProjectiles();
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
            DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
    }

    public static void DrawPlayersBehindNPCs()
    {
        Main.spriteBatch.End();
        Reflection.DrawPlayersBehindNPCs();
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
            DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
    }

    public static void DrawPlayersAfterProjs()
    {
        Main.spriteBatch.End();
        Reflection.DrawPlayersAfterProj();
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
            DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
    }

    public static void DrawNPCsOverPlayer() => Reflection.DrawCachedNPCs(Main.instance.DrawCacheNPCsOverPlayers, false);

    public static void DrawItems() => Main.instance.DrawItems();

    public static void DrawRain() => Reflection.DrawRain();

    public static void DrawGore() => Reflection.DrawGore();

    public static void DrawDust()
    {
        Main.spriteBatch.End();
        Reflection.DrawDust();
        var sampler = Main.drawToScreen ? SamplerState.LinearClamp : SamplerState.PointClamp;
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, sampler, DepthStencilState.None,
            RasterizerState.CullCounterClockwise, null, Main.Transform);
    }

    public static void DrawForegroundWater()
    {
        if (Main.drawToScreen)
            Reflection.DrawWaters(false);
        else
            Main.spriteBatch.Draw(Main.waterTarget, Main.sceneWaterPos - Main.screenPosition, Color.White);
    }

    public static void DrawWires()
    {
        if (WiresUI.Settings.DrawWires)
            Reflection.DrawWires();
    }

    public static void DrawProjsOverWireUI() =>
        Reflection.DrawCachedProjs(Main.instance.DrawCacheProjsOverWiresUI, false);

    public static void DrawInfernoRings() => Main.instance.DrawInfernoRings();

    public static void DrawMoonlordDeathFront() => MoonlordDeathDrama.DrawWhite(Main.spriteBatch);

    public static void DrawScreenObstructions() => ScreenObstruction.Draw(Main.spriteBatch);

    public static void DrawChatOverPlayerHeads()
    {
        if (Main.hideUI) return;
        Reflection.DrawPlayerChatBubbles();
    }

    public static void DrawCombatText()
    {
        if (Main.hideUI) return;
        float targetScale = CombatText.TargetScale;
        var combatText = Main.combatText;
        var spriteBatch = Main.spriteBatch;
        for (int l = 0; l < 100; l++)
        {
            if (!combatText[l].active)
                continue;

            int num10 = 0;
            if (combatText[l].crit)
                num10 = 1;

            Vector2 vector2 = FontAssets.CombatText[num10].Value.MeasureString(combatText[l].text);
            Vector2 origin = new Vector2(vector2.X * 0.5f, vector2.Y * 0.5f);
            float num11 = combatText[l].scale / targetScale;
            float num12 = (int) combatText[l].color.R;
            float num13 = (int) combatText[l].color.G;
            float num14 = (int) combatText[l].color.B;
            float num15 = (int) combatText[l].color.A;
            num12 *= num11 * combatText[l].alpha * 0.3f;
            num14 *= num11 * combatText[l].alpha * 0.3f;
            num13 *= num11 * combatText[l].alpha * 0.3f;
            num15 *= num11 * combatText[l].alpha;
            Microsoft.Xna.Framework.Color color =
                new Microsoft.Xna.Framework.Color((int) num12, (int) num13, (int) num14, (int) num15);
            for (int m = 0; m < 5; m++)
            {
                float num16 = 0f;
                float num17 = 0f;
                switch (m)
                {
                    case 0:
                        num16 -= targetScale;
                        break;
                    case 1:
                        num16 += targetScale;
                        break;
                    case 2:
                        num17 -= targetScale;
                        break;
                    case 3:
                        num17 += targetScale;
                        break;
                    default:
                        num12 = (float) (int) combatText[l].color.R * num11 * combatText[l].alpha;
                        num14 = (float) (int) combatText[l].color.B * num11 * combatText[l].alpha;
                        num13 = (float) (int) combatText[l].color.G * num11 * combatText[l].alpha;
                        num15 = (float) (int) combatText[l].color.A * num11 * combatText[l].alpha;
                        color = new Microsoft.Xna.Framework.Color((int) num12, (int) num13, (int) num14, (int) num15);
                        break;
                }

                if (Main.player[Main.myPlayer].gravDir == -1f)
                {
                    float num18 = combatText[l].position.Y - Main.screenPosition.Y;
                    num18 = (float) Main.screenHeight - num18;
                    spriteBatch.DrawString(FontAssets.CombatText[num10].Value, combatText[l].text,
                        new Vector2(combatText[l].position.X - Main.screenPosition.X + num16 + origin.X,
                            num18 + num17 + origin.Y), color, combatText[l].rotation, origin, combatText[l].scale,
                        SpriteEffects.None, 0f);
                }
                else
                {
                    spriteBatch.DrawString(FontAssets.CombatText[num10].Value, combatText[l].text,
                        new Vector2(combatText[l].position.X - Main.screenPosition.X + num16 + origin.X,
                            combatText[l].position.Y - Main.screenPosition.Y + num17 + origin.Y), color,
                        combatText[l].rotation, origin, combatText[l].scale, SpriteEffects.None, 0f);
                }
            }
        }
    }

    public static void DrawItemText()
    {
        if (Main.hideUI) return;
        var scale = 1f;
        for (int num78 = 0; num78 < 20; num78++)
        {
            if (Main.popupText[num78].active)
            {
                string text = Main.popupText[num78].name;
                if (Main.popupText[num78].stack > 1)
                {
                    text = string.Concat(new object[]
                        {
                                        text,
                                        " (",
                                        Main.popupText[num78].stack,
                                        ")"
                        });
                }
                Vector2 vector7 = ((DynamicSpriteFont)FontAssets.MouseText).MeasureString(text);
                Vector2 origin2 = new Vector2(vector7.X * 0.5f, vector7.Y * 0.5f);
                float num79 = 1f;
                float num80 = (float)Main.popupText[num78].color.R;
                float num81 = (float)Main.popupText[num78].color.G;
                float num82 = (float)Main.popupText[num78].color.B;
                float num83 = (float)Main.popupText[num78].color.A;
                num80 *= num79 * Main.popupText[num78].alpha * 0.3f;
                num82 *= num79 * Main.popupText[num78].alpha * 0.3f;
                num81 *= num79 * Main.popupText[num78].alpha * 0.3f;
                num83 *= num79 * Main.popupText[num78].alpha;
                Microsoft.Xna.Framework.Color color10 = new Microsoft.Xna.Framework.Color((int)num80, (int)num81, (int)num82, (int)num83);
                for (int num84 = 0; num84 < 5; num84++)
                {
                    float num85 = 0f;
                    float num86 = 0f;
                    if (num84 == 0)
                    {
                        num85 -= scale * 2f;
                    }
                    else if (num84 == 1)
                    {
                        num85 += scale * 2f;
                    }
                    else if (num84 == 2)
                    {
                        num86 -= scale * 2f;
                    }
                    else if (num84 == 3)
                    {
                        num86 += scale * 2f;
                    }
                    else
                    {
                        num80 = (float)Main.popupText[num78].color.R * num79 * Main.popupText[num78].alpha;
                        num82 = (float)Main.popupText[num78].color.B * num79 * Main.popupText[num78].alpha;
                        num81 = (float)Main.popupText[num78].color.G * num79 * Main.popupText[num78].alpha;
                        num83 = (float)Main.popupText[num78].color.A * num79 * Main.popupText[num78].alpha;
                        color10 = new Microsoft.Xna.Framework.Color((int)num80, (int)num81, (int)num82, (int)num83);
                    }
                    if (num84 < 4)
                    {
                        num83 = (float)Main.popupText[num78].color.A * num79 * Main.popupText[num78].alpha;
                        color10 = new Microsoft.Xna.Framework.Color(0, 0, 0, (int)num83);
                    }
                    float num87 = Main.popupText[num78].position.Y - Main.screenPosition.Y + num86;
                    if (Main.player[Main.myPlayer].gravDir == -1f)
                    {
                        num87 = (float)Main.screenHeight - num87;
                    }
                    Main.spriteBatch.DrawString((DynamicSpriteFont)FontAssets.MouseText, text, new Vector2(Main.popupText[num78].position.X - Main.screenPosition.X + num85 + origin2.X, num87 + origin2.Y), color10, Main.popupText[num78].rotation, origin2, scale, SpriteEffects.None, 0f);
                }
            }
        }
    }
}