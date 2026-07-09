using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using VMFramework.Core.Pools;
using VMFramework.OdinExtensions;

namespace VMFramework.Core.JSON
{
    public sealed partial class ExternalJSONSerializationReceiver : MonoBehaviour, IJSONSerializationReceiver
    {
        [IsNotNullOrEmpty]
        public List<GameObject> serializedObjects = new();

        private IJSONSerializationReceiver[] receivers;

        private void Awake()
        {
            var receiversList = ListPool<IJSONSerializationReceiver>.Default.Get();
            receiversList.Clear();

            foreach (var serializedObject in serializedObjects)
            {
                receiversList.AddRange(serializedObject.GetComponents<IJSONSerializationReceiver>());
            }
            
            receivers = receiversList.ToArray();
            receiversList.ReturnToDefaultPool();
        }

        public void SerializeTo(JObject o, JsonSerializer serializer)
        {
            foreach (var receiver in receivers)
            {
                receiver.SerializeTo(o, serializer);
            }
        }

        public void DeserializeFrom(JObject o, JsonSerializer serializer)
        {
            foreach (var receiver in receivers)
            {
                receiver.DeserializeFrom(o, serializer);
            }
        }
    }
}