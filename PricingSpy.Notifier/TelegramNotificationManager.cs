using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PricingSpy.Notifier
{
    public class TelegramNotificationManager : INotificationManager
    {
        private readonly string _chatId;
        private readonly string _token;

        public TelegramNotificationManager(string chatId, string token)
        {
            _chatId = chatId;
            _token = token;
        }

        public async Task SendMessage(string message)
        {
            TelegramBotClient bot = new TelegramBotClient(_token);
            await bot.SendTextMessageAsync(new ChatId(_chatId), message);
        }
    }
}
