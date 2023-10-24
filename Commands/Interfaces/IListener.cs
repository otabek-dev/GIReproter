using Telegram.Bot.Types;

namespace HisoBOT.Commands.Interfaces
{
    public interface IListener
    {
        public Task GetUpdate(Update update);
    }
}
