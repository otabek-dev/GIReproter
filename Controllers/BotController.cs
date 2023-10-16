using HisoBOT.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HisoBOT.Controllers
{
    [Route("/")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly ITelegramBotClient _botClient;

        public BotController(

            ITelegramBotClient botClient
            )
        {
            _botClient = botClient;
        }

        [HttpPost]
        public async Task Post(
            [FromBody] Update update,
            [FromServices] UpdateHandlers handleUpdateService,
            CancellationToken cancellationToken)
        {
            string json = JsonConvert.SerializeObject(update.MyChatMember?.NewChatMember.Status);
            var chatId = update.MyChatMember?.Chat.Id;
            _botClient?.LeaveChatAsync(chatId);
            await Console.Out.WriteLineAsync(json);

            await handleUpdateService.HandleUpdateAsync(update, cancellationToken);
        }
    }
}
