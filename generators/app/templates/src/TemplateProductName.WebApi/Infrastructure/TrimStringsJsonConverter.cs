using System;
using Newtonsoft.Json;

namespace TemplateProductName.WebApi.Infrastructure
{
    /// <summary>
    /// Trims strings during deserialization
    /// </summary>
    public class TrimStringsJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Don't trim strings during serialization
            if (value != null)
            {
                writer.WriteValue(value);
            }
            else
            {
                writer.WriteNull();
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Null:
                    return null;
                case JsonToken.String:
                    return ((string) reader.Value).Trim();
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.Boolean:
                    return reader.Value.ToString().Trim();
                default:
                    throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing string.");
            }
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(string);
    }
}
