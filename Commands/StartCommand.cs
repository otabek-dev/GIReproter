﻿using HisoBOT.Commands.Interfaces;
using HisoBOT.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace HisoBOT.Commands
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

        public async Task Execute(Update update)
        {
            var message = update.Message;
            string userIdText = $"Genesis hisobot вас приветсвует!\n\rВаш user id = `{message.From?.Id}`";
            await _userService.SetIsTypeProjectName(message.From.Id, false);

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
    }
}
