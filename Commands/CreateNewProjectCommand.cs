using HisoBOT.Commands.Interfaces;
using HisoBOT.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HisoBOT.Commands
{
    public class CreateNewProjectCommand : ICommand, IListener
    {
        private readonly UserService _userService;
        private readonly ITelegramBotClient _botClient;

        public CreateNewProjectCommand(ITelegramBotClient botClient, UserService userService)
        {
            _userService = userService;
            _botClient = botClient;
        }

        public string Name => "/createNewProject";

        public async Task Execute(Update update)
        {
            var message = update.Message;
            string userIdText = $"Пришлите chatId и название проекта в таком формате:\n\nchatId:название_проекта";

            await _userService.SetUserState(message.From.Id, Models.UserState.CreateProject);

            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: userIdText);
        }

        public async Task GetUpdate(Update update)
        {

        }
    }
}
