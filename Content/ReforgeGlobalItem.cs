using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CalamityReforge.Content;

public class ReforgeGlobalItem : GlobalItem
{
    private static int storedPrefix = -1;

    public override void OnCreated(Item item, ItemCreationContext context)
    {
        storedPrefix = -1;
    }

    public override void PreReforge(Item item)
    {
        storedPrefix = item.prefix;
    }

    public override int ChoosePrefix(Item item, UnifiedRandom rand)
    {
        if (!ModContent.GetInstance<Config.MainConfig>().Enabled || Main.gameMenu || storedPrefix == -1)
        {
            return -1;
        }
        return CalamityReforge.GetReforge(item, rand, storedPrefix);
    }

    public override void PostReforge(Item item)
    {
        storedPrefix = -1;
    }
}
