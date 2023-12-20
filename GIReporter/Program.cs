using GIReporter.Config;
using Middleware;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose() 
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)  
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(new CompactJsonFormatter(), @"../reporter-logs/log-app.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.BotConfigure(builder.Configuration);
builder.Services.RegisterCommand(builder.Configuration);

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.Run();