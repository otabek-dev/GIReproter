using HisoBOT.UpdateHandler;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace HisoBOT.Controllers
{
    [Route("/")]
    [ApiController]
    public class BotController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Telegram bot was started";
        }

        [HttpPost]
        public async Task Post(
            [FromBody] Update update,
            [FromServices] UpdateHandlers handleUpdateService,
            CancellationToken cancellationToken)
        {
            await handleUpdateService.HandleUpdateAsync(update, cancellationToken);
        }
    }
}
