using System.Text.Json;
using WebApplication1.Abstraction.Common.Exceptions;

namespace WebApplication1.Common.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ILogger logger)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (ex is not IErrorVmProvider errorVmProvider)
            {
                await _next(context);
                return;
            }

            await RespondJson(context, errorVmProvider);
        }
    }

    private async Task RespondJson(HttpContext context, IErrorVmProvider errorVmProvider)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = errorVmProvider.GetHttpStatusCode();
        await response.WriteAsync(JsonSerializer.Serialize(errorVmProvider.GetErrorVm()));
    }
}