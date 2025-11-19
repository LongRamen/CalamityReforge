using Terraria;
using Terraria.GameContent.Prefixes;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CalamityReforge;

public class CalamityReforge : Mod
{
    private static int GetPrefixTier(int[][] tiers, int currentPrefix)
    {
        for (int checkingTier = 0; checkingTier < tiers.Length; checkingTier++)
        {
            int[] tierList = tiers[checkingTier];
            for (int i = 0; i < tierList.Length; i++)
            {
                if (tierList[i] == currentPrefix)
                {
                    return checkingTier;
                }
            }
        }
        return -1;
    }

    private static int IteratePrefix(UnifiedRandom rand, int[][] reforgeTiers, int currentPrefix)
    {
        int currentTier = GetPrefixTier(reforgeTiers, currentPrefix);
        int newTier = ((currentTier == reforgeTiers.Length - 1) ? currentTier : (currentTier + 1));
        return rand.Next(reforgeTiers[newTier]);
    }

    public static int GetReforge(Item item, UnifiedRandom rand, int currentPrefix)
    {
        int prefix = -1;

        if (item.accessory)
        {
            int[][] accessoryReforgeTiers =
            [
                [PrefixID.Hard, PrefixID.Jagged, PrefixID.Brisk, PrefixID.Wild],
                [PrefixID.Guarding, PrefixID.Spiked, PrefixID.Precise, PrefixID.Fleeting, PrefixID.Rash],
                [PrefixID.Armored, PrefixID.Angry, PrefixID.Hasty2, PrefixID.Intrepid, PrefixID.Arcane],
                [PrefixID.Warding, PrefixID.Menacing, PrefixID.Lucky, PrefixID.Quick2, PrefixID.Violent]
            ];

            for (int accRerolls = 0; accRerolls < 20; accRerolls++)
            {
                int newPrefix = IteratePrefix(rand, accessoryReforgeTiers, currentPrefix);
                if (newPrefix != currentPrefix)
                {
                    prefix = newPrefix;
                    break;
                }
            }
        }
        else if (item.CountsAsClass<MeleeDamageClass>() || item.CountsAsClass<SummonMeleeSpeedDamageClass>())
        {
            if (PrefixLegacy.ItemSets.ItemsThatCanHaveLegendary2[item.type])
            {
                int[][] terrarianReforgeTiers =
                [
                    [PrefixID.Keen, PrefixID.Forceful, PrefixID.Strong],
                    [PrefixID.Hurtful, PrefixID.Ruthless, PrefixID.Zealous],
                    [PrefixID.Superior, PrefixID.Demonic, PrefixID.Godly],
                    [PrefixID.Legendary2]
                ];
                prefix = IteratePrefix(rand, terrarianReforgeTiers, currentPrefix);
            }
            else if (PrefixLegacy.ItemSets.SwordsHammersAxesPicks[item.type] || (item.ModItem != null && item.ModItem.MeleePrefix()))
            {
                int[][] meleeReforgeTiers =
                [
                    [PrefixID.Keen, PrefixID.Nimble, PrefixID.Nasty, PrefixID.Light, PrefixID.Heavy, PrefixID.Light, PrefixID.Forceful, PrefixID.Strong],
                    [PrefixID.Hurtful, PrefixID.Ruthless, PrefixID.Zealous, PrefixID.Quick, PrefixID.Pointy, PrefixID.Bulky],
                    [PrefixID.Murderous, PrefixID.Agile, PrefixID.Large, PrefixID.Dangerous, PrefixID.Sharp],
                    [PrefixID.Massive, PrefixID.Unpleasant, PrefixID.Savage, PrefixID.Superior],
                    [PrefixID.Demonic, PrefixID.Deadly2, PrefixID.Godly],
                    [PrefixID.Legendary]
                ];
                int[][] toolReforgeTiers =
                [
                    [PrefixID.Keen, PrefixID.Nimble, PrefixID.Nasty, PrefixID.Heavy, PrefixID.Forceful, PrefixID.Strong],
                    [PrefixID.Hurtful, PrefixID.Ruthless, PrefixID.Zealous, PrefixID.Quick, PrefixID.Pointy, PrefixID.Bulky],
                    [PrefixID.Murderous, PrefixID.Agile, PrefixID.Large, PrefixID.Dangerous, PrefixID.Sharp],
                    [PrefixID.Massive, PrefixID.Unpleasant, PrefixID.Savage, PrefixID.Superior],
                    [PrefixID.Demonic, PrefixID.Deadly2, PrefixID.Godly],
                    [PrefixID.Legendary, PrefixID.Light]
                ];
                int[][] tierListToUse = ((item.pick > 0 || item.axe > 0 || item.hammer > 0) ? toolReforgeTiers : meleeReforgeTiers);
                prefix = IteratePrefix(rand, tierListToUse, currentPrefix);
            }
            else
            {
                int[][] meleeNoSpeedReforgeTiers =
                [
                    [PrefixID.Keen, PrefixID.Forceful, PrefixID.Strong],
                    [PrefixID.Hurtful, PrefixID.Ruthless, PrefixID.Zealous],
                    [PrefixID.Superior, PrefixID.Demonic],
                    [PrefixID.Godly]
                ];
                prefix = IteratePrefix(rand, meleeNoSpeedReforgeTiers, currentPrefix);
            }
        }
        else if (item.CountsAsClass<RangedDamageClass>())
        {
            int[][] rangedReforgeTiers =
            [
                [PrefixID.Keen, PrefixID.Nimble, PrefixID.Nasty, PrefixID.Powerful, PrefixID.Forceful, PrefixID.Strong],
                [PrefixID.Hurtful, PrefixID.Ruthless, PrefixID.Zealous, PrefixID.Quick, PrefixID.Intimidating],
                [PrefixID.Murderous, PrefixID.Agile, PrefixID.Hasty, PrefixID.Staunch, PrefixID.Unpleasant],
                [PrefixID.Superior, PrefixID.Demonic, PrefixID.Sighted],
                [PrefixID.Godly, PrefixID.Rapid, PrefixID.Deadly, PrefixID.Deadly2],
                [PrefixID.Unreal]
            ];
            prefix = IteratePrefix(rand, rangedReforgeTiers, currentPrefix);
        }
        else if (item.CountsAsClass<MagicDamageClass>() || item.CountsAsClass<MagicSummonHybridDamageClass>())
        {
            int[][] magicReforgeTiers =
            [
                [PrefixID.Keen, PrefixID.Nimble, PrefixID.Nasty, PrefixID.Furious, PrefixID.Forceful, PrefixID.Strong],
                [PrefixID.Hurtful, PrefixID.Ruthless, PrefixID.Zealous, PrefixID.Quick, PrefixID.Taboo, PrefixID.Manic],
                [PrefixID.Murderous, PrefixID.Agile, PrefixID.Adept, PrefixID.Celestial, PrefixID.Unpleasant],
                [PrefixID.Superior, PrefixID.Demonic, PrefixID.Mystic],
                [PrefixID.Godly, PrefixID.Masterful, PrefixID.Deadly2],
                [PrefixID.Mythical]
            ];
            prefix = IteratePrefix(rand, magicReforgeTiers, currentPrefix);
        }
        else if (item.CountsAsClass<SummonDamageClass>())
        {
            int[][] summonReforgeTiers =
            [
                [PrefixID.Nimble, PrefixID.Furious],
                [PrefixID.Forceful, PrefixID.Strong, PrefixID.Quick, PrefixID.Taboo, PrefixID.Manic],
                [PrefixID.Hurtful, PrefixID.Adept, PrefixID.Celestial],
                [PrefixID.Superior, PrefixID.Demonic, PrefixID.Mystic, PrefixID.Deadly2],
                [PrefixID.Masterful, PrefixID.Godly],
                [PrefixID.Mythical, PrefixID.Ruthless]
            ];
            prefix = IteratePrefix(rand, summonReforgeTiers, currentPrefix);
        }

        return prefix;
    }
}