using GIReporter.Commands.Interfaces;
using GIReporter.DB;
using GIReporter.Services;
using GIReporter.UpdateHandler;
using System.Reflection;
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
        services.AddScoped<ProjectService>();
        services.AddScoped<UpdateHandlers>();
        services.AddScoped<CommandInvoker>();
        services.AddScoped<ReporterService>();
        services.AddDbContext<AppDbContext>();
        services.AddHostedService<WebhookConfig>();
    }

    public static void RegisterCommand(this IServiceCollection services, IConfiguration configuration)
    {
        var commandInterfaceType = typeof(ICommand);
        var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => commandInterfaceType.IsAssignableFrom(type))
                .Where(type => type.IsClass);

        if (types.Any())
            foreach (var type in types)
                services.AddTransient(commandInterfaceType, type);
        else Console.WriteLine("Command not found!");
    }
}