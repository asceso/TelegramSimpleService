using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramSimpleService
{
    public class TelegramService : ITelegramService
    {
        private static TelegramBotClient Client;
        private static TelegramBotClient Debugger;

        public TelegramBotClient CreateMainBot(string token)
        {
            try
            {
                Client = new TelegramBotClient(token);
                return Client;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public TelegramBotClient CreateDebugBot(string token)
        {
            try
            {
                Debugger = new TelegramBotClient(token);
                return Debugger;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public TelegramBotClient CreateOtherBot(string token)
        {
            try
            {
                TelegramBotClient client = new TelegramBotClient(token);
                return client;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public TelegramBotClient GetMainBot() => Client;

        public TelegramBotClient GetDebugBot() => Debugger;

        public CancellationTokenSource StartMainBotReceiving(IUpdateHandler updateHandler, params UpdateType[] allowedTypes)
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
                return cts;
            }
            return null;
        }

        public CancellationTokenSource StartBotReceiving(TelegramBotClient client, IUpdateHandler updateHandler, params UpdateType[] allowedTypes)
        {
            if (client != null)
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                client.StartReceiving(
                    updateHandler,
                    receiverOptions: new ReceiverOptions()
                    {
                        AllowedUpdates = allowedTypes
                    },
                    cancellationToken: cts.Token
                    );
                return cts;
            }
            return null;
        }

        public async Task<bool> CheckBotTokenAsync(string token)
        {
            try
            {
                var client = new TelegramBotClient(token);
                var bot = await client.GetMeAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task SendLogAsync(long uid, string message)
        {
            try
            {
                await Debugger.SendTextMessageAsync(uid, message);
            }
            catch (Exception)
            {
            }
        }

        public async Task DeleteMessageAsync(long chatId, int messageId, TelegramBotClient client = null)
        {
            if (client != null)
            {
                await client.DeleteMessageAsync(chatId, messageId);
            }
            else if (Client != null)
            {
                await Client.DeleteMessageAsync(chatId, messageId);
            }
        }

        public async Task SendRemoveMessageAsync(long uid, string message, ParseMode parse = ParseMode.Html, TelegramBotClient client = null)
        {
            if (client != null)
            {
                await client.SendTextMessageAsync(uid, message, parse, replyMarkup: new ReplyKeyboardRemove());
            }
            else if (Client != null)
            {
                await Client.SendTextMessageAsync(uid, message, parse, replyMarkup: new ReplyKeyboardRemove());
            }
        }

        public async Task SendMessageAsync(long uid, string message, ParseMode parse = ParseMode.Html, TelegramBotClient client = null)
        {
            if (client != null)
            {
                await client.SendTextMessageAsync(uid, message, parse);
            }
            else if (Client != null)
            {
                await Client.SendTextMessageAsync(uid, message, parse);
            }
        }

        public async Task SendMessageWithKeyboardAsync(long uid, string message, ReplyKeyboardMarkup markup, ParseMode parse = ParseMode.Html, TelegramBotClient client = null)
        {
            if (client != null)
            {
                await client.SendTextMessageAsync(uid, message, parse, replyMarkup: markup);
            }
            else if (Client != null)
            {
                await Client.SendTextMessageAsync(uid, message, parse, replyMarkup: markup);
            }
        }

        public async Task SendMessageWithKeyboardAsync(long uid, string message, InlineKeyboardMarkup markup, ParseMode parse = ParseMode.Html, TelegramBotClient client = null)
        {
            if (client != null)
            {
                await client.SendTextMessageAsync(uid, message, parse, replyMarkup: markup);
            }
            else if (Client != null)
            {
                await Client.SendTextMessageAsync(uid, message, parse, replyMarkup: markup);
            }
        }

        public async Task SendMessageWithKeyboardAsync(long uid, string message, ReplyKeyboardRemove markup, ParseMode parse = ParseMode.Html, TelegramBotClient client = null)
        {
            if (client != null)
            {
                await client.SendTextMessageAsync(uid, message, parse, replyMarkup: markup);
            }
            else if (Client != null)
            {
                await Client.SendTextMessageAsync(uid, message, parse, replyMarkup: markup);
            }
        }

        public async Task SendMessageWithKeyboardAsync(long uid, string message, object markup, ParseMode parse = ParseMode.Html, TelegramBotClient client = null)
        {
            if (markup is ReplyKeyboardMarkup replyKeyboardMarkup)
            {
                await SendMessageWithKeyboardAsync(uid, message, replyKeyboardMarkup, parse, client);
            }
            else if (markup is InlineKeyboardMarkup inlineKeyboardMarkup)
            {
                await SendMessageWithKeyboardAsync(uid, message, inlineKeyboardMarkup, parse, client);
            }
            else if (markup == null)
            {
                await SendMessageWithKeyboardAsync(uid, message, new ReplyKeyboardRemove(), parse, client);
            }
            else
            {
                throw new ArgumentException("Another types not support by this method");
            }
        }

        public async Task SendMessageWithFileAsync(long uid,
                                                   string message,
                                                   FileStream fs,
                                                   string fileName,
                                                   bool deleteFileWhenComplete = true,
                                                   ParseMode parse = ParseMode.Html,
                                                   TelegramBotClient client = null)
        {
            if (client != null)
            {
                await client.SendDocumentAsync(uid, new Telegram.Bot.Types.InputFiles.InputOnlineFile(fs, fileName), caption: message, parseMode: parse);
                fs.Close();
                if (deleteFileWhenComplete)
                {
                    File.Delete(fs.Name);
                }
            }
            else if (Client != null)
            {
                await Client.SendDocumentAsync(uid, new Telegram.Bot.Types.InputFiles.InputOnlineFile(fs, fileName), caption: message, parseMode: parse);
                fs.Close();
                if (deleteFileWhenComplete)
                {
                    File.Delete(fs.Name);
                }
            }
        }
    }
}