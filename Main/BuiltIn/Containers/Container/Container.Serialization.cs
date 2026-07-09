using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VMFramework.Core.JSON;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Containers
{
    public partial class Container : IJSONSerializationReceiver, IStateCloneable
    {
        public virtual void CloneFrom(IStateCloneable stateCloneable, StateCloneHint hint)
        {
            var other = (Container)stateCloneable;
            var items = ListPool<IContainerItem>.Default.Get();
            items.Clear();
            foreach (var otherItem in other.items)
            {
                if (otherItem == null)
                {
                    items.Add(null);
                    continue;
                }

                var itemCloneHint = hint;
                itemCloneHint.isNested = true;
                var item = otherItem.GetClone(itemCloneHint);
                items.Add(item);
            }

            LoadFromItemsList(items, autoReturn: true, count: items.Count);

            items.Clear();
            items.ReturnToDefaultPool();
        }

        public virtual void SerializeTo(JObject o, JsonSerializer serializer)
        {
            if (ValidCount > 0)
            {
                o.Add("items", JArray.FromObject(items, serializer));
            }
        }

        public virtual void DeserializeFrom(JObject o, JsonSerializer serializer)
        {
            if (o.TryGetValue("items", out JToken itemsToken))
            {
                var savedItems = itemsToken.ToObject<List<IContainerItem>>(serializer);
                LoadFromItemsList(savedItems, autoReturn: true, count: savedItems.Count);
            }
        }
    }
}