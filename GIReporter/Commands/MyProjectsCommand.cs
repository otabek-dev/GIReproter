using GIReporter.Commands.Interfaces;
using GIReporter.Models;
using GIReporter.Services;
using GIReporter.States;
using System.Text;
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
                await _botClient.SendTextMessageAsync(message.Chat.Id, text: "Проектов не найдено!");
                return;
            }

            var projectStrings = new StringBuilder();
            var botId = _botClient.GetMeAsync().Result.Id;

            foreach (var p in projects)
            {
                try
                {
                    var chat = await _botClient.GetChatAsync(p.ChatId!);
                    var info = chat.Title ?? chat.FirstName ?? chat.Username;
                    var isChatMemberBot = await _botClient.GetChatMemberAsync(p.ChatId, botId);
                    await Console.Out.WriteLineAsync(isChatMemberBot.Status.ToString());
                    projectStrings.AppendLine($"({info}) ` {p.ChatId}:{p.Name} `");

                    await Console.Out.WriteLineAsync(projectStrings);
                }
                catch (Exception e)
                {
                    _projectService.DeleteProject(p.ChatId!);
                }
            }

            await _botClient.SendTextMessageAsync(
                       chatId: message.Chat.Id,
                       text: "Ваши проекты: \n\r" + projectStrings,
                       parseMode: ParseMode.Markdown);
        }

        public Task GetUpdate(Message message)
        {
            return Task.CompletedTask;
        }
    }
}
