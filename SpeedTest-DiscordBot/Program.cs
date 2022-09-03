namespace SpeedTest_DiscordBot
{
    public  partial class Program
    {
        public static CancellationTokenSource CancelTokenSource { get; } = new CancellationTokenSource();
        private static CancellationToken CancelToken => CancelTokenSource.Token;

        public static void Main()
        {
            MainAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public static async Task MainAsync()
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            List<Task> tskl = new();
            BotManager bot = new();
            tskl.Add(bot.RunAsync());
            await Task.Delay(7500).ConfigureAwait(false);

            await Task.WhenAll(tskl).ConfigureAwait(false);

            try
            {
                await Task.Delay(-1, CancelToken).ConfigureAwait(false);
            }
            catch (Exception) { }
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            CancelTokenSource.Cancel();
        }
    }
}