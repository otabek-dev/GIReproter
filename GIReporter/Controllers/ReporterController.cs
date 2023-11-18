using GIReporter.DTOs;
using GIReporter.Services;
using Microsoft.AspNetCore.Mvc;

namespace GIReporter.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ReporterController : ControllerBase
    {
        private readonly ReporterService _hisobotService;

        public ReporterController(ReporterService hisobotService)
        {
            _hisobotService = hisobotService;
        }

        [HttpPost]
        public async Task Post([FromBody] ReporterDTO hisobot)
        {
            await _hisobotService.SendHisobot(hisobot.Info, hisobot.ProjectName);
        }
    }
}
