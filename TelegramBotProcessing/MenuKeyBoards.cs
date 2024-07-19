using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;

namespace TelegramBotProcessing
{
    /// <summary>
    /// Provides methods for creating and managing keyboard menus and buttons for the Telegram bot.
    /// </summary>
    public static class MenuKeyBoards
    {
        /// <summary>
        /// Sends a message with a keyboard to choose the file type (CSV or JSON).
        /// </summary>
        /// <param name="botClient">The Telegram bot client instance.</param>
        /// <param name="chatId">The ID of the chat where the message will be sent.</param>
        public static async Task GetTypeOfFile(ITelegramBotClient botClient, long chatId)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                new KeyboardButton[] { "CSV" },
                new KeyboardButton[] { "JSON" },
            })
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Выберите тип файла:",
                replyMarkup: replyKeyboardMarkup);
        }

        /// <summary>
        /// Sends a message with the main menu keyboard.
        /// </summary>
        /// <param name="botClient">The Telegram bot client instance.</param>
        /// <param name="chatId">The ID of the chat where the message will be sent.</param>
        public static async Task GetMenu(ITelegramBotClient botClient, long chatId)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                   new KeyboardButton[] { "Произвести выборку" },
                   new KeyboardButton[] { "Отсортировать" },
                   new KeyboardButton[] { "Скачать" },
                   new KeyboardButton[] { "Загрузить новый файл" },
            })
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Выберите пункт меню:",
                replyMarkup: replyKeyboardMarkup);
        }

        /// <summary>
        /// Sends a message with a keyboard to choose the sorting order (ascending or descending).
        /// </summary>
        /// <param name="botClient">The Telegram bot client instance.</param>
        /// <param name="chatId">The ID of the chat where the message will be sent.</param>
        public static async Task SortIsReverse(ITelegramBotClient botClient, long chatId)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                   new KeyboardButton[] { "Сортировать в прямом порядке" },
                   new KeyboardButton[] { "Сортировать в обратном порядке" },
            })
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: "Выберите пункт:",
               replyMarkup: replyKeyboardMarkup);
        }

        /// <summary>
        /// Sends a message with a keyboard to choose the filter field (SecurityStatus, ObjectType, or SecurityStatus and Category).
        /// </summary>
        /// <param name="botClient">The Telegram bot client instance.</param>
        /// <param name="chatId">The ID of the chat where the message will be sent.</param>
        public static async Task FilterField(ITelegramBotClient botClient, long chatId)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                   new KeyboardButton[] { "SecurityStatus" },
                   new KeyboardButton[] { "ObjectType" },
                   new KeyboardButton[] { "SecurityStatus и Category" },
            })
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Выберите пункт:",
                replyMarkup: replyKeyboardMarkup);
        }

        /// <summary>
        /// Sends a message with a keyboard to choose the filter field (SecurityStatus, ObjectType, or SecurityStatus and Category).
        /// </summary>
        /// <param name="botClient">The Telegram bot client instance.</param>
        /// <param name="chatId">The ID of the chat where the message will be sent.</param>
        /// <param name="input">The input value to be displayed on the keyboard.</param>
        public static async Task FilterVariants(ITelegramBotClient botClient, long chatId, string input)
        {

            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                   new KeyboardButton[] { "SecurityStatus" },
                   new KeyboardButton[] { "ObjectType" },
                   new KeyboardButton[] { "SecurityStatus и Category" },
            })
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Выберите пункт:",
                replyMarkup: replyKeyboardMarkup);
        }

        /// <summary>
        /// Sends a message with a keyboard displaying the available variants for the selected filter field.
        /// </summary>
        /// <param name="botClient">The Telegram bot client instance.</param>
        /// <param name="chatId">The ID of the chat where the message will be sent.</param>
        /// <param name="variants">The array of variants to be displayed on the keyboard.</param>
        public static async Task FieldVariants(ITelegramBotClient botClient, long chatId, string[] variants)
        {
            KeyboardButton[][] buttons = new KeyboardButton[variants.Length][];
            if (variants.Length == 0)
            {
                UserProcessing.userData[chatId].FirstFilterVar = null;
                UserProcessing.userData[chatId].SecondFilterVar = null;
                UserProcessing.userData[chatId].FilterMode = false;
                UserProcessing.userData[chatId].FilterField = null;
                await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Выборка невозможна, значений нет!");
                await GetMenu(botClient, chatId);
                return;
            }
            for (int i = 0; i < variants.Length; i++)
            {
                buttons[i] = new KeyboardButton[] { variants[i] };
            }
            ReplyKeyboardMarkup replyKeyboardMarkup = new(buttons)
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Выберите пункт:",
                replyMarkup: replyKeyboardMarkup);
        }
    }
}