using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace CalamityReforge.Config;

public class ServerConfig : ModConfig
{
    public static ServerConfig Instance => ModContent.GetInstance<ServerConfig>();
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [DefaultValue(true)]
    public bool Enabled;

    [DefaultValue(false)]
    public bool SimplifyAccessoryReforge;
}
