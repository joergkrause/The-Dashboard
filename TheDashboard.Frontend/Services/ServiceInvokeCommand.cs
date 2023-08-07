using AutoMapper;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using TheDashboard.SharedEntities;
using TheDashboard.ViewModels.Data;

namespace TheDashboard.Frontend.Services;

public abstract class ServiceInvokeCommand
{

  private readonly IMapper _mapper;
  private readonly HttpClient _httpClient;

  protected ServiceInvokeCommand(IMapper mapper, IHttpClientFactory httpClientFactory)
  {
    _mapper = mapper;
    _httpClient = httpClientFactory.CreateClient("HttpCommandProxy");
  }

  protected IMapper Mapper => _mapper;

  protected virtual async Task InvokeCommand<TEvent, TViewModel, TKey>(TViewModel dto) 
    where TEvent : Command
    where TViewModel : ViewModelBase<TKey>
    where TKey: IEquatable<TKey>
  {
    var evt = _mapper.Map<TEvent>(dto);
    await _httpClient.SendAsync(request: new HttpRequestMessage(HttpMethod.Post, "/api/command")
    {
      Content = new StringContent(JsonSerializer.Serialize(evt), Encoding.UTF8, "application/json")
    });
  }
}
