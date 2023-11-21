using GIReporter.Commands.Interfaces;
using GIReporter.Models;
using GIReporter.Services;
using GIReporter.States;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GIReporter.Commands
{
    [UserState(State.DeleteProject)]
    public class DeletePorjectCommand : ICommand
    {
        private readonly UserService _userService;
        private readonly ProjectService _projectService;
        private readonly ITelegramBotClient _botClient;

        public DeletePorjectCommand(
            ITelegramBotClient telegramBotClient,
            UserService userService,
            ProjectService projectService)
        {
            _userService = userService;
            _projectService = projectService;
            _botClient = telegramBotClient;
        }

        public string CommandName => "/deleteproject";
        public string Description => "delete project";

        public async Task Execute(Message message)
        {
            string text = $"Отправьте мне chat id проекта которую хотите удалить.";
            await _userService.SetUserStateAsync(message.From.Id, State.DeleteProject);
            await _userService.SetInProcessCommand(message.From.Id, CommandName);
            await _botClient.SendTextMessageAsync(message.Chat.Id, text);
        }

        public async Task GetUpdate(Message message)
        {
            var result = _projectService.DeleteProject(message.Text);

            if (!result.Success)
            {
                await _botClient.SendTextMessageAsync(message.Chat.Id, result.Message, parseMode: ParseMode.Markdown);
                return;
            }

            await _botClient.SendTextMessageAsync(message.Chat.Id, result.Message, parseMode: ParseMode.Markdown);
            await _userService.SetUserStateAsync(message.From.Id, State.Any);
            await _userService.SetInProcessCommand(message.From.Id, null);

        }
    }
}
