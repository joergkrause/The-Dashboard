using AutoMapper;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
    where TKey : IEquatable<TKey>
  {
    var evt = _mapper.Map<TEvent>(dto);
    JsonSerializerOptions options = new()
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      WriteIndented = true,
      Converters =
    {
      new AddTypeNameConverter<TEvent>()
    }
    };
    var json = JsonSerializer.Serialize(evt, options);
    await _httpClient.SendAsync(request: new HttpRequestMessage(HttpMethod.Post, "/api/command")
    {
      Content = new StringContent(json, Encoding.UTF8, "application/json")
    });
  }
}

public class AddTypeNameConverter<T> : JsonConverter<T> where T : Command
{
  public override bool CanConvert(Type typeToConvert)
  {
    // This converter is applied to all types.
    return true;
  }

  public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    // We're not implementing custom deserialization logic in this example.
    // You can enhance this part if needed.
    throw new NotImplementedException();
  }

  public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
  {
    Type type = value.GetType();
    byte[] tempJsonBytes = JsonSerializer.SerializeToUtf8Bytes(value, type);

    using (JsonDocument doc = JsonDocument.Parse(tempJsonBytes))
    {
      writer.WriteStartObject();

      // Write the type name property.
      writer.WriteString("type", type.Name);

      // Write the original properties.
      foreach (var element in doc.RootElement.EnumerateObject())
      {
        element.WriteTo(writer);
      }

      writer.WriteEndObject();
    }
  }
}
