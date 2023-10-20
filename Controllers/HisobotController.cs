using HisoBOT.DTOs;
using HisoBOT.Services;
using Microsoft.AspNetCore.Mvc;

namespace HisoBOT.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class HisobotController : ControllerBase
    {
        private readonly HisobotService _hisobotService;

        public HisobotController(HisobotService hisobotService)
        {
            _hisobotService = hisobotService;
        }

        [HttpPost]
        public async Task Post([FromBody] HisobotDTO hisobot)
        {
            await _hisobotService.SendHisobot(hisobot.Info, hisobot.ProjectName);
        }
    }
}
