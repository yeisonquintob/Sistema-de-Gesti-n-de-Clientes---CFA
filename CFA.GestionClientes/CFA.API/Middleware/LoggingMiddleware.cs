using System.Diagnostics;

namespace CFA.API.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestTime = DateTime.UtcNow;

        try
        {
            await LogRequest(context);
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            await LogResponse(context, stopwatch.ElapsedMilliseconds, requestTime);
        }
    }

    private async Task LogRequest(HttpContext context)
    {
        context.Request.EnableBuffering();

        var requestBody = string.Empty;
        if (context.Request.ContentLength > 0)
        {
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        _logger.LogInformation(
            "Request {Method} {Path} received at {Time}\nHeaders: {@Headers}\nBody: {Body}",
            context.Request.Method,
            context.Request.Path,
            DateTime.UtcNow,
            context.Request.Headers,
            requestBody);
    }

    private Task LogResponse(HttpContext context, long elapsedMilliseconds, DateTime requestTime)
    {
        _logger.LogInformation(
            "Response {StatusCode} for {Method} {Path}\nProcessing Time: {ElapsedMilliseconds}ms\nRequest Time: {RequestTime}",
            context.Response.StatusCode,
            context.Request.Method,
            context.Request.Path,
            elapsedMilliseconds,
            requestTime);

        return Task.CompletedTask;
    }
}