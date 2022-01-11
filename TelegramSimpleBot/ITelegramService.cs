using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramSimpleBot
{
    public interface ITelegramService
    {
        /// <summary>
        /// Check exist TG token
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>true if token not broken</returns>
        Task<bool> CheckBotToken(string token);
        /// <summary>
        /// Create main bot
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>TelegramBotClient</returns>
        Task<TelegramBotClient> CreateMainBot(string token);
        /// <summary>
        /// Create debugger bot
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>TelegramBotClient</returns>
        Task<TelegramBotClient> CreateDebugBot(string token);
        /// <summary>
        /// Start receiving for main bot
        /// </summary>
        /// <param name="updateHandler">handler for updates</param>
        /// <param name="allowedTypes">allowed update types</param>
        /// <returns>cancelation token</returns>
        Task<CancellationTokenSource> StartMainBotReceiving(IUpdateHandler updateHandler, params UpdateType[] allowedTypes);
        /// <summary>
        /// Start receiving for other bot
        /// </summary>
        /// <param name="client">bot client for handlers</param>
        /// <param name="updateHandler">handler for updates</param>
        /// <param name="allowedTypes">allowed update types</param>
        /// <returns>cancelation token</returns>
        Task<CancellationTokenSource> StartBotReceiving(TelegramBotClient client, IUpdateHandler updateHandler, params UpdateType[] allowedTypes);
        /// <summary>
        /// Send debug message from debugger bot
        /// </summary>
        /// <param name="uid">uid user</param>
        /// <param name="message">message</param>
        /// <returns></returns>
        Task SendLog(long uid, string message);
        /// <summary>
        /// Send clear keyboard message
        /// </summary>
        /// <param name="uid">uid user</param>
        /// <param name="message">message</param>
        /// <returns></returns>
        Task SendRemoveMessage(long uid, string message, TelegramBotClient client = null);
        /// <summary>
        /// Send simple text message
        /// </summary>
        /// <param name="uid">uid user</param>
        /// <param name="message">message</param>
        /// <returns></returns>
        Task SendMessage(long uid, string message, TelegramBotClient client = null);
        /// <summary>
        /// Send message with file
        /// </summary>
        /// <param name="uid">uid user</param>
        /// <param name="message">message (caption)</param>
        /// <param name="fs">file stream of file FileMode.Open</param>
        /// <param name="fileName">filename in telegram</param>
        /// <param name="deleteFileWhenComplete">delete file when send complete</param>
        /// <returns></returns>
        Task SendMessageWithFile(long uid, string message, FileStream fs, string fileName, bool deleteFileWhenComplete = true, TelegramBotClient client = null);
        /// <summary>
        /// Send message with keyboard reply
        /// </summary>
        /// <param name="uid">uid user</param>
        /// <param name="message">message</param>
        /// <param name="markup">ReplyKeyboardMarkup</param>
        /// <returns></returns>
        Task SendMessageWithKeyboard(long uid, string message, ReplyKeyboardMarkup markup, TelegramBotClient client = null);
        /// <summary>
        /// Send message with inline buttons
        /// </summary>
        /// <param name="uid">uid user</param>
        /// <param name="message">message</param>
        /// <param name="markup">InlineKeyboardMarkup</param>
        /// <returns></returns>
        Task SendMessageWithKeyboard(long uid, string message, InlineKeyboardMarkup markup, TelegramBotClient client = null);
    }
}
