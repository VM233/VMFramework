using System;
using System.Globalization;
using Newtonsoft.Json;
using UnityEngine;

namespace VMFramework.Core.JSON
{
    public sealed class RectangleFloatConverter : JsonConverter<RectangleFloat>
    {
        public override void WriteJson(JsonWriter writer, RectangleFloat value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            WriteVector2(writer, value.min);
            WriteVector2(writer, value.max);
            writer.WriteEndArray();
        }

        public override RectangleFloat ReadJson(JsonReader reader, Type objectType, RectangleFloat existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartArray)
            {
                throw new JsonSerializationException($"Error deserializing {nameof(RectangleFloat)}. Expected StartArray token.");
            }

            reader.Read();
            
            var min = ReadVector2(reader);
            reader.Read();
            var max = ReadVector2(reader);
            reader.Read();
            
            if (reader.TokenType != JsonToken.EndArray)
            {
                throw new JsonSerializationException($"Error deserializing {nameof(RectangleFloat)}. Expected EndArray token.");
            }
            
            return new(min, max);
        }

        private static void WriteVector2(JsonWriter writer, Vector2 vector)
        {
            writer.WriteValue(FormattableString.Invariant($"({vector.x}, {vector.y})"));
        }

        private static Vector2 ReadVector2(JsonReader reader)
        {
            if (reader.TokenType != JsonToken.String)
            {
                throw new JsonSerializationException(
                    $"Error deserializing {nameof(Vector2)}. Value: {reader.Value} must be a string.");
            }

            var value = (string)reader.Value;

            if (value == null)
            {
                throw new JsonSerializationException($"Error deserializing {nameof(Vector2)}. Value cannot be null.");
            }

            var parts = value.Trim('(', ')').Split(',');

            if (parts.Length != 2 ||
                float.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var x) == false ||
                float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var y) == false)
            {
                throw new JsonSerializationException($"Error deserializing {nameof(Vector2)}. " +
                                                     $"Value: {reader.Value} is not a valid {nameof(Vector2)}.");
            }

            return new Vector2(x, y);
        }
    }
}
