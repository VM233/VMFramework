#if UNITY_EDITOR && ODIN_INSPECTOR
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using VMFramework.Core.Editor;
using VMFramework.Core.Linq;
using VMFramework.Core.Pools;
using VMFramework.Editor;
using VMFramework.GameLogicArchitecture;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.OdinExtensions
{
    internal sealed class GamePrefabWrapperContextMenuDrawer : OdinValueDrawer<GamePrefabWrapper>, IDefinesGenericMenuItems
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            CallNextDrawer(label);
        }

        void IDefinesGenericMenuItems.PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            var value = ValueEntry.SmartValue;

            var gamePrefabsCache = ListPool<IGamePrefab>.Default.Get();
            gamePrefabsCache.Clear();
            value.GetGamePrefabs(gamePrefabsCache);

            if (gamePrefabsCache.IsNullOrEmptyOrAllNull())
            {
                gamePrefabsCache.ReturnToDefaultPool();
                return;
            } 
                
            genericMenu.AddItem(EditorNames.OPEN_GAME_PREFAB_SCRIPT_PATH, value.OpenGamePrefabScripts);

            if (gamePrefabsCache.Any(gamePrefab => gamePrefab.GameItemType != null))
            {
                genericMenu.AddItem(EditorNames.OPEN_GAME_ITEM_SCRIPT_PATH, value.OpenGameItemScripts);
            }
            
            gamePrefabsCache.ReturnToDefaultPool();
        }
    }
}
#endif