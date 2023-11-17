using Telegram.Bot.Types;

namespace GIReporter.UpdateHandler
{
    public interface ITelegramUpdateListener
    {
        public async Task GetUpdate(Update update) { }
    }
}