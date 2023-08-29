using System.Text.Json;
using System;
using TheDashboard.SharedEntities;
using System.Text.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace TheDashboard.Frontend.Services.Converter;

/// <summary>
/// A generic converter that adds the commands type information so the proxy can re-create the
/// object and forward to the bus as a command's instance.
/// </summary>
public class CommandTypeConverter : JsonConverter<Command>
{

  public override bool CanConvert(Type typeToConvert)
  {
    return typeof(Command).IsAssignableFrom(typeToConvert) && typeToConvert != typeof(Command);
  }

  public override Command Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    // Implement deserialization if needed
    throw new NotImplementedException();
  }

  public override void Write(Utf8JsonWriter writer, Command value, JsonSerializerOptions options)
  {
    var baseJson = JsonSerializer.Serialize(value, value.GetType(), new JsonSerializerOptions());
    Utf8JsonReader reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(baseJson));    

    while (reader.Read())
    {
      switch (reader.TokenType)
      {
        case JsonTokenType.StartObject:
          writer.WriteStartObject();
          break;
        case JsonTokenType.EndObject:
          // Before ending the object, add the additional property
          writer.WriteString("Type", value.GetType().Name);
          writer.WriteEndObject();
          break;
        case JsonTokenType.PropertyName:
          writer.WritePropertyName(reader.GetString());
          break;
        case JsonTokenType.String:
          writer.WriteStringValue(reader.GetString());
          break;
        case JsonTokenType.Number:
          writer.WriteNumberValue(reader.GetDouble());
          break;
          // Handle other token types as needed
      }
    }
  }
}
