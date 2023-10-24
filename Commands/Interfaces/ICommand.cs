using Telegram.Bot;
using Telegram.Bot.Types;

namespace HisoBOT.Commands.Interfaces
{
    public interface ICommand
    {
        public string Name { get; }
        public async Task Execute(Update update) { }
    }
}
