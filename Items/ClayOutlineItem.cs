using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ControlCorruption.Items
{
    public class ClayOutlineItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Container");
            Tooltip.SetDefault("Surrounds any Crimson / Corruption / Hallowed Blocks \n with a layer of clay " + Config.claySize + " blocks thick");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 999;
            item.value = 100;
            item.rare = 1;
            item.useStyle = 4;
            item.useTurn = true;
            item.autoReuse = false;
            item.consumable = true;
            item.UseSound = SoundID.Item4;
            item.useTime = 60;
            item.useAnimation = 60;
            item.noMelee = true;
            // Set other item.X values here
        }

        public override bool UseItem(Player player)
        {
            int runs = 0;
            int worldChunkSize = Config.worldChunkSize;
            while (runs < Main.maxTilesX / worldChunkSize)
            {
                GenerateClayLayerWorld.makeClayClumps(runs * worldChunkSize, (runs + 1) * worldChunkSize);
                runs++;
            }
            Main.NewText("Your Crimson / Corruption / Hallowed has been surrounded by a layer of clay " + Config.claySize + " blocks thick!", 200, 200, 55);
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ClayBlock, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();// Recipes here. See Basic Recipe Guide
        }
    }
}