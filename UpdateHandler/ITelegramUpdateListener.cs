using Telegram.Bot.Types;

namespace HisoBOT.UpdateHandler
{
    public interface ITelegramUpdateListener
    {
        public async Task GetUpdate(Update update) { }
    }
}