using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace HisoBOT.Controllers
{
    [Route("/")]
    [ApiController]
    public class BotController : ControllerBase
    {
        [HttpPost]
        public async Task Post(Update update)
        {
            string json = JsonConvert.SerializeObject(update);
            await Console.Out.WriteLineAsync(json.ToString());
        }
    }
}
