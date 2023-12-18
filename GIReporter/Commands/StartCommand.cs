using GIReporter.Commands.Interfaces;
using GIReporter.Models;
using GIReporter.Services;
using GIReporter.States;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GIReporter.Commands
{
    [UserState(State.Any)]
    public class StartCommand : ICommand
    {
        private readonly ITelegramBotClient _botClient;
        private readonly UserService _userService;

        public StartCommand(ITelegramBotClient botClient, UserService userService)
        {
            _botClient = botClient;
            _userService = userService;
        }

        public string CommandName => "/start";
        public string Description => "start or restart bot";

        public async Task Execute(Message message)
        {
            string userIdText = $"Ваш user id = `{message.From.Id}`";
            await _userService.SetUserStateAsync(message.From.Id, State.Any);
            await _userService.SetInProcessCommand(message.From.Id, null);
            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: userIdText,
                parseMode: ParseMode.Markdown,
                replyMarkup: default);
        }

        public Task GetUpdate(Message message)
        {
            return Task.CompletedTask;
        }
    }
}
