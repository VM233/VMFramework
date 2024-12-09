using System.Collections.Generic;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public interface IUIPanelConfig : ILocalizedGamePrefab, IGamePrefabProvider
    {
        public bool IsUnique { get; }
        
        public int SortingOrder { get; }
        
        public IReadOnlyList<IPanelModifierConfig> ModifiersConfigs { get; }
    }
}