using NUnit.Framework;
using System.Threading.Tasks;
using TelegramSimpleService;

namespace ServiceTests
{
    public class CheckBotToken
    {
        private static string botToken;
        private static ITelegramService telegramService;

        [SetUp]
        public void Setup()
        {
            botToken = "YOUR_TOKEN";
            telegramService = new TelegramService();
        }

        [Test]
        public async Task InitAsync()
        {
            bool isCorrect = await telegramService.CheckBotToken(botToken);
            Assert.IsTrue(isCorrect);
        }
    }
}