using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workshop.BuildingBlocks.Exceptions;

namespace Workshop.BuildingBlocks.Extensions;

public static class WebAppExtension
{
  public static IApplicationBuilder UseDefaultConfiguration(this IApplicationBuilder app)
  {
    var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseHealthChecks("/health");

    app.UseCors("");
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    // app.UseMetricServer(); // Prometheus
    // app.UseHttpMetrics(); // Prometheus

    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers();
    });

    // app.UseSerilog();

    return app;
  }

}
