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

  public static IServiceCollection AddEventbus<T, C>(this IServiceCollection services, IConfiguration configuration, string serviceName, bool receiveOnly = false)
    where T : DbContext
    where C : IConsumer
  {
    services.AddMassTransit(x =>
    {
      if (!receiveOnly)
      {
        x.AddEntityFrameworkOutbox<T>(options =>
        {
          options.UseSqlServer();
          options.UseBusOutbox();
        });
      }
      x.AddConsumers(typeof(T).Assembly);
      x.AddConsumersFromNamespaceContaining<C>();
      x.UsingRabbitMq((context, cfg) =>
      {
        // rabbitmq://
        cfg.Host($"{configuration["RabbitMq:Host"]}", "/", h =>
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

  public static IServiceCollection AddEventbus(this IServiceCollection services, IConfiguration configuration)    
  {
    services.AddMassTransit(x =>
    {
      x.UsingRabbitMq((context, cfg) =>
      {
        // rabbitmq://
        cfg.Host($"{configuration["RabbitMq:Host"]}", "/", h =>
        {
          h.Username(configuration["RabbitMq:User"]);
          h.Password(configuration["RabbitMq:Password"]);
        });
      });
    });
    return services;
  }

}
