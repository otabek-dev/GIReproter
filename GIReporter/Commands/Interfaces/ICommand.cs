using Telegram.Bot.Types;

namespace GIReporter.Commands.Interfaces
{
    public interface ICommand
    {
        public string CommandName { get; }
        public string Description { get; }
        public Task Execute(Message update);
        public Task GetUpdate(Message update);
    }
}
