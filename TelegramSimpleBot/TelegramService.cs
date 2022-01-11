using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramSimpleBot
{
    public class TelegramService : ITelegramService
    {
        private static TelegramBotClient Client;
        private static TelegramBotClient Debugger;

        public async Task<bool> CheckBotToken(string token)
        {
            try
            {
                var client = new TelegramBotClient(token);
                var bot = await Client.GetMeAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Task<TelegramBotClient> CreateMainBot(string token)
        {
            try
            {
                Client = new TelegramBotClient(token);
                return Task.FromResult(Client);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Task<TelegramBotClient> CreateDebugBot(string token)
        {
            try
            {
                Debugger = new TelegramBotClient(token);
                return Task.FromResult(Debugger);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Task<CancellationTokenSource> StartMainBotReceiving(IUpdateHandler updateHandler, params UpdateType[] allowedTypes)
        {
            if (Client != null)
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                Client.StartReceiving(
                    updateHandler,
                    receiverOptions: new ReceiverOptions()
                    {
                        AllowedUpdates = allowedTypes
                    },
                    cancellationToken: cts.Token
                    );
                return Task.FromResult(cts);
            }
            return null;
        }
        public async Task SendLog(long uid, string message)
        {
            try
            {
                await Debugger.SendTextMessageAsync(uid, message);
            }
            catch (Exception)
            {
            }
        }
        public async Task SendRemoveMessage(long uid, string message) => await Client.SendTextMessageAsync(uid, message, replyMarkup: new ReplyKeyboardRemove());
        public async Task SendMessage(long uid, string message) => await Client.SendTextMessageAsync(uid, message, ParseMode.Html);
        public async Task SendMessageWithFile(long uid, string message, FileStream fs, string fileName, bool deleteFileWhenComplete = true)
        {
            await Client.SendDocumentAsync(uid, new Telegram.Bot.Types.InputFiles.InputOnlineFile(fs, fileName), caption: message);
            fs.Close();
            if (deleteFileWhenComplete)
            {
                System.IO.File.Delete(fs.Name);
            }
        }
        public async Task SendMessageWithKeyboard(long uid, string message, ReplyKeyboardMarkup markup) => await Client.SendTextMessageAsync(uid, message, ParseMode.Html, replyMarkup: markup);
        public async Task SendMessageWithKeyboard(long uid, string message, InlineKeyboardMarkup markup) => await Client.SendTextMessageAsync(uid, message, ParseMode.Html, replyMarkup: markup);
    }
}
