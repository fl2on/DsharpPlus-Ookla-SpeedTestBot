namespace SpeedTest_DiscordBot
{
    public partial class BotManager
    {
        internal static EventId TestBotEventId { get; } = new EventId(1000, "NYZX");
        public DiscordClient Discord { get; }
        private CommandsNextExtension CommandsNextService { get; }

        public BotManager()
        {
            DiscordConfiguration dcfg = new()
            {
                AutoReconnect = true,
                LargeThreshold = 250,
                MinimumLogLevel = LogLevel.Trace,
                Token = "Your Discord bot Token",
                TokenType = TokenType.Bot,
                MessageCacheSize = 2048,
                LogTimestampFormat = "dd-MM-yyyy HH:mm:ss zzz",
                Intents = DiscordIntents.All
            };
            Discord = new DiscordClient(dcfg);

            Discord.Ready += Discord_Ready;
            Discord.SocketErrored += Discord_SocketError;

            ServiceCollection depco = new();

            InteractivityConfiguration icfg = new()
            {
                Timeout = TimeSpan.FromSeconds(10),
                AckPaginationButtons = true,
                ResponseBehavior = InteractionResponseBehavior.Respond,
                PaginationBehaviour = PaginationBehaviour.Ignore,
                ResponseMessage = "Sorry, but this wasn't a valid option, or does not belong to you!",
                PaginationButtons = new PaginationButtons()
                {
                    Stop = new DiscordButtonComponent(ButtonStyle.Danger, "stop", null, false, new DiscordComponentEmoji(862259725785497620)),
                    Left = new DiscordButtonComponent(ButtonStyle.Secondary, "left", null, false, new DiscordComponentEmoji(862259522478800916)),
                    Right = new DiscordButtonComponent(ButtonStyle.Secondary, "right", null, false, new DiscordComponentEmoji(862259691212242974)),
                    SkipLeft = new DiscordButtonComponent(ButtonStyle.Primary, "skipl", null, false, new DiscordComponentEmoji(862259605464023060)),
                    SkipRight = new DiscordButtonComponent(ButtonStyle.Primary, "skipr", null, false, new DiscordComponentEmoji(862259654403031050))
                }
            };
            
            var cncfg = new CommandsNextConfiguration
            {
                StringPrefixes = Debugger.IsAttached ? new string[] { "+" } : new string[] { "?" },
                EnableDms = true,
                EnableMentionPrefix = true,
                CaseSensitive = false,
                Services = depco.BuildServiceProvider(true),
                IgnoreExtraArguments = false,
                EnableDefaultHelp = false,
                UseDefaultCommandHandler = true,
            };
            CommandsNextService = Discord.UseCommandsNext(cncfg);

            CommandsNextService.CommandErrored += CommandsNextService_CommandErrored;
            CommandsNextService.CommandExecuted += CommandsNextService_CommandExecuted;

            CommandsNextService.RegisterCommands(typeof(BotManager).GetTypeInfo().Assembly);
        }

        public async Task RunAsync()
        {
            DiscordActivity act = new("NYZX-SpeedTest", ActivityType.ListeningTo);
            await Discord.ConnectAsync(act, UserStatus.DoNotDisturb).ConfigureAwait(false);
        }

        public async Task StopAsync()
        {
            await Discord.DisconnectAsync().ConfigureAwait(false);
        }
    }
}
