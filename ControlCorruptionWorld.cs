//css_ref Terraria.dll

using System.IO;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace ControlCorruption
{
    public class GenerateClayLayerWorld : ModWorld
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalweight)
        {
            int finalCleanupIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
            int runs = 0;
            int worldChunkSize = Config.worldChunkSize;

            if (finalCleanupIndex != -1)
            {
                tasks.Insert(finalCleanupIndex + 1, new PassLegacy("Clay Clumps", delegate (GenerationProgress progress)
                {
                    progress.Message = "Generating Clay Clumps";
                    while (runs < Main.maxTilesX / worldChunkSize)
                    {
                        makeClayClumps(runs * worldChunkSize, (runs + 1) * worldChunkSize);
                        runs++;
                    }
                    //makeClayClumps(0, Main.maxTilesX / 2);
                    //makeClayClumps(Main.maxTilesX / 2, Main.maxTilesX);
                }));
            }
        }

        public override void ModifyHardmodeTasks(List<GenPass> list)
        {
            if (Config.dontGenerateHardmodeV)
            {
                int hardmodeVIndexCorruption = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Evil"));
                int hardmodeVIndexHallowed = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Good"));

                if (hardmodeVIndexCorruption != -1 && hardmodeVIndexHallowed != -1)
                {
                    list[hardmodeVIndexCorruption] = new PassLegacy("Doing Nothing", delegate { });
                    list[hardmodeVIndexHallowed] = new PassLegacy("Doing Nothing", delegate { });
                }
            }
            else
            {
                int hardmodeIndex = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Walls"));
                int runs = 0;
                int worldChunkSize = Config.worldChunkSize;

                if (hardmodeIndex != -1)
                {
                    list.Insert(hardmodeIndex + 1, new PassLegacy("Hardmode Clay Clumps", delegate
                    {
                        while (runs < Main.maxTilesX / worldChunkSize)
                        {
                            makeClayClumps(runs * worldChunkSize, (runs + 1) * worldChunkSize);
                            runs++;
                        }
                    }));
                }
            }

            int hardmodeAnnouncementIndex = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Announcment"));
            int numberOfAltarsToSmash = Config.timesToRunHardmodeOreGen;
            int runs2 = 0;
            int somethingIDKWhat = 0;
            int oreToSpawn = 0;
            double height = 0;
            float k = 0;
            int oreTier1 = Config.hardmodeOreTier1;
            int oreTier2 = Config.hardmodeOreTier2;
            int oreTier3 = Config.hardmodeOreTier3;

            if (hardmodeAnnouncementIndex != -1)
            {
                list.Insert(hardmodeAnnouncementIndex + 1, new PassLegacy("Hardmode Ores", delegate
                {
                    if (!Config.disableAutoHardmodeOreGen)
                    {
                        runs2 = 0;
                        while (runs2 < numberOfAltarsToSmash)
                        {
                            int num = runs2 % 3;
                            int num2 = runs2 / 3 + 1;
                            float num3 = (float)(Main.maxTilesX / 4200);
                            int num4 = 1 - num;
                            num3 = num3 * 310f - (float)(85 * num);
                            num3 *= 0.85f;
                            num3 /= (float)num2;
                            switch (num)
                            {
                                case 0:
                                    {
                                        if (oreTier1 == 221)
                                        {
                                            num3 *= 0.9f;
                                        }
                                        num = oreTier1;
                                        num3 *= 1.05f;
                                        break;
                                    }
                                case 1:
                                    {
                                        if (oreTier2 == 222)
                                        {
                                            num3 *= 0.9f;
                                        }
                                        num = oreTier2;
                                        break;
                                    }
                                default:
                                    {
                                        if (oreTier3 == 223)
                                        {
                                            num3 *= 0.9f;
                                        }
                                        num = oreTier3;
                                        break;
                                    }
                            }
                            for (int whatever = 0; (float)whatever < num3; whatever++)
                            {
                                int i2 = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
                                double num8 = Main.worldSurface;

                                if (num == 108 || num == 222)
                                {
                                    num8 = Main.rockLayer;
                                }
                                if (num == 111 || num == 223)
                                {
                                    num8 = (Main.rockLayer + Main.rockLayer + (double)Main.maxTilesY) / 3.0;
                                }

                                int j2 = WorldGen.genRand.Next((int)num8, Main.maxTilesY - 150);
                                WorldGen.OreRunner(i2, j2, (double)WorldGen.genRand.Next(5, 9 + num4), WorldGen.genRand.Next(5, 9 + num4), (ushort)num);
                            }
                            runs2++;
                        }
                        Main.NewText("Your world has been blessed with hardmode ores equivalent to " + Config.timesToRunHardmodeOreGen + " altars smashed!", 200, 200, 55);
                    }
                    else
                    {
                    }
                }));
            }
        }

        public static bool checkTile(Tile tileToCheck)
        {
            if (tileToCheck.active() && (tileToCheck.type == TileID.CorruptGrass || tileToCheck.type == TileID.CorruptIce || tileToCheck.type == TileID.CorruptHardenedSand || tileToCheck.type == TileID.CorruptSandstone || tileToCheck.type == TileID.Ebonstone || tileToCheck.type == TileID.Ebonsand || tileToCheck.type == TileID.EbonstoneBrick
                || tileToCheck.type == TileID.Crimstone || tileToCheck.type == TileID.Crimsand || tileToCheck.type == TileID.CrimtaneBrick || tileToCheck.type == TileID.CrimsonHardenedSand || tileToCheck.type == TileID.CrimsonSandstone || tileToCheck.type == TileID.FleshBlock || tileToCheck.type == TileID.FleshGrass || tileToCheck.type == TileID.FleshIce
                || tileToCheck.type == TileID.HallowedGrass || tileToCheck.type == TileID.HallowedIce || tileToCheck.type == TileID.HallowHardenedSand || tileToCheck.type == TileID.HallowSandstone || tileToCheck.type == TileID.Pearlsand || tileToCheck.type == TileID.Pearlstone || tileToCheck.type == TileID.PearlstoneBrick))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static  void insertClayClump(int x, int y, int size)
        {
            int sizeCounter = 0;
            int sizeCounter2 = 0;

            while (sizeCounter < size)
            {
                sizeCounter2 = 0;
                while (sizeCounter2 < size)
                {
                    Main.tile[x + sizeCounter - size, y + sizeCounter2 - size].type = TileID.ClayBlock;
                    sizeCounter2++;
                }

                sizeCounter++;
            }
        }

        public static void makeClayClumps(int Point1, int Point2)
        {
            int worldScanX = Point1;
            int worldScanY = 0;
            int size = Config.claySize;
            int k = 0;

            List<int> listX = new List<int>();
            List<int> listY = new List<int>();

            while (worldScanX < Point2)
            {
                worldScanY = 0;
                while (worldScanY < Main.maxTilesY)
                {
                    Tile tileType = Framing.GetTileSafely(worldScanX, worldScanY);

                    if (checkTile(tileType))
                    {
                        listX.Add(worldScanX);
                        listY.Add(worldScanY);
                    }
                    worldScanY++;
                }
                worldScanX++;
            }

            //rectangular clay block structure surrounding corruption/crimson structure

            int finalXLeft = 0;
            int finalXRight = 0;
            int finalYBottom = 0;
            int finalYTop = 0;

            if (listX.Count > 0)
            {
                finalXLeft = listX[k];
                finalXRight = listX[k];
                finalYBottom = listY[k];
                finalYTop = listY[k];

                while (k < listX.Count)
                {
                    if (finalXLeft > listX[k])
                    {
                        finalXLeft = listX[k];
                    }
                    if (finalXRight < listX[k])
                    {
                        finalXRight = listX[k];
                    }
                    if (finalYBottom < listY[k])
                    {
                        finalYBottom = listY[k];
                    }
                    if (finalYTop > listY[k])
                    {
                        finalYTop = listY[k];
                    }
                    k++;
                }
            }

            int writtenX = finalXLeft;
            int writtenY = finalYBottom;

            if (listX.Count > 0)
            {
                writtenY = finalYBottom;
                while (writtenY > finalYTop)
                {
                    insertClayClump(finalXLeft, writtenY, size);
                    //WorldGen.TileRunner(finalXLeft, writtenY, size, 1, TileID.ClayBlock, false, 0f, 0f, true, true);
                    writtenY--;
                }
                writtenX = finalXLeft;
                while (writtenX < finalXRight)
                {
                    insertClayClump(writtenX, finalYBottom, size);
                    //WorldGen.TileRunner(writtenX, finalYBottom, size, 1, TileID.ClayBlock, false, 0f, 0f, true, true);
                    writtenX++;
                }
                writtenY = finalYBottom;
                while (writtenY > finalYTop)
                {
                    insertClayClump(finalXRight, writtenY, size);
                    //WorldGen.TileRunner(finalXRight, writtenY, size, 1, TileID.ClayBlock, false, 0f, 0f, true, true);
                    writtenY--;
                }
                writtenX = finalXLeft;
                while (writtenX < finalXRight)
                {
                    //if (finalYTop > WorldGen.rockLayer)
                    //{
                    insertClayClump(writtenX, finalYTop, size);
                    //}
                    writtenX++;
                }
            }
        }
    }
}