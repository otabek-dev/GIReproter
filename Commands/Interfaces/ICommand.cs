using GIReporter.Models;
using Telegram.Bot.Types;

namespace GIReporter.Commands.Interfaces
{
    public interface ICommand
    {
        public static string CommandName { get; } = "None"; 
        public State State { get; }
        public Task Execute(Update update);
        public Task GetUpdate(Update update);
    }
}
