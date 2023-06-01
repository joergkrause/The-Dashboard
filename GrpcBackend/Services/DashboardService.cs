using Grpc.Core;
using static GrpcBackend.DashboardReply.Types;

namespace GrpcBackend.Services;

public class DashboardService : Dashboard.DashboardBase
{

  private readonly List<DashboardAllReply.Types.Dashboard> dashboards;

  public DashboardService()
  {
    dashboards = new List<DashboardAllReply.Types.Dashboard>
    {
      new DashboardAllReply.Types.Dashboard()
      {
        Id = 1,
        Name = "Dashboard 1",
        HasTiles = true,        
      //  ,
      //  Tiles = new[]
      //{
      //  new Tile () { Id = 1, Url = "https://www.google.com", Title = "Google" },
      //  new Tile () { Id = 2, Url = "https://www.microsoft.com", Title = "Microsoft" },
      //}
      }
    };
  }

  public override async Task<DashboardAllReply> GetAll(DashboardAllRequest request, ServerCallContext context)
  {
    return new DashboardAllReply { Dashboards = { dashboards } };
  }

  public override async Task<DashboardReply> Get(DashboardRequest request, ServerCallContext context)
  {
    var reply = new DashboardReply { HasTiles = true };
    reply.Tiles.AddRange(new[]
    {
      new Tile () { Id = 1, Url = "https://www.google.com", Text = "Google" },
      new Tile () { Id = 2, Url = "https://www.microsoft.com", Text = "Microsoft" },
    });
    return reply;
  }
}
