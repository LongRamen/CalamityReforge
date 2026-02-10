using CalamityReforge.Config;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Prefixes;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CalamityReforge.Content;

// This mod adapts code from the Calamity mod.
// Original code taken from the official public repository at https://github.com/CalamityTeam/CalamityModPublic/tree/1.4.4-release
//
// As the license in that repository states:
// " - You may use the source code of the Calamity Mod as a reference for building or developing Terraria mods or other software.
//   - Code lifted from the Calamity Mod repository must be credited to the Calamity Team."
//
// Credit for the logic used in this alternative reforging system goes to the Calamity Team.

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
        if (Main.gameMenu || !ServerConfig.Instance.Enabled)
        {
            return -1;
        }

        if (item.accessory && ServerConfig.Instance.SimplifyAccessoryReforge)
        {
            return GetSimplifiedAccessoryReforge(rand, storedPrefix);
        }
        else if (storedPrefix != -1)
        {
            return GetReforge(item, rand, storedPrefix);
        }

        return -1;
    }

    public override void PostReforge(Item item)
    {
        storedPrefix = -1;
    }

    #region Prefix Definition
    public static readonly int[] SimplifiedAccessoryPrefixes =
    [
        PrefixID.Warding,
        PrefixID.Menacing,
        PrefixID.Lucky,
        PrefixID.Quick2,
        PrefixID.Violent,
        PrefixID.Arcane,
    ];

    public static readonly int[][] AccessoryPrefixTiers =
    [
        [PrefixID.Hard, PrefixID.Jagged, PrefixID.Brisk, PrefixID.Wild],
        [PrefixID.Guarding, PrefixID.Spiked, PrefixID.Precise, PrefixID.Fleeting, PrefixID.Rash],
        [PrefixID.Armored, PrefixID.Angry, PrefixID.Hasty2, PrefixID.Intrepid, PrefixID.Arcane],
        [PrefixID.Warding, PrefixID.Menacing, PrefixID.Lucky, PrefixID.Quick2, PrefixID.Violent],
    ];

    public static readonly int[][] TerrarianPrefixTiers =
    [
        [PrefixID.Keen, PrefixID.Forceful, PrefixID.Strong],
        [PrefixID.Hurtful, PrefixID.Ruthless, PrefixID.Zealous],
        [PrefixID.Superior, PrefixID.Demonic, PrefixID.Godly],
        [PrefixID.Legendary2],
    ];

    public static readonly int[][] MeleePrefixTiers =
    [
        [PrefixID.Keen, PrefixID.Nimble, PrefixID.Nasty, PrefixID.Light, PrefixID.Heavy, PrefixID.Light, PrefixID.Forceful, PrefixID.Strong],
        [PrefixID.Hurtful, PrefixID.Ruthless, PrefixID.Zealous, PrefixID.Quick, PrefixID.Pointy, PrefixID.Bulky],
        [PrefixID.Murderous, PrefixID.Agile, PrefixID.Large, PrefixID.Dangerous, PrefixID.Sharp],
        [PrefixID.Massive, PrefixID.Unpleasant, PrefixID.Savage, PrefixID.Superior],
        [PrefixID.Demonic, PrefixID.Deadly2, PrefixID.Godly],
        [PrefixID.Legendary],
    ];

    public static readonly int[][] MeleeNoSpeedPrefixTiers =
    [
        [PrefixID.Keen, PrefixID.Forceful, PrefixID.Strong],
        [PrefixID.Hurtful, PrefixID.Ruthless, PrefixID.Zealous],
        [PrefixID.Superior, PrefixID.Demonic],
        [PrefixID.Godly],
    ];

    public static readonly int[][] MeleeNoSpeedAlwaysCritPrefixTiers =
    [
        [PrefixID.Keen, PrefixID.Forceful, PrefixID.Strong],
        [PrefixID.Hurtful, PrefixID.Ruthless, PrefixID.Zealous],
        [PrefixID.Superior, PrefixID.Demonic],
        [PrefixID.Godly, PrefixID.Ruthless],
    ];

    public static readonly int[][] ToolPrefixTiers =
    [
        [PrefixID.Keen, PrefixID.Nimble, PrefixID.Nasty, PrefixID.Heavy, PrefixID.Forceful, PrefixID.Strong],
        [PrefixID.Hurtful, PrefixID.Ruthless, PrefixID.Zealous, PrefixID.Quick, PrefixID.Pointy, PrefixID.Bulky],
        [PrefixID.Murderous, PrefixID.Agile, PrefixID.Large, PrefixID.Dangerous, PrefixID.Sharp],
        [PrefixID.Massive, PrefixID.Unpleasant, PrefixID.Savage, PrefixID.Superior],
        [PrefixID.Demonic, PrefixID.Deadly2, PrefixID.Godly],
        [PrefixID.Legendary, PrefixID.Light],
    ];

    public static readonly int[][] RangedPrefixTiers =
    [
        [PrefixID.Keen, PrefixID.Nimble, PrefixID.Nasty, PrefixID.Powerful, PrefixID.Forceful, PrefixID.Strong],
        [PrefixID.Hurtful, PrefixID.Ruthless, PrefixID.Zealous, PrefixID.Quick, PrefixID.Intimidating],
        [PrefixID.Murderous, PrefixID.Agile, PrefixID.Hasty, PrefixID.Staunch, PrefixID.Unpleasant],
        [PrefixID.Superior, PrefixID.Demonic, PrefixID.Sighted],
        [PrefixID.Godly, PrefixID.Rapid, PrefixID.Deadly, PrefixID.Deadly2],
        [PrefixID.Unreal],
    ];

    public static readonly int[][] MagicPrefixTiers =
    [
        [PrefixID.Keen, PrefixID.Nimble, PrefixID.Nasty, PrefixID.Furious, PrefixID.Forceful, PrefixID.Strong],
        [PrefixID.Hurtful, PrefixID.Ruthless, PrefixID.Zealous, PrefixID.Quick, PrefixID.Taboo, PrefixID.Manic],
        [PrefixID.Murderous, PrefixID.Agile, PrefixID.Adept, PrefixID.Celestial, PrefixID.Unpleasant],
        [PrefixID.Superior, PrefixID.Demonic, PrefixID.Mystic],
        [PrefixID.Godly, PrefixID.Masterful, PrefixID.Deadly2],
        [PrefixID.Mythical],
    ];

    public static readonly int[][] SummonPrefixTiers =
    [
        [PrefixID.Nimble, PrefixID.Furious],
        [PrefixID.Forceful, PrefixID.Strong, PrefixID.Quick, PrefixID.Taboo, PrefixID.Manic],
        [PrefixID.Hurtful, PrefixID.Adept, PrefixID.Celestial],
        [PrefixID.Superior, PrefixID.Demonic, PrefixID.Mystic, PrefixID.Deadly2],
        [PrefixID.Masterful, PrefixID.Godly],
        [PrefixID.Mythical, PrefixID.Ruthless],
    ];
    #endregion

    public static int GetPrefixTier(int[][] tiers, int currentPrefix)
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

    public static int IteratePrefix(UnifiedRandom rand, int[][] reforgeTiers, int currentPrefix)
    {
        int currentTier = GetPrefixTier(reforgeTiers, currentPrefix);
        int newTier = (currentTier == reforgeTiers.Length - 1) ? currentTier : (currentTier + 1);
        return rand.Next(reforgeTiers[newTier]);
    }

    public static int GetReforge(Item item, UnifiedRandom rand, int currentPrefix)
    {
        int prefix = -1;

        // Accessories
        if (item.accessory)
        {
            for (int accRerolls = 0; accRerolls < 20; accRerolls++)
            {
                int newPrefix = IteratePrefix(rand, AccessoryPrefixTiers, currentPrefix);
                if (newPrefix != currentPrefix)
                {
                    prefix = newPrefix;
                    break;
                }
            }
        }

        // Melee (includes tools and whips)
        else if (item.CountsAsClass<MeleeDamageClass>() || item.CountsAsClass<SummonMeleeSpeedDamageClass>())
        {
            // Terrarian / other items that can use Legendary2
            if (PrefixLegacy.ItemSets.ItemsThatCanHaveLegendary2[item.type])
            {
                prefix = IteratePrefix(rand, TerrarianPrefixTiers, currentPrefix);
            }

            // Swords, Whips, Tools, other items that support the Legendary modifier
            else if (PrefixLegacy.ItemSets.SwordsHammersAxesPicks[item.type] || (item.ModItem != null && item.ModItem.MeleePrefix()))
            {
                int[][] tierListToUse = ((item.pick > 0 || item.axe > 0 || item.hammer > 0) ? ToolPrefixTiers : MeleePrefixTiers);
                prefix = IteratePrefix(rand, tierListToUse, currentPrefix);
            }

            // Yoyos, Flails, Spears, etc.
            else
            {
                bool has100Crit = Main.LocalPlayer.GetTotalCritChance(item.DamageType) >= 100;
                prefix = IteratePrefix(rand, has100Crit ? MeleeNoSpeedAlwaysCritPrefixTiers : MeleeNoSpeedPrefixTiers, currentPrefix);
            }
        }

        // Ranged
        else if (item.CountsAsClass<RangedDamageClass>())
        {
            prefix = IteratePrefix(rand, RangedPrefixTiers, currentPrefix);
        }

        // Magic
        else if (item.CountsAsClass<MagicDamageClass>() || item.CountsAsClass<MagicSummonHybridDamageClass>())
        {
            prefix = IteratePrefix(rand, MagicPrefixTiers, currentPrefix);
        }

        // Summon (not whips)
        else if (item.CountsAsClass<SummonDamageClass>())
        {
            prefix = IteratePrefix(rand, SummonPrefixTiers, currentPrefix);
        }

        return prefix;
    }

    public static int GetSimplifiedAccessoryReforge(UnifiedRandom rand, int currentPrefix)
    {
        var pool = SimplifiedAccessoryPrefixes.Where(id => id != currentPrefix);
        return pool.ElementAt(rand.Next(0, pool.Count()));
    }
}
