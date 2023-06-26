using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDashboard.BuildingBlocks.Extensions;

public static class EventBusExtension
{

  public static IServiceCollection AddEventbus<T>(this IServiceCollection services, IConfiguration configuration, string serviceName)
    where T: DbContext
  {
    services.AddMassTransit(x =>
    {
      x.AddEntityFrameworkOutbox<T>(options =>
      {
        options.UseSqlServer();
        options.UseBusOutbox();
      });

      x.AddConsumersFromNamespaceContaining<T>();
      x.UsingRabbitMq((context, cfg) =>
      {
        cfg.Host(configuration["RabbitMq:Host"], "/", h =>
        {
          h.Username(configuration["RabbitMq:User"]);
          h.Password(configuration["RabbitMq:Password"]);
        });
        cfg.ReceiveEndpoint(serviceName, e => e.ConfigureConsumers(context));
      });
    });
    // services.AddMassTransitHostedService();
    return services;
  }

}
