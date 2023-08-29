using AutoMapper;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using TheDashboard.Frontend.Services.Converter;
using TheDashboard.SharedEntities;
using TheDashboard.ViewModels.Data;

namespace TheDashboard.Frontend.Services;

public abstract class ServiceInvokeCommand
{

  private readonly IMapper _mapper;
  private readonly HttpClient _httpClient;
  private readonly JsonSerializerOptions _options;

  protected ServiceInvokeCommand(IMapper mapper, IHttpClientFactory httpClientFactory)
  {
    _mapper = mapper;
    _httpClient = httpClientFactory.CreateClient("HttpCommandProxy");
    _options = new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      WriteIndented = false,
      Converters =
      {
        new CommandTypeConverter()
      }
    };
  }

  protected IMapper Mapper => _mapper;

  protected virtual async Task InvokeCommand<TEvent, TViewModel, TKey>(TViewModel viewModel) 
    where TEvent : Command
    where TViewModel : ViewModelBase<TKey>
    where TKey: IEquatable<TKey>
  {
    var evt = _mapper.Map<TEvent>(viewModel);
    var json = JsonSerializer.Serialize(evt, _options);
    await _httpClient.SendAsync(request: new HttpRequestMessage(HttpMethod.Post, "/api/command")
    {
      Content = new StringContent(json, Encoding.UTF8, "application/json")
    });
  }
}
