using GIReporter.Commands.Interfaces;
using GIReporter.Models;
using GIReporter.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace GIReporter.Commands
{
    public class StartCommand : ICommand
    {
        private readonly ITelegramBotClient _botClient;
        private readonly UserService _userService;

        public StartCommand(ITelegramBotClient botClient, UserService userService)
        {
            _botClient = botClient;
            _userService = userService;
        }

        public string Name => "/start";

        public State State => State.Start;

        public async Task Execute(Update update)
        {
            var message = update.Message;
            string userIdText = $"Genesis hisobot вас приветсвует!\n\rВаш user id = `{message.From?.Id}`";
            await _userService.SetUserState(message.From.Id, State.All);

            var buttons = new ReplyKeyboardMarkup(
                new[]
                {
                    new[]
                    {
                        new KeyboardButton("Добавить проект"),
                        new KeyboardButton("Удалить проект")
                    },
                    new[]
                    {
                        new KeyboardButton("Мои проекты")
                    }
                });

            buttons.ResizeKeyboard = true;

            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: userIdText,
                parseMode: ParseMode.Markdown,
                replyMarkup: buttons);
        }

        public Task GetUpdate(Update update)
        {
            return Task.CompletedTask;
        }
    }
}
