using DataFilter;
using Serilog;
using Telegram.Bot;

namespace TelegramBotProcessing
{
    /// <summary>
    /// Provides methods for handling data filtering and sorting operations in a Telegram bot.
    /// </summary>
    public static class DataChanger
    {
        /// <summary>
        /// Handles sorting of cultural objects based on user input.
        /// </summary>
        /// <param name="botClient">The Telegram bot client instance.</param>
        /// <param name="chatId">The ID of the chat where the sorting operation is requested.</param>
        /// <param name="input">The user input specifying the sorting order.</param>
        /// <param name="userData">The FileData object containing the cultural objects and user data.</param>
        public static async void HandleSort(ITelegramBotClient botClient, long chatId, string input, FileData userData)
        {
            if (userData.Records == null)
            {
                await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Данных нет!");
                await MenuKeyBoards.GetMenu(botClient, chatId);
                return;
            }
            try
            {
                if (input == "Сортировать в прямом порядке")
                {
                    userData.Records = DataSort.Sort.SortByObjectNameOnDoc(userData.Records, true).ToList();
                    userData.SortMode = false;
                    await MenuKeyBoards.GetMenu(botClient, chatId);
                }
                else if (input == "Сортировать в обратном порядке")
                {
                    userData.Records = DataSort.Sort.SortByObjectNameOnDoc(userData.Records, false).ToList();
                    userData.SortMode = false;
                    await MenuKeyBoards.GetMenu(botClient, chatId);
                }
                else
                {
                    await MenuKeyBoards.SortIsReverse(botClient, chatId);
                    return;
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Произошла ошибка при сортировке данных.");
                Log.Fatal($"An error occurred while sorting data in chat {chatId}: {ex.Message}.");
                UserProcessing.userData[chatId].SortMode = false;
                await MenuKeyBoards.GetMenu(botClient, chatId);
            }
        }

        /// <summary>
        /// Handles filtering of cultural objects based on user input.
        /// </summary>
        /// <param name="botClient">The Telegram bot client instance.</param>
        /// <param name="chatId">The ID of the chat where the filtering operation is requested.</param>
        /// <param name="input">The user input specifying the filtering criteria.</param>
        /// <param name="userData">The FileData object containing the cultural objects and user data.</param>
        public static async void HandleFilter(ITelegramBotClient botClient, long chatId, string input, FileData userData)
        {
            try
            {
                if (userData.Records == null)
                {
                    await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Данных нет!");
                    await MenuKeyBoards.GetMenu(botClient, chatId);
                    return;
                }
                if (userData.FilterField == null)
                    FilterField(botClient, chatId, input, userData);
                else
                {
                    if (userData.IsTwoFields)
                        TwoFilterField(botClient, chatId, input, userData);
                    else
                        OneFilterField(botClient, chatId, input, userData);
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Произошла ошибка при фильтрации данных.");
                Log.Fatal($"An error occurred while filtring data in chat {chatId}: {ex.Message}.");
                userData.FilterMode = false;
                userData.FilterField = null;
                userData.FirstFilterVar = null;
                userData.SecondFilterVar = null;
                await MenuKeyBoards.GetMenu(botClient, chatId);
            }
        }

        /// <summary>
        /// Handles the selection of the filter field for cultural objects based on user input.
        /// </summary>
        /// <param name="botClient">The Telegram bot client instance.</param>
        /// <param name="chatId">The ID of the chat where the filter field selection is requested.</param>
        /// <param name="input">The user input specifying the filter field.</param>
        /// <param name="userData">The FileData object containing the cultural objects and user data.</param>
        public static async void FilterField(ITelegramBotClient botClient, long chatId, string input, FileData userData)
        {
            if (input == "SecurityStatus" || input == "ObjectType")
            {
                userData.FilterField = input;
                string[] variants = FilterProcessing.GetVariants(input, userData.Records!);
                userData.IsTwoFields = false;
                await MenuKeyBoards.FieldVariants(botClient, chatId, variants);
            }
            else if (input == "SecurityStatus и Category")
            {
                userData.FilterField = input;
                string[] variants = FilterProcessing.GetVariants("SecurityStatus", userData.Records!);
                userData.IsTwoFields = true;
                await MenuKeyBoards.FieldVariants(botClient, chatId, variants);
            }
            else
            {
                await MenuKeyBoards.FilterField(botClient, chatId);
                return;
            }
        }

        /// <summary>
        /// Handles the selection of two filter fields for cultural objects based on user input.
        /// </summary>
        /// <param name="botClient">The Telegram bot client instance.</param>
        /// <param name="chatId">The ID of the chat where the two filter fields selection is requested.</param>
        /// <param name="input">The user input specifying the filter field value.</param>
        /// <param name="userData">The FileData object containing the cultural objects and user data.</param>
        public static async void TwoFilterField(ITelegramBotClient botClient, long chatId, string input, FileData userData)
        {
            if (userData.FirstFilterVar == null)
            {
                if (!FilterProcessing.GetVariants("SecurityStatus", userData.Records!).Contains(input))
                {
                    await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Такого значения не существует!");
                    await MenuKeyBoards.FieldVariants(botClient, chatId, FilterProcessing.GetVariants("SecurityStatus",
                        userData.Records!));
                    return;
                }
                userData.FirstFilterVar = input;
                userData.Records = Filter.FilterByField(userData.Records!, "SecurityStatus", userData.FirstFilterVar).ToList();
                string[] variants = FilterProcessing.GetVariants("Category", userData.Records);
                await MenuKeyBoards.FieldVariants(botClient, chatId, variants);
            }
            else
            {
                if (!FilterProcessing.GetVariants("Category", userData.Records!).Contains(input))
                {
                    await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Такого значения не существует!");
                    await MenuKeyBoards.FieldVariants(botClient, chatId, FilterProcessing.GetVariants("Category", userData.Records!));
                    return;
                }
                userData.SecondFilterVar = input;
                userData.Records = Filter.FilterByField(userData.Records!, "Category", userData.SecondFilterVar).ToList();
                userData.FilterMode = false;
                userData.FilterField = null;
                userData.FirstFilterVar = null;
                userData.SecondFilterVar = null;
                await MenuKeyBoards.GetMenu(botClient, chatId);
            }
        }

        /// <summary>
        /// Handles the selection of a single filter field value for cultural objects based on user input.
        /// </summary>
        /// <param name="botClient">The Telegram bot client instance.</param>
        /// <param name="chatId">The ID of the chat where the single filter field value selection is requested.</param>
        /// <param name="input">The user input specifying the filter field value.</param>
        /// <param name="userData">The FileData object containing the cultural objects and user data.</param>
        public static async void OneFilterField(ITelegramBotClient botClient, long chatId, string input, FileData userData)
        {
            if (!FilterProcessing.GetVariants(userData.FilterField!, userData.Records!).Contains(input))
            {
                await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Такого значения не существует!");
                await MenuKeyBoards.FieldVariants(botClient, chatId,
                    FilterProcessing.GetVariants(userData.FilterField!, userData.Records!));
                return;
            }
            userData.FirstFilterVar = input;
            userData.Records = Filter.FilterByField(userData.Records!, userData.FilterField!, userData.FirstFilterVar).ToList();
            userData.FilterMode = false;
            userData.FilterField = null;
            userData.FirstFilterVar = null;
            await MenuKeyBoards.GetMenu(botClient, chatId);
        }
    }
}
