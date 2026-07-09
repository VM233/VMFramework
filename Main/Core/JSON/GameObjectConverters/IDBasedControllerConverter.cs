using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VMFramework.Core.JSON
{
    public abstract class IDBasedControllerConverter<TController> : JsonConverter<TController>
        where TController : class, IIDOwner<string>, IJSONSerializationProvider
    {
        public virtual void WriteExtraData(JObject o, TController value)
        {
            
        }
        
        public override void WriteJson(JsonWriter writer, TController value, JsonSerializer serializer)
        {
            var tokenObject = new JObject { { "id", value.id } };
            
            WriteExtraData(tokenObject, value);

            foreach (var receiver in value.JSONSerializationReceivers)
            {
                receiver.SerializeTo(tokenObject, serializer);
            }

            tokenObject.WriteTo(writer);
        }

        public abstract TController CreateInstance(string id, JObject o);

        public override TController ReadJson(JsonReader reader, Type objectType, TController existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            
            JObject o = JObject.Load(reader);

            if (o.TryGetValue("id", out var idToken) == false)
            {
                return existingValue;
            }

            if (idToken.Type != JTokenType.String)
            {
                return existingValue;
            }

            var id = idToken.Value<string>();

            TController controller;

            if (hasExistingValue)
            {
                controller = existingValue;
            }
            else
            {
                controller = CreateInstance(id, o);
            }

            if (controller == null)
            {
                return null;
            }

            foreach (var component in controller.JSONSerializationReceivers)
            {
                component.DeserializeFrom(o, serializer);
            }

            return controller;
        }
    }
}