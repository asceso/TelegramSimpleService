using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramSimpleService;

namespace ServiceTests
{
    public class UnitTests
    {
        private static long uidToSend;
        private static string botToken;
        private static ITelegramService telegramService;
        private static IKeyboardService keyboardService;

        [SetUp]
        public void Setup()
        {
            uidToSend = 0123456789;
            botToken = "YOUR_TOKEN";
            telegramService = new TelegramService();
            keyboardService = new KeyboardService();
        }

        [Test]
        public async Task InitAsync()
        {
            if (botToken != "YOUR_TOKEN")
            {
                bool isCorrect = await telegramService.CheckBotTokenAsync(botToken);
                Assert.IsTrue(isCorrect);
            }
        }

        [Test]
        public async Task TestKeyboard()
        {
            telegramService.CreateMainBot(botToken);

            Dictionary<string, ReplyKeyboardMarkup> r_keys = new Dictionary<string, ReplyKeyboardMarkup>();
            r_keys.Add("Test1", new ReplyKeyboardMarkup(new List<List<KeyboardButton>>()
            {
                new List<KeyboardButton>()
                {
                    new KeyboardButton("Test Row 1")
                },
                new List<KeyboardButton>()
                {
                    new KeyboardButton("Test Row 2")
                },
                new List<KeyboardButton>()
                {
                    new KeyboardButton("Test Row 3.1"),
                    new KeyboardButton("Test Row 3.2"),
                },
                new List<KeyboardButton>()
                {
                    new KeyboardButton("Test Row 4"),
                }
            }));
            r_keys.Add("Test2", new ReplyKeyboardMarkup(new List<List<KeyboardButton>>()
            {
                new List<KeyboardButton>()
                {
                    new KeyboardButton("Test Row 1")
                },
                new List<KeyboardButton>()
                {
                    new KeyboardButton("Test Row 2")
                }
            }));
            await keyboardService.SaveKeyboardsAsync(r_keys);
            Assert.IsTrue(System.IO.File.Exists(keyboardService.GetFileName(KeyboardType.Reply)));

            Dictionary<string, InlineKeyboardMarkup> i_keys = new Dictionary<string, InlineKeyboardMarkup>();
            i_keys.Add("Test1", new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>()
            {
                new List<InlineKeyboardButton>()
                {
                    new InlineKeyboardButton("Test Row 1") { CallbackData = "Data Row 1" }
                },
                new List<InlineKeyboardButton>()
                {
                    new InlineKeyboardButton("Test Row 2") { CallbackData = "Data Row 2" }
                },
                new List<InlineKeyboardButton>()
                {
                    new InlineKeyboardButton("Test Row 3.1") { CallbackData = "Data Row 3.1" },
                    new InlineKeyboardButton("Test Row 3.2") { CallbackData = "Data Row 3.2" },
                },
                new List<InlineKeyboardButton>()
                {
                    new InlineKeyboardButton("Test Row 4") { CallbackData = "Data Row 4" },
                }
            }));
            i_keys.Add("Test2", new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>()
            {
                new List<InlineKeyboardButton>()
                {
                    new InlineKeyboardButton("Test Row 1") { CallbackData = "Data Row 1" }
                },
                new List<InlineKeyboardButton>()
                {
                    new InlineKeyboardButton("Test Row 2") { CallbackData = "Data Row 2" }
                }
            }));
            await keyboardService.SaveKeyboardsAsync(i_keys);
            Assert.IsTrue(System.IO.File.Exists(keyboardService.GetFileName(KeyboardType.Inline)));

            Dictionary<string, ReplyKeyboardMarkup> loaded_r_keys = (Dictionary<string, ReplyKeyboardMarkup>)await keyboardService.LoadKeyboardsAsync(KeyboardType.Reply);
            foreach (string key in loaded_r_keys.Keys)
            {
                Assert.IsTrue(r_keys.Any(r => r.Key == key));
            }

            Dictionary<string, InlineKeyboardMarkup> loaded_i_keys = (Dictionary<string, InlineKeyboardMarkup>)await keyboardService.LoadKeyboardsAsync(KeyboardType.Inline);
            foreach (string key in loaded_i_keys.Keys)
            {
                Assert.IsTrue(i_keys.Any(r => r.Key == key));
            }
        }

        [Test]
        public void TestOneRowLoad()
        {
            Dictionary<string, ReplyKeyboardMarkup> r_keys = new Dictionary<string, ReplyKeyboardMarkup>();
            r_keys.Add("Test1", new ReplyKeyboardMarkup(new List<List<KeyboardButton>>()
            {
                new List<KeyboardButton>()
                {
                    new KeyboardButton("Test Row 1")
                },
                new List<KeyboardButton>()
                {
                    new KeyboardButton("Test Row 2")
                },
                new List<KeyboardButton>()
                {
                    new KeyboardButton("Test Row 3.1"),
                    new KeyboardButton("Test Row 3.2"),
                },
                new List<KeyboardButton>()
                {
                    new KeyboardButton("Test Row 4"),
                }
            }));
            r_keys.Add("Test2", new ReplyKeyboardMarkup(new List<List<KeyboardButton>>()
            {
                new List<KeyboardButton>()
                {
                    new KeyboardButton("Test Row 1")
                },
                new List<KeyboardButton>()
                {
                    new KeyboardButton("Cancel 🚫"),
                    new KeyboardButton("Shop 🏪")
                },
                new List<KeyboardButton>()
                {
                    new KeyboardButton("Test Row 2")
                }
            }));
            keyboardService.SaveKeyboardsToOneRowType(r_keys);

            Dictionary<string, ReplyKeyboardMarkup> loaded_r_keys = keyboardService.LoadOneRowKeyboards();
            foreach (string key in loaded_r_keys.Keys)
            {
                Assert.IsTrue(r_keys.Any(r => r.Key == key));
            }
        }

        [Test]
        public void SendInlineKeyboard()
        {
            TelegramBotClient bot = telegramService.CreateMainBot(botToken);

            var data = new List<Tuple<string, string>>();
            for (int i = 1; i <= 85; i++)
            {
                data.Add(new Tuple<string, string>($"Btn{i}", $"D{i}"));
            }

            Tuple<string, string> backBtn = new Tuple<string, string>("⬅️", "back");
            Tuple<string, string> nextBtn = new Tuple<string, string>("➡️", "fwd");

            bot.SendTextMessageAsync(uidToSend, "test page 1", replyMarkup: keyboardService.GeneratePagedInlineKeyboard(
                data, 1, 6, 2, backBtn, nextBtn))
                .Wait();

            bot.SendTextMessageAsync(uidToSend, "test page 2", replyMarkup: keyboardService.GeneratePagedInlineKeyboard(
                data, 2, 6, 2, backBtn, nextBtn))
                .Wait();

            bot.SendTextMessageAsync(uidToSend, "test page 15", replyMarkup: keyboardService.GeneratePagedInlineKeyboard(
                data, 15, 6, 2, backBtn, nextBtn))
                .Wait();
        }
    }
}