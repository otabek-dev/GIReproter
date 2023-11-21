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
    public class MyProjectsCommand : ICommand
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ProjectService _projectService;

        public MyProjectsCommand(ITelegramBotClient botClient, ProjectService projectService)
        {
            _botClient = botClient;
            _projectService = projectService;
        }

        public string CommandName => "/myprojects";

        public string Description => "get my projects";

        public async Task Execute(Message message)
        {
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

        public Task GetUpdate(Message message)
        {
            return Task.CompletedTask;
        }
    }
}
