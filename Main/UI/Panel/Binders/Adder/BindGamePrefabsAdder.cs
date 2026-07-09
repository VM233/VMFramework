using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public class BindGamePrefabsAdder : PipelinedBindObjectsAdder<IGamePrefab>
    {
        public override bool IsComponentType => false;
    }
}