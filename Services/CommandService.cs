using HisoBOT.Commands;
using HisoBOT.Commands.Interfaces;
using Telegram.Bot;

namespace HisoBOT.Services
{
    public class CommandService
    {
        public Dictionary<string, ICommand> MyCommands { get; init; }

        public CommandService(ITelegramBotClient botClient, UserService userService, ProjectService projectService)
        {
            MyCommands = new Dictionary<string, ICommand>
            {
                { "/start", new StartCommand(botClient, userService) },
                { "Мои проекты", new MyProjectsCommand(botClient, projectService) }
            };
        }
    }
}
