using HisoBOT.Commands;
using HisoBOT.Commands.Interfaces;
using HisoBOT.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HisoBOT.UpdateHandler
{
    public class CommandExecutor : ITelegramUpdateListener
    {
        private Dictionary<string, ICommand> MyCommands { get; init; }

        public CommandExecutor(
            ITelegramBotClient botClient,
            UserService userService,
            ProjectService projectService)
        {
            MyCommands = new Dictionary<string, ICommand>
            {
                { "/start", new StartCommand(botClient, userService) },
                { "Мои проекты", new MyProjectsCommand(botClient, projectService) }
            };
        }

        public async Task GetUpdate(Update update)
        {
            if (MyCommands.TryGetValue(update.Message.Text, out var command))
            {
                await command.Execute(update);
            }
        }
    }
}
