using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramSimpleService
{
    public interface ITelegramService
    {
        /// <summary>
        /// Create main bot
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>TelegramBotClient</returns>
        TelegramBotClient CreateMainBot(string token);

        /// <summary>
        /// Create debugger bot
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>TelegramBotClient</returns>
        TelegramBotClient CreateDebugBot(string token);

        /// <summary>
        /// Create other bot
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>TelegramBotClient</returns>
        TelegramBotClient CreateOtherBot(string token);

        /// <summary>
        /// Get main bot
        /// </summary>
        /// <returns>if bot is null return null</returns>
        TelegramBotClient GetMainBot();

        /// <summary>
        /// Get debug bot
        /// </summary>
        /// <returns>if bot is null return null</returns>
        TelegramBotClient GetDebugBot();

        /// <summary>
        /// Start receiving for main bot
        /// </summary>
        /// <param name="updateHandler">handler for updates</param>
        /// <param name="allowedTypes">allowed update types</param>
        /// <returns>cancelation token</returns>
        CancellationTokenSource StartMainBotReceiving(IUpdateHandler updateHandler, params UpdateType[] allowedTypes);

        /// <summary>
        /// Start receiving for other bot
        /// </summary>
        /// <param name="client">bot client for handlers</param>
        /// <param name="updateHandler">handler for updates</param>
        /// <param name="allowedTypes">allowed update types</param>
        /// <returns>cancelation token</returns>
        CancellationTokenSource StartBotReceiving(TelegramBotClient client, IUpdateHandler updateHandler, params UpdateType[] allowedTypes);

        /// <summary>
        /// Check exist TG token
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>true if token not broken</returns>
        Task<bool> CheckBotTokenAsync(string token);

        /// <summary>
        /// Send debug message from debugger bot
        /// </summary>
        /// <param name="uid">uid user</param>
        /// <param name="message">message</param>
        /// <returns></returns>
        Task SendLogAsync(long uid, string message);

        /// <summary>
        /// Delete message from chat
        /// </summary>
        /// <param name="chatId">id chat</param>
        /// <param name="messageId">id message</param>
        /// <param name="client">client may be null</param>
        /// <returns></returns>
        Task DeleteMessageAsync(long chatId, int messageId, TelegramBotClient client = null);

        /// <summary>
        /// Send clear keyboard message
        /// </summary>
        /// <param name="uid">uid user</param>
        /// <param name="message">message</param>
        /// <param name="parse">parse mode, default is HTML</param>
        /// <param name="client">client may be null</param>
        /// <returns></returns>
        Task SendRemoveMessageAsync(long uid, string message, ParseMode parse = ParseMode.Html, TelegramBotClient client = null);

        /// <summary>
        /// Send simple text message
        /// </summary>
        /// <param name="uid">uid user</param>
        /// <param name="message">message</param>
        /// <param name="parse">parse mode, default is HTML</param>
        /// <param name="client">client may be null</param>
        /// <returns></returns>
        Task SendMessageAsync(long uid, string message, ParseMode parse = ParseMode.Html, TelegramBotClient client = null);

        /// <summary>
        /// Send message with keyboard reply
        /// </summary>
        /// <param name="uid">uid user</param>
        /// <param name="message">message</param>
        /// <param name="markup">ReplyKeyboardMarkup</param>
        /// <param name="parse">parse mode, default is HTML</param>
        /// <param name="client">client may be null</param>
        /// <returns></returns>
        Task SendMessageWithKeyboardAsync(long uid, string message, ReplyKeyboardMarkup markup, ParseMode parse = ParseMode.Html, TelegramBotClient client = null);

        /// <summary>
        /// Send message with inline buttons
        /// </summary>
        /// <param name="uid">uid user</param>
        /// <param name="message">message</param>
        /// <param name="markup">InlineKeyboardMarkup</param>
        /// <param name="parse">parse mode, default is HTML</param>
        /// <param name="client">client may be null</param>
        /// <returns></returns>
        Task SendMessageWithKeyboardAsync(long uid, string message, InlineKeyboardMarkup markup, ParseMode parse = ParseMode.Html, TelegramBotClient client = null);

        /// <summary>
        /// Send message without buttons
        /// </summary>
        /// <param name="uid">uid user</param>
        /// <param name="message">message</param>
        /// <param name="markup">remove markup</param>
        /// <param name="parse">parse mode, default is HTML</param>
        /// <param name="client">client may be null</param>
        /// <returns></returns>
        Task SendMessageWithKeyboardAsync(long uid, string message, ReplyKeyboardRemove markup, ParseMode parse = ParseMode.Html, TelegramBotClient client = null);

        /// <summary>
        /// Send message with file
        /// </summary>
        /// <param name="uid">uid user</param>
        /// <param name="message">message (caption)</param>
        /// <param name="fs">file stream of file FileMode.Open</param>
        /// <param name="fileName">filename in telegram</param>
        /// <param name="deleteFileWhenComplete">delete file when send complete</param>
        /// <param name="parse">parse mode, default is HTML</param>
        /// <param name="client">client may be null</param>
        /// <returns></returns>
        Task SendMessageWithFileAsync(long uid,
                                      string message,
                                      FileStream fs,
                                      string fileName,
                                      bool deleteFileWhenComplete = true,
                                      ParseMode parse = ParseMode.Html,
                                      TelegramBotClient client = null);
    }
}