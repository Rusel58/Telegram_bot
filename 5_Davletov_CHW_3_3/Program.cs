using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotProcessing;
using Serilog;
using Serilog.Events;

namespace _5_Davletov_CHW_3_3
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var botClient = new TelegramBotClient("7062040554:AAELdeuCiK9u0dOJPz0i0ZnxdVwMCN6S938");

            // Path of logger file.
            string path = string.Join(Path.DirectorySeparatorChar, Directory.GetCurrentDirectory().Split(Path.DirectorySeparatorChar)[..^4]) +
                Path.DirectorySeparatorChar + "var" + Path.DirectorySeparatorChar + "logging.txt";

            // The announcement of the logger and its configuration.
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .WriteTo.File(path, rollingInterval: RollingInterval.Infinite).CreateLogger();

            using CancellationTokenSource cts = new();

            var me = await botClient.GetMeAsync();

            Log.Information($"Start listening for @{me.Username}.");

            Console.WriteLine("Для того, чтобы завершить программу нажмите Enter.");

            // StartReceiving does not block the caller thread.
            // Receiving is done on the ThreadPool.
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // Receive all update types except ChatMember related updates.
            };

            while (true)
            {
                try
                {
                    botClient.StartReceiving(
                        updateHandler: Updates.HandleUpdateAsync,
                        pollingErrorHandler: Errors.HandlePollingErrorAsync,
                        receiverOptions: receiverOptions,
                        cancellationToken: cts.Token
                    );
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Log.Fatal($"An error occurred while bot activation: {ex.Message}.");
                    continue;
                }
                break;
            }

            // Send messages to all users that bot is stopped.
            foreach (var update in UserProcessing.userData.Keys)
            {
                await botClient.SendTextMessageAsync(
                    chatId: update,
                    text: "Бот приостановлен!\nПри его активации напишите что-либо!",
                    replyMarkup: new ReplyKeyboardRemove());
            }

            // Send cancellation request to stop bot
            cts.Cancel();
            Log.Information("Bot is stopped!");
        }
    }
}