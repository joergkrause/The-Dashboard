namespace TheDashboard.BuildingBlocks.Extensions;

public static class DefaultServiceExtension
{

  public static IServiceCollection AddDefaultServices(this IServiceCollection services)
  {

    services.AddEndpointsApiExplorer();

    services.AddRouting();
    services.AddControllers();



    return services;
  }

}
