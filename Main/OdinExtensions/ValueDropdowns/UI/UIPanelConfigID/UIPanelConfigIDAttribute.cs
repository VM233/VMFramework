using System;
using System.Collections.Generic;
using System.Diagnostics;
using VMFramework.GameLogicArchitecture;
using VMFramework.UI;

namespace VMFramework.OdinExtensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    [Conditional("UNITY_EDITOR")]
    public sealed class UIPanelConfigIDAttribute : Attribute, IGamePrefabIDFilterAttribute
    {
        public bool? IsUnique = null;

        public UIPanelConfigIDAttribute(bool isUnique)
        {
            IsUnique = isUnique;
        }

        public IEnumerable<IGamePrefab> GetGamePrefabs()
        {
            foreach (var uiPanelConfig in GamePrefabManager.GetAllGamePrefabs<IUIPanelConfig>())
            {
                if (IsUnique.HasValue && uiPanelConfig.IsUnique != IsUnique.Value)
                {
                    continue;
                }
                
                yield return uiPanelConfig;
            }
        }
    }
}