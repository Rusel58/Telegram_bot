using Telegram.Bot.Exceptions;
using Telegram.Bot;
using Serilog;

namespace TelegramBotProcessing
{
    /// <summary>
    /// Provides utility methods for handling errors related to the Telegram bot.
    /// </summary>
    public static class Errors
    {
        /// <summary>
        /// Handles errors that occur during the Telegram bot's polling process.
        /// </summary>
        /// <param name="botClient">The Telegram bot client instance.</param>
        /// <param name="exception">The exception that occurred during polling.</param>
        /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
        /// <returns>A completed Task.</returns>
        public static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error: [{apiRequestException.ErrorCode}] {apiRequestException.Message}",
                _ => $"{exception.Message}."
            };
            Log.Fatal(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
