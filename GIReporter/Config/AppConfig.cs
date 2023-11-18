using GIReporter.DB;
using GIReporter.Services;
using GIReporter.UpdateHandler;
using Telegram.Bot;

namespace GIReporter.Config;

public static class AppConfig
{
    public static void BotConfigure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ITelegramBotClient>(x =>
        {
            return new TelegramBotClient(configuration["BotToken"]);
        });

        services.AddScoped<UserService>();
        services.AddScoped<ReporterService>();
        services.AddScoped<ProjectService>();
        services.AddScoped<UpdateHandlers>();
        services.AddScoped<CommandExecutor>();
        services.AddDbContext<AppDbContext>();
        services.AddHostedService<WebhookConfig>();
    }
}

