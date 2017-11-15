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
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonToken.String)
            {
                return ((string) reader.Value).Trim();
            }

            if (reader.TokenType == JsonToken.Integer ||
                reader.TokenType == JsonToken.Float ||
                reader.TokenType == JsonToken.Boolean)
            {
                return reader.Value.ToString().Trim();
            }

            throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing string.");
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(string);
    }
}
