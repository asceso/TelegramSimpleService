using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramSimpleService
{
    public class KeyboardService : IKeyboardService
    {
        private static string replyFileName;
        private static string inlineFileName;

        public KeyboardService()
        {
            replyFileName = Environment.CurrentDirectory + "/r_keys.json";
            inlineFileName = Environment.CurrentDirectory + "/i_keys.json";
        }
        public string GetFileName(KeyboardType keyboardType)
        {
            return keyboardType switch
            {
                KeyboardType.Reply => replyFileName,
                KeyboardType.Inline => inlineFileName,
                _ => string.Empty,
            };
        }
        public Task<bool> SetStoreFileName(string replyKeyboards, string inlineKeyboards)
        {
            try
            {
                replyFileName = Environment.CurrentDirectory + $"/{replyKeyboards}";
                inlineFileName = Environment.CurrentDirectory + $"/{inlineKeyboards}";
                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }
        public async Task<bool> SaveKeyboards(Dictionary<string, ReplyKeyboardMarkup> keyboards)
        {
            try
            {
                using StreamWriter writer = new StreamWriter(replyFileName);
                await writer.WriteAsync(JsonConvert.SerializeObject(keyboards, Formatting.Indented));
                writer.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> SaveKeyboards(Dictionary<string, InlineKeyboardMarkup> keyboards)
        {
            try
            {
                using StreamWriter writer = new StreamWriter(inlineFileName);
                await writer.WriteAsync(JsonConvert.SerializeObject(keyboards, Formatting.Indented));
                writer.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<object> LoadKeyboards(KeyboardType keyboardType)
        {
            try
            {
                switch (keyboardType)
                {
                    case KeyboardType.Reply:
                        {
                            Dictionary<string, ReplyKeyboardMarkup> keys;
                            using (StreamReader reader = new StreamReader(replyFileName))
                            {
                                string buffer = await reader.ReadToEndAsync();
                                reader.Close();
                                keys = JsonConvert.DeserializeObject<Dictionary<string, ReplyKeyboardMarkup>>(buffer);
                            }
                            foreach (var key in keys.Values)
                            {
                                key.ResizeKeyboard = true;
                            }
                            return keys;
                        }
                    case KeyboardType.Inline:
                        {
                            Dictionary<string, InlineKeyboardMarkup> keys;
                            using (StreamReader reader = new StreamReader(inlineFileName))
                            {
                                string buffer = await reader.ReadToEndAsync();
                                reader.Close();
                                keys = JsonConvert.DeserializeObject<Dictionary<string, InlineKeyboardMarkup>>(buffer);
                            }
                            return keys;
                        }
                    default: return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Task<InlineKeyboardMarkup> GenerateInlineKeyboard(List<Tuple<string, string>> tupleTextData)
        {
            List<List<InlineKeyboardButton>> rows = new List<List<InlineKeyboardButton>>();
            foreach (Tuple<string, string> item in tupleTextData)
            {
                List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton(item.Item1) { CallbackData = $"{item.Item2}" }
                };
                rows.Add(buttons);
            }
            return Task.FromResult(new InlineKeyboardMarkup(rows));
        }
        public Task<InlineKeyboardMarkup> GeneratePagedInlineKeyboard(List<Tuple<string, string>> tupleTextData, int pageNumber, int countInPage, Tuple<string, string> backButton, Tuple<string, string> forwardButton)
        {
            List<List<InlineKeyboardButton>> rows = new List<List<InlineKeyboardButton>>();

            int rowCount = 0;
            int pageMax = tupleTextData.Count / countInPage;
            if (tupleTextData.Count - (pageMax * countInPage) > 0)
            {
                pageMax++;
            }
            if (pageNumber > pageMax)
            {
                pageNumber = pageMax;
            }
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            foreach (var item in tupleTextData.Skip((pageNumber - 1) * countInPage))
            {
                List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton(item.Item1)
                    {
                        CallbackData = $"{item.Item2}",
                    }
                };
                rows.Add(buttons);
                rowCount++;
                if (rowCount >= countInPage)
                {
                    break;
                }
            }

            if (pageNumber != 1 && pageNumber < pageMax)
            {
                List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton(backButton.Item1) { CallbackData = $"{backButton.Item2}" },
                    new InlineKeyboardButton(forwardButton.Item1) { CallbackData = $"{forwardButton.Item2}" }
                };
                rows.Add(buttons);
            }
            else if (pageNumber == 1)
            {
                List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton(forwardButton.Item1) { CallbackData = $"{forwardButton.Item2}" }
                };
                rows.Add(buttons);
            }
            else if (pageNumber == pageMax)
            {
                List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton(backButton.Item1) { CallbackData = $"{backButton.Item2}" },
                };
                rows.Add(buttons);
            }

            return Task.FromResult(new InlineKeyboardMarkup(rows));
        }
    }
    public enum KeyboardType { Reply, Inline }
}
