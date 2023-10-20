using HisoBOT.DB;
using Telegram.Bot;

namespace HisoBOT.Services
{
    public class HisobotService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly AppDbContext _context;

        public HisobotService(ITelegramBotClient botClient, AppDbContext context)
        {
            _botClient = botClient;
            _context = context;
        }

        public async Task SendHisobot(string info, string projectName)
        {
            var projects = _context.Projects
                .Where(pn => pn.Name == projectName)
                .ToList();

            long chatId;

            foreach (var project in projects)
            {
                if (long.TryParse(project.ChatId, out chatId))
                {
                    await _botClient.SendTextMessageAsync(chatId, info);
                }
            }
        }
    }
}
