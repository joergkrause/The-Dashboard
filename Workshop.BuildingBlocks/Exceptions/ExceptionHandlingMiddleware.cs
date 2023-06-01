using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;

[assembly: InternalsVisibleTo("BuildingBlocksTests")]

namespace Workshop.BuildingBlocks.Exceptions;

public class ExceptionHandlingMiddleware
{
  private readonly RequestDelegate next;
  private readonly ILogger<ExceptionHandlingMiddleware> logger;

  //todo Translate
  [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Needs to be Internal, for Testing")]
  internal readonly string DefaultMessage = "Es ist ein unerwarteter Fehler aufgetreten, bitte versuchen Sie es noch einmal. Falls der Fehler bestehen bleibt, melden Sie sich bitte beim Support.";

  public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
  {
    this.next = next;
    this.logger = logger;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await next(context);
    }
    catch (Exception ex)
    {
      await HandleExceptionsAsync(context, ex);
    }
  }

  internal async Task HandleExceptionsAsync(HttpContext context, Exception exception)
  {
    context.Response.ContentType = "application/json";
    var response = context.Response;

    //https://stackoverflow.com/questions/60474213/asp-net-core-healthchecks-randomly-fails-with-taskcanceledexception-or-operation
    if (context.RequestAborted.IsCancellationRequested)
    {
      logger.LogDebug("{ExceptionMessage}", exception.Message);
      return;
    }

    Type type = exception.GetType();
    if (type == typeof(ItemNotFoundException))
    {
      logger.LogWarning("{ExceptionMessage}", exception.Message);
      response.StatusCode = StatusCodes.Status404NotFound;
      await context.Response.WriteAsync(JsonSerializer.Serialize(exception.Message));
      return;
    }

    if (typeof(DomainException).IsAssignableFrom(type))
    {
      logger.LogWarning("{ExceptionMessage}", exception.Message);
      response.StatusCode = (int)HttpStatusCode.BadRequest;
      await context.Response.WriteAsync(JsonSerializer.Serialize(exception.Message));
      return;
    }

    logger.LogError("{ExceptionMessage}", exception.Message);
    response.StatusCode = (int)HttpStatusCode.InternalServerError;
    await context.Response.WriteAsync(JsonSerializer.Serialize(DefaultMessage));
  }
}
