using System;
using System.Globalization;
using Newtonsoft.Json;
using UnityEngine;

namespace VMFramework.Core.JSON
{
    public sealed class RectangleIntegerConverter : JsonConverter<RectangleInteger>
    {
        public override void WriteJson(JsonWriter writer, RectangleInteger value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            WriteVector2Int(writer, value.min);
            WriteVector2Int(writer, value.max);
            writer.WriteEndArray();
        }

        public override RectangleInteger ReadJson(JsonReader reader, Type objectType,
            RectangleInteger existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartArray)
            {
                throw new JsonSerializationException($"Error deserializing {nameof(RectangleInteger)}. Expected StartArray token.");
            }

            reader.Read();
            
            var min = ReadVector2Int(reader);
            reader.Read();
            var max = ReadVector2Int(reader);
            reader.Read();
            
            if (reader.TokenType != JsonToken.EndArray)
            {
                throw new JsonSerializationException($"Error deserializing {nameof(RectangleInteger)}. Expected EndArray token.");
            }
            
            return new(min, max);
        }

        private static void WriteVector2Int(JsonWriter writer, Vector2Int vector)
        {
            writer.WriteValue(FormattableString.Invariant($"({vector.x}, {vector.y})"));
        }

        private static Vector2Int ReadVector2Int(JsonReader reader)
        {
            if (reader.TokenType != JsonToken.String)
            {
                throw new JsonSerializationException(
                    $"Error deserializing {nameof(Vector2Int)}. Value: {reader.Value} must be a string.");
            }

            var value = (string)reader.Value;

            if (value == null)
            {
                throw new JsonSerializationException(
                    $"Error deserializing {nameof(Vector2Int)}. Value cannot be null.");
            }

            var parts = value.Trim('(', ')').Split(',');

            if (parts.Length != 2 ||
                int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var x) == false ||
                int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var y) == false)
            {
                throw new JsonSerializationException($"Error deserializing {nameof(Vector2Int)}. " +
                                                     $"Value: {reader.Value} is not a valid {nameof(Vector2Int)}");
            }

            return new Vector2Int(x, y);
        }
    }
}
