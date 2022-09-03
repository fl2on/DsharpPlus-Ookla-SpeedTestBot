namespace SpeedTest_DiscordBot
{
    public partial class BotManager
    {
        private Task Discord_SocketError(DiscordClient client, SocketErrorEventArgs e)
        {
            var ex = e.Exception is AggregateException ae ? ae.InnerException : e.Exception;
            client.Logger.LogError(TestBotEventId, ex, "WebSocket threw an exception");
            return Task.CompletedTask;
        }

        private Task Discord_Ready(DiscordClient client, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }

        private async Task CommandsNextService_CommandErrored(CommandsNextExtension cnext, CommandErrorEventArgs e)
        {
            if (e.Exception is CommandNotFoundException && (e.Command == null || e.Command.QualifiedName != "help"))
                return;

            e.Context.Client.Logger.LogError(TestBotEventId, e.Exception, "Exception occurred during {User}'s invocation of '{Command}'", e.Context.User.Username, e.Context.Command.QualifiedName);

            var exs = new List<Exception>();
            if (e.Exception is AggregateException ae)
                exs.AddRange(ae.InnerExceptions);
            else
                exs.Add(e.Exception);

            foreach (var ex in exs)
            {
                if (ex is CommandNotFoundException && (e.Command == null || e.Command.QualifiedName != "help"))
                    return;

                var embed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#FF0000"),
                    Title = "An exception occurred when executing a command",
                    Description = $"`{e.Exception.GetType()}` occurred when executing `{e.Command.QualifiedName}`.",
                    Timestamp = DateTime.UtcNow
                };
                embed.WithFooter(this.Discord.CurrentUser.Username, this.Discord.CurrentUser.AvatarUrl)
                    .AddField("Message", ex.Message);
                await e.Context.RespondAsync(embed: embed.Build()).ConfigureAwait(false);
            }
        }

        private Task CommandsNextService_CommandExecuted(CommandsNextExtension cnext, CommandExecutionEventArgs e)
        {
            e.Context.Client.Logger.LogInformation(TestBotEventId, "User {User} executed '{Command}' in {Channel}", e.Context.User.Username, e.Command.QualifiedName, e.Context.Channel.Name);
            return Task.CompletedTask;
        }
    }
}
