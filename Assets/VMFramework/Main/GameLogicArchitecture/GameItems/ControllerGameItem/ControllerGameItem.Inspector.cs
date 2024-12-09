using Sirenix.OdinInspector;

namespace VMFramework.GameLogicArchitecture
{
    public partial class ControllerGameItem
    {
        [ShowInInspector, DisplayAsString]
        private string _id => GamePrefab?.id;
    }
}