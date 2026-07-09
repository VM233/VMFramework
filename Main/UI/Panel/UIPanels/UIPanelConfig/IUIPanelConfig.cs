using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public interface IUIPanelConfig : IGamePrefab
    {
        public bool IsUnique { get; }
        
        public int SortingOrder { get; }
    }
}