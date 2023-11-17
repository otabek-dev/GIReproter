﻿using GIReporter.Commands.Interfaces;
using GIReporter.Models;
using GIReporter.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GIReporter.Commands
{
    public class MyProjectsCommand : ICommand
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ProjectService _projectService;

        public MyProjectsCommand(ITelegramBotClient botClient, ProjectService projectService)
        {
            _botClient = botClient;
            _projectService = projectService;
        }

        public string Name => "/myProjects";

        public State State => State.All;

        public async Task Execute(Update update)
        {
            var message = update.Message;
            var projects = _projectService.GetAllProjects();

            if (!projects.Any())
            {
                await _botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Проектов не найдено!");
            }

            var projectStrings = projects.Select(p => $"`{p.ChatId}:{p.Name}`");
            string projectsAsString = string.Join(Environment.NewLine, projectStrings);

            await _botClient.SendTextMessageAsync(
                       chatId: message.Chat.Id,
                       text: "Ваши проекты:\n\r" + projectsAsString,
                       parseMode: ParseMode.Markdown);
        }

        public Task GetUpdate(Update update)
        {
            return Task.CompletedTask;
        }
    }
}