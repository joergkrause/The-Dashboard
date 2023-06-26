namespace TheDashboard.BuildingBlocks.Controllers.Middleware;

public class UserInfoMiddleware : IMiddleware
{
  public async Task InvokeAsync(HttpContext context, RequestDelegate next)
  {
    // TODO: context.User.Claims.ToList().ForEach(c => Console.WriteLine($"{c.Type} : {c.Value}"));
    
    await next(context);
  }
}
