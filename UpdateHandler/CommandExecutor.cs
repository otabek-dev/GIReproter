using HisoBOT.Commands;
using HisoBOT.Commands.Interfaces;
using HisoBOT.Models;
using HisoBOT.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HisoBOT.UpdateHandler
{
    public class CommandExecutor : ITelegramUpdateListener
    {
        private Dictionary<string, ICommand> MyCommands { get; init; }
        private Dictionary<State, ICommand> States { get; init; }
        
        private readonly UserService _userService;

        public CommandExecutor(
            ITelegramBotClient botClient,
            UserService userService,
            ProjectService projectService)
        {
            _userService = userService;
            States = new Dictionary<State, ICommand>();
            MyCommands = new Dictionary<string, ICommand>
            {
                { "/start", new StartCommand(botClient, userService) },
                { "Мои проекты", new MyProjectsCommand(botClient, projectService) },
                { "Добавить проект", new CreateNewProjectCommand(botClient, userService, projectService) },
            };

            foreach (var command in MyCommands)
            {
                if (command.Value.State != State.All)
                    States.Add(command.Value.State, command.Value);
            }
        }

        public async Task GetUpdate(Update update)
        {
            var userId = update.Message.From.Id;
            var userState = _userService.GetUserState(userId);

            if (MyCommands.TryGetValue(update.Message.Text, out var command))
            {
                if (command.State == State.Start)
                {
                    await command.Execute(update);
                    return;
                }

                await command.Execute(update);
            }

            if (States.TryGetValue(userState, out var command1))
            {
                await command1.GetUpdate(update);
            }
        }
    }
}
