using CSVFormat;
using CultObjectProcessing;
using JSONFormat;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotProcessing
{
    /// <summary>
    /// Provides methods for outputting data and sending files to users through the Telegram bot.
    /// </summary>
    public static class Output
    {
        /// <summary>
        /// The default welcome message for the Telegram bot.
        /// </summary>
        public static string start = "Добро пожаловать в бота для работы с данными, содержащими культурные объекты!";

        public async static void Print(ITelegramBotClient botClient, long chatId,
            string message)
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: message);
        }

        /// <summary>
        /// Sends a text message to the specified chat.
        /// </summary>
        /// <param name="botClient">The Telegram bot client instance.</param>
        /// <param name="chatId">The ID of the chat where the message will be sent.</param>
        /// <param name="message">The text message to be sent.</param>
        public static async Task SendFile(ITelegramBotClient botClient, long chatId, List<CultObject>? objects, string type)
        {
            if (objects == null)
            {
                await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Данных нет!");
                await MenuKeyBoards.GetMenu(botClient, chatId);
                return;
            }
            Stream outputStream;
            if (type == "CSV")
            {
                outputStream = CSVProcessing.Write(objects);
            }
            else
            {
                outputStream = JSONProcessing.Write(objects);
            }
            await botClient.SendDocumentAsync(
                       chatId: chatId,
                       document: InputFile.FromStream(stream: outputStream,
                       fileName: UserProcessing.userData[chatId].FileName + UserProcessing.userData[chatId].FileType?.ToString().ToLower()));
            UserProcessing.userData[chatId].FileUpload = false;
            outputStream.Close();
        }
    }
}
