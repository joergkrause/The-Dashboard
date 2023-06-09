using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using MassTransit.Monitoring.Performance;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using Workshop.DashboardService.Infrastructure;
using Workshop.DashboardService.Infrastructure.Integration.Events;
using Workshop.DatabaseLayer;
using Workshop.Domain;
using Workshop.Services.TransferObjects;

namespace Workshop.Services;

public class LayoutService : UnitOfWork<DashboardContext>
{

  private readonly IMapper _mapper;
  private readonly IPublishEndpoint _publishEndpoint;

  public LayoutService(DashboardContext context, IMapper mapper, IPublishEndpoint publishEndpoint) : base(context)
  {
    _mapper = mapper;
    _publishEndpoint = publishEndpoint;
  }

  public async Task<IEnumerable<AdminLayout>> GetAdminLayouts()
  {
    var adminLayouts = Context.Layouts.OfType<AdminLayout>();
    return adminLayouts.ToList();
  }

  public async Task<IEnumerable<UserLayout>> GetUserLayouts()
  {
    var userLayouts = Context.Layouts.OfType<UserLayout>();
    return userLayouts.ToList();
  }
}
