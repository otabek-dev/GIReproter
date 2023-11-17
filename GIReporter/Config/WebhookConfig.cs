using Telegram.Bot.Types.Enums;
using Telegram.Bot;

namespace GIReporter.Config
{
    public class WebhookConfig : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _botConfig;
        //private readonly ITelegramBotClient _botClient;

        public WebhookConfig(
            IServiceProvider serviceProvider,
            IConfiguration configuration
            //ITelegramBotClient telegramBot
            )
        {
            _serviceProvider = serviceProvider;
            _botConfig = configuration;
            //_botClient = telegramBot;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
            await botClient.SetWebhookAsync(
                url: _botConfig["HostAddress"],
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
            await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}
