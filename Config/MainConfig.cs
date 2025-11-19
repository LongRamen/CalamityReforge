using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace CalamityReforge.Config;

public class MainConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [DefaultValue(true)]
    public bool Enabled;
}
