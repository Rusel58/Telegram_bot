namespace TelegramBotProcessing
{
    /// <summary>
    /// Provides functionality for managing user data in the Telegram bot.
    /// </summary>
    public static class UserProcessing
    {
        /// <summary>
        /// A dictionary that stores user data for each chat, where the key is the chat ID and the value is a FileData object.
        /// </summary>
        public static Dictionary<long, FileData> userData = new Dictionary<long, FileData>();
    }
}