﻿using HisoBOT.Services;
using Telegram.Bot;

namespace HisoBOT.Config
{
    public static class BotConfig
    {
        public static void BotConfigure(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddHttpClient("telegram_bot_client")
                    .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                    {
                        TelegramBotClientOptions options = new(configuration["BotToken"]);
                        return new TelegramBotClient(options, httpClient);
                    });

            services.AddHostedService<ConfigureWebhook>();
        }
    }
}