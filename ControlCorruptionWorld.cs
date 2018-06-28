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
    public class ControlCorrution : ModWorld
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalweight)
        {
            int finalCleanupIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));

            if (finalCleanupIndex != -1)
            {
                tasks.Insert(finalCleanupIndex + 1, new PassLegacy("Clay Clumps", delegate (GenerationProgress progress)
                {
                    progress.Message = "Generating Clay Clumps";
                    makeClayClumps(0, Main.maxTilesX / 2);
                    makeClayClumps(Main.maxTilesX / 2, Main.maxTilesX);

                }));
            }
        }

        private void insertClayClump(int x, int y, int size)
        {
            int sizeCounter = 0;
            int sizeCounter2 = 0;

            while (sizeCounter < size)
            {
                sizeCounter2 = 0;
                while (sizeCounter2 < size)
                {
                    Main.tile[x + sizeCounter, y + sizeCounter2].type = TileID.ClayBlock;
                    sizeCounter2++;
                }

                sizeCounter++;
            }
        }

        private void makeClayClumps(int Point1, int Point2)
        {
            int worldScanX = Point1;
            int worldScanY = 0;
            int size = 10;
            int i = 0;
            int j = 0;
            int k = 0;

            List<int> listX = new List<int>();
            List<int> listY = new List<int>();

            while (worldScanX < Point2)
            {
                worldScanY = 0;
                while (worldScanY < Main.maxTilesY)
                {
                    Tile tileType = Framing.GetTileSafely(worldScanX, worldScanY);

                    if (tileType.active() && (tileType.type == TileID.CorruptGrass || tileType.type == TileID.CorruptIce || tileType.type == TileID.CorruptHardenedSand || tileType.type == TileID.CorruptSandstone || tileType.type == TileID.Ebonstone || tileType.type == TileID.Ebonsand || tileType.type == TileID.EbonstoneBrick))
                    {
                        listX.Add(worldScanX);
                        listY.Add(worldScanY);
                    }
                    worldScanY++;
                }
                worldScanX++;
            }

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
                while (writtenY > WorldGen.worldSurfaceLow)
                {
                    insertClayClump(finalXLeft, writtenY, size);
                    //WorldGen.TileRunner(finalXLeft, writtenY, size, 1, TileID.ClayBlock, false, 0f, 0f, true, true);
                    writtenY--;
                }
                while (writtenX < finalXRight)
                {
                    insertClayClump(writtenX, finalYBottom, size);
                    //WorldGen.TileRunner(writtenX, finalYBottom, size, 1, TileID.ClayBlock, false, 0f, 0f, true, true);
                    writtenX++;
                }
                writtenY = finalYBottom;
                while (writtenY > WorldGen.worldSurfaceLow)
                {
                    insertClayClump(finalXRight, writtenY, size);
                    //WorldGen.TileRunner(finalXRight, writtenY, size, 1, TileID.ClayBlock, false, 0f, 0f, true, true);
                    writtenY--;
                }
            }
        }
    }
}