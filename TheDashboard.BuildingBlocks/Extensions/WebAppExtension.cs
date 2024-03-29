﻿using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Validations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDashboard.BuildingBlocks.Exceptions;

namespace TheDashboard.BuildingBlocks.Extensions;

public static class WebAppExtension
{
  public static IApplicationBuilder UseDefaultConfiguration(this IApplicationBuilder app, string? corsConfig = null)
  {
    var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseHealthChecks("/health");

    app.UseRouting();
    app.UseHttpsRedirection();

    if (corsConfig != null)
    {
      app.UseCors(corsConfig);
    }

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
