using Newtonsoft.Json.Linq;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Core.JSON
{
    public class ControllerGameItemConverter : IDBasedControllerConverter<IJSONSerializableControllerGameItem>
    {
        public override IJSONSerializableControllerGameItem CreateInstance(string id, JObject o)
        {
            return GameItemManager.Instance.Get<IJSONSerializableControllerGameItem>(id);
        }
    }
}