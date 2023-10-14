using Microsoft.Extensions.Options;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;

namespace HisoBOT.Services
{
    public class ConfigureWebhook : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly ITelegramBotClient _botClient;

        public ConfigureWebhook(IConfiguration configuration, ITelegramBotClient botClient)
        {
            _configuration = configuration;
            _botClient = botClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _botClient.SetWebhookAsync(
                url: _configuration["WebhookURL"],
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}
