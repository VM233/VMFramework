#if UNITY_EDITOR && ODIN_INSPECTOR
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.OdinExtensions
{
    public static class GamePrefabNameListQuery
    {
        public static ValueDropdownItem GetNameIDDropDownItem(this IGamePrefab prefab)
        {
            string name = prefab.Name;
            if (name.IsNullOrEmpty())
            {
                name = prefab.id;
            }
            return new(name, prefab.id);
        }
        
        /// <summary>
        /// 获取所有<see cref="IGamePrefab"/>的名称和ID，一般用于Odin插件里的ValueDropdownAttribute
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ValueDropdownItem> GetGamePrefabNameList()
        {
            foreach (var prefab in GamePrefabManager.GetAllGamePrefabs())
            {
                if (prefab is not null)
                {
                    yield return prefab.GetNameIDDropDownItem();
                }
            }
        }

        /// <summary>
        /// 获取特定Type对应的类或其派生类的<see cref="IGamePrefab"/>的名称和ID，
        /// 一般用于Odin插件里的ValueDropdown Attribute
        /// </summary>
        /// <param name="specificTypes"></param>
        /// <returns></returns>
        public static IEnumerable<ValueDropdownItem> GetGamePrefabNameListByType(
            params Type[] specificTypes)
        {
            foreach (var gamePrefab in GamePrefabManager.GetGamePrefabsByTypes(specificTypes))
            {
                yield return gamePrefab.GetNameIDDropDownItem();
            }
        }

        /// <summary>
        /// 获取特定Type对应的类或其派生类的<see cref="IGamePrefab"/>的名称和ID，
        /// 一般用于Odin插件里的ValueDropdown Attribute
        /// </summary>
        /// <param name="specificType"></param>
        /// <returns></returns>
        public static IEnumerable<ValueDropdownItem> GetGamePrefabNameListByType(Type specificType)
        {
            foreach (var gamePrefab in GamePrefabManager.GetGamePrefabsByType(specificType))
            {
                yield return gamePrefab.GetNameIDDropDownItem();
            }
        }

        /// <summary>
        /// 获取特定的GameItemType对应的<see cref="IGamePrefab"/>的名称和ID，
        /// 一般用于Odin插件里的ValueDropdown Attribute
        /// </summary>
        /// <param name="gameItemType"></param>
        /// <returns></returns>
        public static IEnumerable<ValueDropdownItem> GetGamePrefabNameListByGameItemType(Type gameItemType)
        {
            foreach (var gamePrefab in GamePrefabManager.GetGamePrefabsByGameItemType(gameItemType))
            {
                yield return gamePrefab.GetNameIDDropDownItem();
            }
        }
    }
}

#endif