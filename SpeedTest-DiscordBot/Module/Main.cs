using DSharpPlus.CommandsNext.Attributes;

namespace SpeedTest_DiscordBot.Module
{
    public class Main : BaseCommandModule
    {
        [Command("speedtest"), Description("Host Network Tester.")]
        public async Task SpeedTest(CommandContext ctx)
        {
            var result = new Program().GetInternetInfo();
            if (result != null)
            {
                var output = new DiscordEmbedBuilder()
                    .WithTitle("SpeedTest results (view online)")
                    .WithUrl(result["Result URL"])
                    .AddField("Server: ", result["Server"].Replace("(id", ""))
                    .AddField("ISP: ", result["ISP"])
                    .AddField("⚡️ Latency: ", result["Latency"])
                    .AddField("⬇️ Download: ", result["Download"])
                    .AddField("⬆️ Upload: ", result["Upload"])
                    .AddField("〽️ Packet Loss: ", result["Packet Loss"])
                    .WithThumbnail("https://play-lh.googleusercontent.com/1LKWySZIE8Shx6Y1oy1LgrAtgDo-FwweOmW1hnfTVggPOM0txJ1qYtc8G8XOEfDpGA")
                    .WithFooter(ctx.Guild.Name + " / #" + ctx.Channel.Name + " / " + DateTime.Now)
                    .WithColor(DiscordColor.Black);
                await ctx.RespondAsync(embed: output.Build()).ConfigureAwait(false);
            }
        }
    }
}
