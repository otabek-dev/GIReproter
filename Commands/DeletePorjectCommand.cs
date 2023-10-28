using HisoBOT.Commands.Interfaces;
using HisoBOT.Models;
using HisoBOT.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HisoBOT.Commands
{
    public class DeletePorjectCommand : ICommand
    {
        public string Name => "/deleteProject";
        public static string CommandName => "/deleteProject";
        public State State => State.DeleteProject;
        
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

        public async Task Execute(Update update)
        {
            var message = update.Message;
            string text = $"Отправьте мне chat id проекта которую хотите удалить.";
            await _userService.SetUserState(message.From.Id, State.DeleteProject);
            await _botClient.SendTextMessageAsync(message.Chat.Id, text);
        }

        public async Task GetUpdate(Update update)
        {
            var message = update.Message;
            var result = _projectService.DeleteProject(update.Message.Text);
            
            if (!result.Success)
            {
                await _botClient.SendTextMessageAsync(message.Chat.Id, result.Message, parseMode: ParseMode.Markdown);
                return;
            }

            await _botClient.SendTextMessageAsync(message.Chat.Id, result.Message, parseMode: ParseMode.Markdown);
            await _userService.SetUserState(message.From.Id, State.All);
        }
    }
}
