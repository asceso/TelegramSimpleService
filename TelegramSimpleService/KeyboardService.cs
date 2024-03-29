﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public bool SetStoreFileName(string replyKeyboards, string inlineKeyboards)
        {
            try
            {
                replyFileName = Environment.CurrentDirectory + $"/{replyKeyboards}";
                inlineFileName = Environment.CurrentDirectory + $"/{inlineKeyboards}";
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SaveKeyboards(Dictionary<string, ReplyKeyboardMarkup> keyboards)
        {
            try
            {
                Dictionary<string, List<string>> savePairs = new Dictionary<string, List<string>>();
                foreach (var pair in keyboards)
                {
                    int rowNum = 1;
                    List<string> saveValues = new List<string>();
                    foreach (var keyboard in pair.Value.Keyboard)
                    {
                        int colNum = 1;
                        foreach (var value in keyboard.Select(k => k.Text))
                        {
                            saveValues.Add($"{rowNum}.{colNum++}:{value}");
                        }
                        rowNum++;
                    }
                    savePairs.Add(pair.Key, saveValues);
                }

                using StreamWriter writer = new StreamWriter(replyFileName);
                writer.Write(JsonConvert.SerializeObject(savePairs, Formatting.Indented));
                writer.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SaveKeyboardsAsync(Dictionary<string, ReplyKeyboardMarkup> keyboards)
        {
            try
            {
                Dictionary<string, List<string>> savePairs = new Dictionary<string, List<string>>();
                foreach (var pair in keyboards)
                {
                    int rowNum = 1;
                    List<string> saveValues = new List<string>();
                    foreach (var keyboard in pair.Value.Keyboard)
                    {
                        int colNum = 1;
                        foreach (var value in keyboard.Select(k => k.Text))
                        {
                            saveValues.Add($"{rowNum}.{colNum++}:{value}");
                        }
                        rowNum++;
                    }
                    savePairs.Add(pair.Key, saveValues);
                }

                using StreamWriter writer = new StreamWriter(replyFileName);
                await writer.WriteAsync(JsonConvert.SerializeObject(savePairs, Formatting.Indented));
                writer.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SaveKeyboards(Dictionary<string, InlineKeyboardMarkup> keyboards)
        {
            try
            {
                Dictionary<string, List<string>> savePairs = new Dictionary<string, List<string>>();
                foreach (var pair in keyboards)
                {
                    int rowNum = 1;
                    List<string> saveValues = new List<string>();
                    foreach (var keyboard in pair.Value.InlineKeyboard)
                    {
                        int colNum = 1;
                        foreach (var value in keyboard.ToList())
                        {
                            saveValues.Add($"{rowNum}.{colNum++}:{value.Text}:{value.CallbackData}");
                        }
                        rowNum++;
                    }
                    savePairs.Add(pair.Key, saveValues);
                }

                using StreamWriter writer = new StreamWriter(inlineFileName);
                writer.WriteAsync(JsonConvert.SerializeObject(savePairs, Formatting.Indented));
                writer.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SaveKeyboardsToOneRowType(Dictionary<string, ReplyKeyboardMarkup> keyboards)
        {
            try
            {
                Dictionary<string, string> saveToFile = new Dictionary<string, string>();
                foreach (var pair in keyboards)
                {
                    StringBuilder savedRow = new StringBuilder();
                    foreach (var keyboard in pair.Value.Keyboard)
                    {
                        foreach (var value in keyboard.Select(k => k.Text))
                        {
                            savedRow.Append("{").Append(value).Append("}");
                        }
                        savedRow.Append("{END()}");
                    }
                    saveToFile.Add(pair.Key, savedRow.ToString());
                }
                using StreamWriter writer = new StreamWriter(replyFileName);
                writer.Write(JsonConvert.SerializeObject(saveToFile, Formatting.Indented));
                writer.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SaveKeyboardsAsync(Dictionary<string, InlineKeyboardMarkup> keyboards)
        {
            try
            {
                Dictionary<string, List<string>> savePairs = new Dictionary<string, List<string>>();
                foreach (var pair in keyboards)
                {
                    int rowNum = 1;
                    List<string> saveValues = new List<string>();
                    foreach (var keyboard in pair.Value.InlineKeyboard)
                    {
                        int colNum = 1;
                        foreach (var value in keyboard.ToList())
                        {
                            saveValues.Add($"{rowNum}.{colNum++}:{value.Text}:{value.CallbackData}");
                        }
                        rowNum++;
                    }
                    savePairs.Add(pair.Key, saveValues);
                }

                using StreamWriter writer = new StreamWriter(inlineFileName);
                await writer.WriteAsync(JsonConvert.SerializeObject(savePairs, Formatting.Indented));
                writer.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public object LoadKeyboards(KeyboardType keyboardType)
        {
            try
            {
                switch (keyboardType)
                {
                    case KeyboardType.Reply:
                        {
                            Dictionary<string, List<string>> savedPairs = new Dictionary<string, List<string>>();
                            using StreamReader reader = new StreamReader(replyFileName);
                            savedPairs = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(reader.ReadToEnd());
                            reader.Close();
                            Dictionary<string, ReplyKeyboardMarkup> keys = new Dictionary<string, ReplyKeyboardMarkup>();

                            foreach (KeyValuePair<string, List<string>> pair in savedPairs)
                            {
                                int currentRow = 1;
                                int currentCol = 1;

                                List<List<KeyboardButton>> rows = new List<List<KeyboardButton>>();
                                List<KeyboardButton> collumns = new List<KeyboardButton>();

                                foreach (string item in pair.Value)
                                {
                                    string[] parts = item.Split(':');
                                    int[] position = parts[0].Split('.').Select(p => int.Parse(p)).ToArray();

                                    if (position[0] != currentRow)
                                    {
                                        currentRow++;
                                        currentCol = 1;

                                        rows.Add(collumns.ToList());
                                        collumns.Clear();
                                    }

                                    collumns.Add(new KeyboardButton(parts[1]));
                                    currentCol++;
                                }

                                rows.Add(collumns.ToList());
                                collumns.Clear();

                                keys.Add(pair.Key, new ReplyKeyboardMarkup(rows.ToList()));
                                rows.Clear();
                            }

                            foreach (var key in keys.Values)
                            {
                                key.ResizeKeyboard = true;
                            }
                            return keys;
                        }
                    case KeyboardType.Inline:
                        {
                            Dictionary<string, List<string>> savedPairs = new Dictionary<string, List<string>>();
                            using StreamReader reader = new StreamReader(inlineFileName);
                            savedPairs = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(reader.ReadToEnd());
                            reader.Close();
                            Dictionary<string, InlineKeyboardMarkup> keys = new Dictionary<string, InlineKeyboardMarkup>();

                            foreach (KeyValuePair<string, List<string>> pair in savedPairs)
                            {
                                int currentRow = 1;
                                int currentCol = 1;

                                List<List<InlineKeyboardButton>> rows = new List<List<InlineKeyboardButton>>();
                                List<InlineKeyboardButton> collumns = new List<InlineKeyboardButton>();

                                foreach (string item in pair.Value)
                                {
                                    string[] parts = item.Split(':');
                                    int[] position = parts[0].Split('.').Select(p => int.Parse(p)).ToArray();

                                    if (position[0] != currentRow)
                                    {
                                        currentRow++;
                                        currentCol = 1;

                                        rows.Add(collumns.ToList());
                                        collumns.Clear();
                                    }

                                    collumns.Add(new InlineKeyboardButton(parts[1]) { CallbackData = parts[2] });
                                    currentCol++;
                                }

                                rows.Add(collumns.ToList());
                                collumns.Clear();

                                keys.Add(pair.Key, new InlineKeyboardMarkup(rows.ToList()));
                                rows.Clear();
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

        public Dictionary<string, ReplyKeyboardMarkup> LoadOneRowKeyboards()
        {
            Dictionary<string, string> savedPairs = new Dictionary<string, string>();
            using StreamReader reader = new StreamReader(replyFileName);
            savedPairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(reader.ReadToEnd());
            reader.Close();
            Dictionary<string, ReplyKeyboardMarkup> keys = new Dictionary<string, ReplyKeyboardMarkup>();

            foreach (var pair in savedPairs)
            {
                List<List<KeyboardButton>> rows = new List<List<KeyboardButton>>();
                List<KeyboardButton> collumns = new List<KeyboardButton>();

                string[] rows_array = pair.Value.Split("{END()}").Where(r => !string.IsNullOrEmpty(r)).ToArray();
                foreach (var row in rows_array)
                {
                    string[] collums_array = row.Split("{").Where(r => !string.IsNullOrEmpty(r)).ToArray();
                    for (int i = 0; i < collums_array.Length; i++)
                    {
                        collums_array[i] = collums_array[i].Remove(collums_array[i].Length - 1);
                    }

                    foreach (var item in collums_array)
                    {
                        collumns.Add(new KeyboardButton(item));
                    }

                    rows.Add(collumns.ToList());
                    collumns.Clear();
                }

                keys.Add(pair.Key, new ReplyKeyboardMarkup(rows.ToList()));
                rows.Clear();
            }

            foreach (var key in keys.Values)
            {
                key.ResizeKeyboard = true;
            }
            return keys;
        }

        public async Task<object> LoadKeyboardsAsync(KeyboardType keyboardType)
        {
            try
            {
                switch (keyboardType)
                {
                    case KeyboardType.Reply:
                        {
                            Dictionary<string, List<string>> savedPairs = new Dictionary<string, List<string>>();
                            using StreamReader reader = new StreamReader(replyFileName);
                            savedPairs = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(await reader.ReadToEndAsync());
                            reader.Close();
                            Dictionary<string, ReplyKeyboardMarkup> keys = new Dictionary<string, ReplyKeyboardMarkup>();

                            foreach (KeyValuePair<string, List<string>> pair in savedPairs)
                            {
                                int currentRow = 1;
                                int currentCol = 1;

                                List<List<KeyboardButton>> rows = new List<List<KeyboardButton>>();
                                List<KeyboardButton> collumns = new List<KeyboardButton>();

                                foreach (string item in pair.Value)
                                {
                                    string[] parts = item.Split(':');
                                    int[] position = parts[0].Split('.').Select(p => int.Parse(p)).ToArray();

                                    if (position[0] != currentRow)
                                    {
                                        currentRow++;
                                        currentCol = 1;

                                        rows.Add(collumns.ToList());
                                        collumns.Clear();
                                    }

                                    collumns.Add(new KeyboardButton(parts[1]));
                                    currentCol++;
                                }

                                rows.Add(collumns.ToList());
                                collumns.Clear();

                                keys.Add(pair.Key, new ReplyKeyboardMarkup(rows.ToList()));
                                rows.Clear();
                            }

                            foreach (var key in keys.Values)
                            {
                                key.ResizeKeyboard = true;
                            }
                            return keys;
                        }
                    case KeyboardType.Inline:
                        {
                            Dictionary<string, List<string>> savedPairs = new Dictionary<string, List<string>>();
                            using StreamReader reader = new StreamReader(inlineFileName);
                            savedPairs = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(await reader.ReadToEndAsync());
                            reader.Close();
                            Dictionary<string, InlineKeyboardMarkup> keys = new Dictionary<string, InlineKeyboardMarkup>();

                            foreach (KeyValuePair<string, List<string>> pair in savedPairs)
                            {
                                int currentRow = 1;
                                int currentCol = 1;

                                List<List<InlineKeyboardButton>> rows = new List<List<InlineKeyboardButton>>();
                                List<InlineKeyboardButton> collumns = new List<InlineKeyboardButton>();

                                foreach (string item in pair.Value)
                                {
                                    string[] parts = item.Split(':');
                                    int[] position = parts[0].Split('.').Select(p => int.Parse(p)).ToArray();

                                    if (position[0] != currentRow)
                                    {
                                        currentRow++;
                                        currentCol = 1;

                                        rows.Add(collumns.ToList());
                                        collumns.Clear();
                                    }

                                    collumns.Add(new InlineKeyboardButton(parts[1]) { CallbackData = parts[2] });
                                    currentCol++;
                                }

                                rows.Add(collumns.ToList());
                                collumns.Clear();

                                keys.Add(pair.Key, new InlineKeyboardMarkup(rows.ToList()));
                                rows.Clear();
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

        public InlineKeyboardMarkup GenerateInlineKeyboard(List<Tuple<string, string>> tupleTextData)
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
            return new InlineKeyboardMarkup(rows);
        }

        public InlineKeyboardMarkup GeneratePagedInlineKeyboard(List<Tuple<string, string>> tupleTextData,
                                                                int pageNumber,
                                                                int countInPage,
                                                                int collums,
                                                                Tuple<string, string> backButton,
                                                                Tuple<string, string> forwardButton)
        {
            List<List<InlineKeyboardButton>> rows = new List<List<InlineKeyboardButton>>();

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

            List<InlineKeyboardButton> singleRow = new List<InlineKeyboardButton>();
            int elementCounter = 0;
            int collumnCounter = 0;
            foreach (var item in tupleTextData.Skip((pageNumber - 1) * countInPage))
            {
                singleRow.Add(new InlineKeyboardButton(item.Item1)
                {
                    CallbackData = item.Item2
                });
                collumnCounter++;
                elementCounter++;
                if (collumnCounter >= collums)
                {
                    rows.Add(singleRow.ToList());
                    singleRow = new List<InlineKeyboardButton>();
                    collumnCounter = 0;
                }
                if (elementCounter >= countInPage)
                {
                    break;
                }
            }
            if (singleRow.Any())
            {
                rows.Add(singleRow.ToList());
            }

            if (pageMax != 1)
            {
                if (pageNumber == 1)
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
                else if (pageNumber < pageMax)
                {
                    List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton(backButton.Item1) { CallbackData = $"{backButton.Item2}" },
                    new InlineKeyboardButton(forwardButton.Item1) { CallbackData = $"{forwardButton.Item2}" }
                };
                    rows.Add(buttons);
                }
            }

            return new InlineKeyboardMarkup(rows);
        }
    }

    public enum KeyboardType
    { Reply, Inline }
}