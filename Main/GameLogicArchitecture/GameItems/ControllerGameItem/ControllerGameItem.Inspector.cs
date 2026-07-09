#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public partial class ControllerGameItem
    {
        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector, DisplayAsString]
        private string _id => GamePrefab?.id;
    }
}
#endif