using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workshop.BuildingBlocks.Extensions;

public static class DefaultServiceExtension
{

  public static IServiceCollection AddDefaultServices(this IServiceCollection services)
  {

    // services.AddAuthentication()

    services.AddSwaggerGen();
    services.AddEndpointsApiExplorer();

    services.AddRouting();
    services.AddControllers();



    return services;
  }

}
