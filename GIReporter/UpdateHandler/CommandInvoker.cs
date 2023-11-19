using GIReporter.Commands;
using GIReporter.Commands.Interfaces;
using GIReporter.Models;
using GIReporter.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GIReporter.UpdateHandler
{
    public class CommandInvoker : ITelegramUpdateListener
    {
        private Dictionary<string, ICommand> MyCommands { get; init; }
        private Dictionary<State, ICommand> States { get; init; }
        private readonly UserService _userService;

        public CommandInvoker(
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
                { "Удалить проект", new DeletePorjectCommand(botClient,userService,projectService) }
            };

            foreach (var command in MyCommands)
            {
                if (command.Value.State != State.All)
                    States.Add(command.Value.State, command.Value);
            }
        }

        public async Task CommandExexute(Update update)
        {
            var userId = update.Message.From.Id;
            var userState = _userService.GetUserState(userId);
            ICommand? command;

            if (MyCommands.TryGetValue(update.Message.Text, out command))
                if (command.State == State.Start)
                {
                    await command.Execute(update);
                    return;
                }

            if (userState == State.All 
                && MyCommands.TryGetValue(update.Message.Text, out command))
            {
                await command.Execute(update);
                return;
            }

            if (States.TryGetValue(userState, out command))
            {
                await command.GetUpdate(update);
                return;
            }
        }
    }
}
