using System.Text;
using System.Linq;

namespace TheDashboard.Proxy.Middleware;

public class HttpInspectorMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public HttpInspectorMiddleware(RequestDelegate next,
                                   ILogger<HttpInspectorMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        LogRequest(context);
        await _next(context);
    }

    private void LogRequest(HttpContext context)
    {
        var buffer = new StringBuilder();

        context.Request.Headers
            .ToList()
            .ForEach(kvp => buffer.Append($"{kvp.Key}: {kvp.Value}{Environment.NewLine}"));

        _logger.LogDebug($"Http Request Information:{Environment.NewLine}" +
                               $"Schema: {context.Request.Scheme}{Environment.NewLine}" +
                               $"Host: {context.Request.Host}{Environment.NewLine}" +
                               $"Path: {context.Request.Path}{Environment.NewLine}" +
                               $"QueryString: {context.Request.QueryString}{Environment.NewLine}" +
                               $"Headers: {Environment.NewLine}{buffer}");
    }
}