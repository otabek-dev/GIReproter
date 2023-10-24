using HisoBOT.UpdateHandler;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace HisoBOT.Controllers
{
    [Route("/")]
    [ApiController]
    public class BotController : ControllerBase
    {
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
