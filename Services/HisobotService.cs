using HisoBOT.DB;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HisoBOT.Services
{
    public class HisobotService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly AppDbContext _context;
        private readonly ProjectService _projectService;

        public HisobotService(ITelegramBotClient botClient, AppDbContext context, ProjectService projectService)
        {
            _botClient = botClient;
            _context = context;
            _projectService = projectService;
        }

        public async Task SendHisobot(string info, string projectName)
        {
            var projects = _context.Projects
                .Where(pn => pn.Name == projectName)
                .ToList();

            foreach (var project in projects)
            {
                if (long.TryParse(project.ChatId, out var chatId))
                {
                    try
                    {
                        ChatMember chatMember = await _botClient.GetChatMemberAsync(chatId, 6441434094);

                        if (chatMember.Status == ChatMemberStatus.Member 
                            || chatMember.Status == ChatMemberStatus.Administrator 
                            || chatMember.Status == ChatMemberStatus.Creator)
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
