using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using CultObjectProcessing;
using CSVFormat;
using JSONFormat;
using Serilog;

namespace TelegramBotProcessing
{
    /// <summary>
    /// This class handles updates from Telegram Bot API and performs various operations on the received data.
    /// </summary>
    public static class Updates
    {
        /// <summary>
        /// Handles incoming updates from the Telegram Bot API.
        /// </summary>
        /// <param name="botClient">The Telegram Bot client.</param>
        /// <param name="update">The update object received from the Telegram Bot API.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;

            if (message == null)
                return;

            var chatId = message.Chat.Id;

            if (message.Text != null)
                HandleTextMessage(botClient, chatId, message.Text);

            else if (message.Document != null && UserProcessing.userData.ContainsKey(chatId)
                && UserProcessing.userData[chatId].FileType != null && !UserProcessing.userData[chatId].FileDownload)
                HandleDocumentMessage(botClient, chatId, message.Document);
            else
            {
                await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Выберите пункт из предложенных вариантов!");
            }
            Log.Information($"Received a '{message.Text}' message in chat {chatId}.");
        }

        /// <summary>
        /// Handles text messages received from the Telegram Bot API.
        /// </summary>
        /// <param name="botClient">The Telegram Bot client.</param>
        /// <param name="chatId">The chat ID of the user who sent the message.</param>
        /// <param name="text">The text content of the message.</param>
        private static async void HandleTextMessage(ITelegramBotClient botClient, long chatId, string text)
        {
            if (text == "/start")
                UserProcessing.userData.Remove(chatId);
            if (!UserProcessing.userData.ContainsKey(chatId))
            {
                Output.Print(botClient, chatId, Output.start);
                UserProcessing.userData[chatId] = new FileData();
                await MenuKeyBoards.GetTypeOfFile(botClient, chatId);
            }
            else if (UserProcessing.userData[chatId].FileType == null)
                HandleFileTypeInput(botClient, chatId, text);
            else if (UserProcessing.userData[chatId].FileDownload)
            {
                if (UserProcessing.userData[chatId].FileUpload)
                    HandleUploadFile(botClient, chatId, text);
                else
                    HandleMenuTypeInput(botClient, chatId, text);
            }
            else
            {
                await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Отправьте файл.");
                return;
            }
        }

        /// <summary>
        /// Handles the upload of a file in a specific format (CSV or JSON).
        /// </summary>
        /// <param name="botClient">The Telegram Bot client.</param>
        /// <param name="chatId">The chat ID of the user who sent the message.</param>
        /// <param name="text">The text content of the message specifying the file format.</param>
        private static async void HandleUploadFile(ITelegramBotClient botClient, long chatId, string text)
        {
            try
            {
                if (text == "CSV")
                {
                    UserProcessing.userData[chatId].FileType = FileType.Csv;
                    await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Итоговый csv файл.",
                    replyMarkup: new ReplyKeyboardRemove());
                    await Output.SendFile(botClient, chatId, UserProcessing.userData[chatId].Records, text);
                }
                else if (text == "JSON")
                {
                    UserProcessing.userData[chatId].FileType = FileType.Json;
                    await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Итоговый json файл.",
                    replyMarkup: new ReplyKeyboardRemove());
                    await Output.SendFile(botClient, chatId, UserProcessing.userData[chatId].Records, text);
                }
                else
                {
                    await MenuKeyBoards.GetTypeOfFile(botClient, chatId);
                    return;
                }
                await MenuKeyBoards.GetMenu(botClient, chatId);
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Произошла ошибка при записи файла.");
                Log.Fatal($"An error occurred while writing file in chat {chatId}: {ex.Message}.");
                UserProcessing.userData[chatId].FileUpload = false;
                await MenuKeyBoards.GetMenu(botClient, chatId);
            }
        }

        /// <summary>
        /// Handles document messages received from the Telegram Bot API.
        /// </summary>
        /// <param name="botClient">The Telegram Bot client.</param>
        /// <param name="chatId">The chat ID of the user who sent the message.</param>
        /// <param name="document">The document object received from the Telegram Bot API.</param>
        private static async void HandleDocumentMessage(ITelegramBotClient botClient, long chatId, Document document)
        {
            if (UserProcessing.userData.ContainsKey(chatId) && !UserProcessing.userData[chatId].FileDownload &&
                UserProcessing.userData[chatId].FileType != null)
            {
                try
                {
                    var fileId = document.FileId;
                    var fileInfo = await botClient.GetFileAsync(fileId);
                    var filePath = fileInfo.FilePath ?? string.Empty;

                    string name = document.FileName ?? string.Empty;
                    UserProcessing.userData[chatId].FileName = name.Replace(UserProcessing.userData[chatId].
                        FileType.ToString()!.ToLower(), "");
                    if (!name.Contains("." + UserProcessing.userData[chatId].FileType?.ToString().ToLower()))
                    {
                        await botClient.SendTextMessageAsync(
                          chatId: chatId,
                          text: "Введите файл с корректным форматом!");
                        return;
                    }
                    using var stream = new MemoryStream();
                    await botClient.DownloadFileAsync(filePath, stream);
                    stream.Position = 0;
                    if (UserProcessing.userData[chatId].FileType == FileType.Csv)
                    {
                        List<CultObject>? list = CSVProcessing.Read(stream);
                        if (list != null)
                            UserProcessing.userData[chatId].Records = list;
                        else
                        {
                            await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Введите файл с корректными данными!");
                            return;
                        }
                    }
                    else
                    {
                        UserProcessing.userData[chatId].Records = JSONProcessing.Read(stream);
                    }
                    UserProcessing.userData[chatId].FileDownload = true;
                    stream.Close();
                    await MenuKeyBoards.GetMenu(botClient, chatId);
                }
                catch (Exception ex)
                {
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Произошла ошибка при чтении файла.");
                    Log.Fatal($"An error occurred while reading file in chat {chatId}: {ex.Message}.");
                    UserProcessing.userData[chatId].FileType = null;
                    await MenuKeyBoards.GetTypeOfFile(botClient, chatId);
                }
            }
        }

        /// <summary>
        /// Handles the input for selecting the file type (JSON or CSV).
        /// </summary>
        /// <param name="botClient">The Telegram Bot client.</param>
        /// <param name="chatId">The chat ID of the user who sent the message.</param>
        /// <param name="input">The user input specifying the file type.</param>
        private static async void HandleFileTypeInput(ITelegramBotClient botClient, long chatId, string input)
        {
            var userData = UserProcessing.userData[chatId];

            if (userData.FileType == null)
            {
                if (input == "JSON")
                {
                    userData.FileType = FileType.Json;
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Отправьте JSON файл.",
                        replyMarkup: new ReplyKeyboardRemove());
                }
                else if (input == "CSV")
                {
                    userData.FileType = FileType.Csv;
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Отправьте CSV файл.",
                        replyMarkup: new ReplyKeyboardRemove());
                }
                else
                {
                    UserProcessing.userData[chatId].FileType = null;
                    await MenuKeyBoards.GetTypeOfFile(botClient, chatId);
                    return;
                }
            }
        }

        /// <summary>
        /// Handles the input for selecting the menu option (sort, filter, download, or upload new file).
        /// </summary>
        /// <param name="botClient">The Telegram Bot client.</param>
        /// <param name="chatId">The chat ID of the user who sent the message.</param>
        /// <param name="input">The user input specifying the menu option.</param>
        private static async void HandleMenuTypeInput(ITelegramBotClient botClient, long chatId, string input)
        {
            var userData = UserProcessing.userData[chatId];

            if (userData.Records != null)
            {
                if (userData.SortMode)
                    DataChanger.HandleSort(botClient, chatId, input, userData);
                else if (userData.FilterMode)
                    DataChanger.HandleFilter(botClient, chatId, input, userData);
                else
                {
                    switch (input)
                    {
                        case "Отсортировать":
                            userData.SortMode = true;
                            await MenuKeyBoards.SortIsReverse(botClient, chatId);
                            break;
                        case "Произвести выборку":
                            userData.FilterMode = true;
                            await MenuKeyBoards.FilterField(botClient, chatId);
                            break;
                        case "Скачать":
                            userData.FileUpload = true;
                            await MenuKeyBoards.GetTypeOfFile(botClient, chatId);
                            break;
                        case "Загрузить новый файл":
                            userData.UpdateUserInfo();
                            await MenuKeyBoards.GetTypeOfFile(botClient, chatId);
                            break;
                        default:
                            await MenuKeyBoards.GetMenu(botClient, chatId);
                            break;
                    }
                }
            }
        }
    }
}
