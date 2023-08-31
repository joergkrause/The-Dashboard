using System.Text.Json.Serialization;
using System.Text.Json;
using TheDashboard.SharedEntities;
using System.Runtime.Serialization;

namespace TheDashboard.Proxy.Services;

public class CommandConverter : JsonConverter<Command>
{
  public override bool CanConvert(Type typeToConvert)
  {
    return typeof(Command).IsAssignableFrom(typeToConvert);
  }

  public override Command Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    var jsonObject = JsonDocument.ParseValue(ref reader).RootElement;

    try
    {
      var typeName = jsonObject.GetProperty("Type").GetString();
      var type = Type.GetType($"TheDashboard.SharedEntities.{typeName}, TheDashboard.SharedEntities");
      if (type != null)
      {
        var command = JsonSerializer.Deserialize(jsonObject, type) as Command;
        if (command != null)
        {
          return command;
        }
      }
    }
    catch
    {
    }
    throw new SerializationException($"Unable to deserialize command: {jsonObject}");
  }

  public override void Write(Utf8JsonWriter writer, Command value, JsonSerializerOptions options)
  {
    throw new NotImplementedException();
  }
}