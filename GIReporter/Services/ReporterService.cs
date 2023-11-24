using GIReporter.DB;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GIReporter.Services
{
    public class ReporterService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly AppDbContext _context;
        private readonly ProjectService _projectService;

        public ReporterService(ITelegramBotClient botClient, AppDbContext context, ProjectService projectService)
        {
            _botClient = botClient;
            _context = context;
            _projectService = projectService;
        }

        public async Task SendReportAsync(string info, string projectName)
        {
            var projects = await _context.Projects
                .Where(p => p.Name == projectName)
                .ToListAsync();

            foreach (var project in projects)
            {
                if (long.TryParse(project.ChatId, out var chatId))
                {
                    try
                    {
                        ChatMember chatMember = await _botClient.GetChatMemberAsync(chatId, 6441434094);

                        if (chatMember.Status == ChatMemberStatus.Member 
                            || chatMember.Status == ChatMemberStatus.Administrator)
                        {
                            await _botClient.SendTextMessageAsync(chatId, info);
                        }
                    }
                    catch (ApiRequestException ex)
                    {
                        if (ex.ErrorCode == 403)
                        {
                            _projectService.DeleteProject(chatId.ToString());
                        }
                        else
                        {
                            Console.WriteLine("Произошла ошибка: " + ex.Message);
                        }
                    }
                }
            }
        }
    }
}
