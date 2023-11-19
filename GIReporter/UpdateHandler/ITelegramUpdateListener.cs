using Telegram.Bot.Types;

namespace GIReporter.UpdateHandler
{
    public interface ITelegramUpdateListener
    {
        public async Task CommandExexute(Update update) { }
    }
}