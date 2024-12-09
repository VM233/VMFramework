using System;
using System.Diagnostics;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.OdinExtensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    [Conditional("UNITY_EDITOR")]
    public class GamePrefabIDAttribute : GeneralValueDropdownAttribute
    {
        public readonly Type[] GamePrefabTypes;

        public readonly bool FilterByGameItemType;
        
        public readonly Type GameItemType;

        public GamePrefabIDAttribute(bool filterByGameItemType, Type type)
        {
            FilterByGameItemType = filterByGameItemType;
            if (filterByGameItemType)
            {
                GameItemType = type;
            }
            else
            {
                GamePrefabTypes = new[] { type };
            }
        }

        public GamePrefabIDAttribute(params Type[] gamePrefabTypes)
        {
            FilterByGameItemType = false;
            GamePrefabTypes = gamePrefabTypes;
        }

        public GamePrefabIDAttribute()
        {
            FilterByGameItemType = false;
            GamePrefabTypes = new[] { typeof(IGamePrefab) };
        }
    }
}