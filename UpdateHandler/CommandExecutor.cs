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
        private readonly UserService _userService;

        public CommandExecutor(
            ITelegramBotClient botClient,
            UserService userService,
            ProjectService projectService)
        {
            _userService = userService;
            MyCommands = new Dictionary<string, ICommand>
            {
                { "/start", new StartCommand(botClient, userService) },
                { "Мои проекты", new MyProjectsCommand(botClient, projectService) },
                { "Добавить проект", new CreateNewProjectCommand(botClient, userService, projectService) },
            };
        }

        public async Task GetUpdate(Update update)
        {
            var userId = update.Message.From.Id;
            var userState = _userService.GetUserState(userId);

            if (userState == Models.UserState.None 
                && MyCommands.TryGetValue(update.Message.Text, out var command))
            {
                await command.Execute(update);
            }

            if (userState == Models.UserState.CreateProject)
            {
                await MyCommands["Добавить проект"].GetUpdate(update);
            }
        }
    }
}
