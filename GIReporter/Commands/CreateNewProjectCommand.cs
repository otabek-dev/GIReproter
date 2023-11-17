﻿using GIReporter.Commands.Interfaces;
using GIReporter.Models;
using GIReporter.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GIReporter.Commands
{
    public class CreateNewProjectCommand : ICommand
    {
        private readonly UserService _userService;
        private readonly ITelegramBotClient _botClient;
        private readonly ProjectService _projectService;
        public CreateNewProjectCommand(
            ITelegramBotClient botClient, 
            UserService userService,
            ProjectService projectService)
        {
            _userService = userService;
            _botClient = botClient;
            _projectService = projectService;
        }

        public string Name => "/createNewProject";

        public State State => State.CreateProject;

        public async Task Execute(Update update)
        {
            var message = update.Message;
            string userIdText = $"Пришлите chatId и название проекта в таком формате:\n\nchatId:название_проекта";

            await _userService.SetUserState(message.From.Id, State.CreateProject);

            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: userIdText);
        }

        public async Task GetUpdate(Update update)
        {
            var message = update.Message;
            string userIdText = $"*User id =* `{message.From?.Id}` \n\rКоманда не найдена";
            var userId = message.From.Id;

            if (_userService.GetUserState(userId) == State.CreateProject)
            {
                await CreateProject(message);
                return;
            }

            await _botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: userIdText,
                    parseMode: ParseMode.Markdown);
        }

        private async Task<Message> CreateProject(Message message)
        {
            var chatIdAndProjectName = message.Text.Trim();
            string[] parts = chatIdAndProjectName.Split(':');

            if (parts.Length == 2)
            {
                string chatId = parts[0];
                string projectName = parts[1];

                var result = _projectService.CreateProject(chatId, projectName);
                if (result.Success is true)
                {
                    await _userService.SetUserState(message.From.Id, State.All);
                    return await _botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: result.Message,
                        parseMode: ParseMode.Markdown);
                }
                else
                {
                    return await _botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: result.Message,
                        parseMode: ParseMode.Markdown);
                }
            }

            return await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Не верный формат для создания проекта. Введите заново!",
                parseMode: ParseMode.Markdown);
        }
    }
}