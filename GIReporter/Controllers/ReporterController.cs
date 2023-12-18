using GIReporter.DTOs;
using GIReporter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        [HttpGet]
        public string Get()
        {
            string remoteIpAddress = string.Empty;
            if (HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
            {
                remoteIpAddress = forwardedFor.FirstOrDefault();
            }
            else
            {
                remoteIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            }

            return remoteIpAddress;
        }

        [HttpPost]
        public async Task Post([FromBody] ReporterDTO hisobot)
        {
            await _hisobotService.SendReportAsync(hisobot.Info, hisobot.ProjectName);
        }
    }
}
