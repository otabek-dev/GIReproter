using GIReporter.DTOs;
using Serilog;
using System.Net;
using System.Text.Json;

namespace Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string remoteIpAddress = string.Empty;
        try
        {
            if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
                remoteIpAddress = forwardedFor.FirstOrDefault();
            else
                remoteIpAddress = context.Connection.RemoteIpAddress?.ToString();

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            var requestBodyStream = new MemoryStream();
            var originalRequestBody = context.Request.Body;

            await context.Request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);
            var requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();

            Log.Information($"Request: {context.Request.Method} \n{context.Request.Path}\n {requestBodyText}");

            requestBodyStream.Seek(0, SeekOrigin.Begin);
            context.Request.Body = requestBodyStream;

            await _next(context);

            stopwatch.Stop();
            Log.Information($"Request completed in {stopwatch.ElapsedMilliseconds} ms");
            LogRequestToFile(new LogDTO()
            {
                IpAddress = remoteIpAddress,
                RequestMethod = context.Request.Method,
                RequestPath = context.Request.Path,
                RequestBodyText = requestBodyText.ToString() ?? "",
                StatusCode = context.Response.StatusCode.ToString(),
                Duration = $"{stopwatch.ElapsedMilliseconds} ms"
            });

            context.Request.Body = originalRequestBody;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error processing request: {ex.Message}");

            LogRequestToFile(new LogDTO()
            {
                IpAddress = remoteIpAddress,
                Exception = $"Exception:\nMessage: {ex.Message}\nInnerException: {ex.InnerException}\nStackTrace: {ex.StackTrace}",
                RequestMethod = context.Request.Method,
                RequestPath = context.Request.Path,
                StatusCode = context.Response.StatusCode.ToString(),
            });

            await HandleExceptionAsync(context, ex.Message, HttpStatusCode.InternalServerError, "Internal server error");
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, string exMsg, HttpStatusCode httpStatusCode, string message)
    {
        HttpResponse response = context.Response;

        response.ContentType = "application/json";
        response.StatusCode = (int)httpStatusCode;

        ErrorDTO errorDto = new()
        {
            ExceptionTittle = exMsg,
            StatusCode = (int)httpStatusCode,
            Message = message
        };

        await response.WriteAsJsonAsync(errorDto);
    }

    private void LogRequestToFile(object logMessage)
    {
        var logFilePath = @$"../reporter-logs/log-request{DateTime.Now:yyyyMMdd}.txt";
        var logEntry = JsonSerializer.Serialize(logMessage);
        logEntry = $"{logEntry}\n";
        File.AppendAllText(logFilePath, logEntry);
    }
}