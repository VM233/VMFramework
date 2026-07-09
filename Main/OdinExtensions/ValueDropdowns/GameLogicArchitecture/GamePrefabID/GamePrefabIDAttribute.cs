using System;
using System.Collections.Generic;
using System.Diagnostics;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.OdinExtensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    [Conditional("UNITY_EDITOR")]
    public class GamePrefabIDAttribute : GeneralValueDropdownAttribute, IGamePrefabIDFilterAttribute
    {
        private readonly bool hasFilter;
        
        public readonly Type[] GamePrefabTypes;

        public readonly bool FilterByGameItemType;
        
        public readonly Type GameItemType;

        public GamePrefabIDAttribute(bool filterByGameItemType, Type type)
        {
            hasFilter = true;
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
            hasFilter = true;
            FilterByGameItemType = false;
            GamePrefabTypes = gamePrefabTypes;
        }

        public GamePrefabIDAttribute()
        {
            hasFilter = false;
            FilterByGameItemType = false;
            GamePrefabTypes = null;
        }

        public IEnumerable<IGamePrefab> GetGamePrefabs()
        {
            if (hasFilter == false)
            {
                return GamePrefabManager.GetAllGamePrefabs();
            }
            
            if (FilterByGameItemType)
            {
                return GamePrefabManager.GetGamePrefabsByGameItemType(GameItemType);
            }

            return GamePrefabManager.GetGamePrefabsByTypes(GamePrefabTypes);
        }
    }
}