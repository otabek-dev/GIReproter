using HisoBOT.Services;
using Telegram.Bot;

namespace HisoBOT.Config
{
    public static class BotConfig
    {
        public static void BotConfigure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(configuration["BotToken"]));
            services.AddHostedService<ConfigureWebhook>();
        }
    }
}
