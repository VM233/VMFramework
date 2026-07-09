using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace VMFramework.Core.JSON
{
    public class GameObjectConverter : JsonConverter<GameObject>
    {
        public override void WriteJson(JsonWriter writer, GameObject value, JsonSerializer serializer)
        {
            var objectToken = new JObject();

            foreach (var receiver in value.GetComponents<IJSONSerializationReceiver>())
            {
                receiver.SerializeTo(objectToken, serializer);
            }

            objectToken.WriteTo(writer);
        }

        public override GameObject ReadJson(JsonReader reader, Type objectType, GameObject existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            
            JObject objectToken = JObject.Load(reader);
            
            if (hasExistingValue == false)
            {
                throw new NotImplementedException();
            }

            if (existingValue == null)
            {
                return null;
            }

            var components = existingValue.GetComponents<IJSONSerializationReceiver>();

            foreach (var component in components)
            {
                component.DeserializeFrom(objectToken, serializer);
            }

            return existingValue;
        }
    }
}