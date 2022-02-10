using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramSimpleService
{
    public interface IKeyboardService
    {
        /// <summary>
        /// Method return filename for keyboard
        /// </summary>
        /// <param name="keyboardType">keyboard type from enum</param>
        /// <returns></returns>
        string GetFileName(KeyboardType keyboardType);

        /// <summary>
        /// Set name for keyboards data, as default when dont use this method names is<br></br>r_keys.json for reply keyboards<br></br>i_keys.json for inline keyboards
        /// </summary>
        /// <param name="replyKeyboards">new name for r_keys.json</param>
        /// <param name="inlineKeyboards">new name for i_keys.json</param>
        /// <returns>true when complete</returns>
        bool SetStoreFileName(string replyKeyboards, string inlineKeyboards);

        /// <summary>
        /// Method save reply keyboards to json file
        /// </summary>
        /// <param name="keyboards">dictionary with keyboards, key is name for keyboard</param>
        /// <returns>true when complete</returns>
        bool SaveKeyboards(Dictionary<string, ReplyKeyboardMarkup> keyboards);

        /// <summary>
        /// Method save reply keyboards to json file
        /// </summary>
        /// <param name="keyboards">dictionary with keyboards, key is name for keyboard</param>
        /// <returns>true when complete</returns>
        Task<bool> SaveKeyboardsAsync(Dictionary<string, ReplyKeyboardMarkup> keyboards);

        /// <summary>
        /// Method save inline keyboards to json file
        /// </summary>
        /// <param name="keyboards">dictionary with keyboards, key is name for keyboard</param>
        /// <returns>true when complete</returns>
        bool SaveKeyboards(Dictionary<string, InlineKeyboardMarkup> keyboards);

        /// <summary>
        /// Method save inline keyboards to json file
        /// </summary>
        /// <param name="keyboards">dictionary with keyboards, key is name for keyboard</param>
        /// <returns>true when complete</returns>
        Task<bool> SaveKeyboardsAsync(Dictionary<string, InlineKeyboardMarkup> keyboards);

        /// <summary>
        /// Method load keyboards from file
        /// </summary>
        /// <param name="keyboardType">keyboard type</param>
        /// <returns>Dictionary<string, ReplyKeyboardMarkup> or Dictionary<string, InlineKeyboardMarkup></returns>
        object LoadKeyboards(KeyboardType keyboardType);

        /// <summary>
        /// Method load keyboards from file
        /// </summary>
        /// <param name="keyboardType">keyboard type</param>
        /// <returns>Dictionary<string, ReplyKeyboardMarkup> or Dictionary<string, InlineKeyboardMarkup></returns>
        Task<object> LoadKeyboardsAsync(KeyboardType keyboardType);

        /// <summary>
        /// Generating inline keyboard with list of tuple
        /// </summary>
        /// <param name="tupleTextData">tuple element item1 is text for button item2 is callback data</param>
        /// <returns>Inline keyboard markup</returns>
        InlineKeyboardMarkup GenerateInlineKeyboard(List<Tuple<string, string>> tupleTextData);

        /// <summary>
        /// Generate paged list for big list collection
        /// </summary>
        /// <param name="tupleTextData">Data for pages</param>
        /// <param name="pageNumber">page number start with 1</param>
        /// <param name="countInPage">count elements on page</param>
        /// <param name="backButton">back button text and callback data</param>
        /// <param name="forwardButton">forward button text and callback data</param>
        /// <returns>inline keyboard button with pages</returns>
        InlineKeyboardMarkup GeneratePagedInlineKeyboard(List<Tuple<string, string>> tupleTextData,
                                                         int pageNumber,
                                                         int countInPage,
                                                         Tuple<string, string> backButton,
                                                         Tuple<string, string> forwardButton);
    }
}