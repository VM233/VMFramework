#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GamePrefabGeneralSetting
    {
        [TabGroup(TAB_GROUP_NAME, METADATA_CATEGORY)]
        [ShowInInspector]
        public string GamePrefabFolderPath =>
            EditorSetting.GamePrefabsAssetFolderPath.PathCombine(GamePrefabName);
        
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            RefreshInitialGamePrefabProviders();
        }
        
        private void OnInitialGamePrefabProvidersChanged()
        {
            OnInspectorInit();
            
            this.EnforceSave();
        }
        
        public void AddToInitialGamePrefabProviders(IGamePrefabsProvider provider)
        {
            if (provider.IsUnityNull())
            {
                return;
            }
            
            if (initialGamePrefabProviders.Contains(provider))
            {
                return;
            }
            
            initialGamePrefabProviders.Add(provider);
            
            OnInitialGamePrefabProvidersChanged();
        }

        public void AddToInitialGamePrefabProviders(IEnumerable<IGamePrefabsProvider> providers)
        {
            if (providers == null)
            {
                return;
            }

            foreach (var wrapper in providers)
            {
                if (wrapper.IsUnityNull())
                {
                    continue;
                }
                
                if (initialGamePrefabProviders.Contains(wrapper) == false)
                {
                    initialGamePrefabProviders.Add(wrapper);
                }
            }
            
            OnInitialGamePrefabProvidersChanged();
        }
        
        public void RemoveFromInitialGamePrefabProviders(IGamePrefabsProvider provider)
        {
            initialGamePrefabProviders.Remove(provider);
            
            OnInitialGamePrefabProvidersChanged();
        }
        
        public void SaveAllGamePrefabs()
        {
            foreach (var wrapper in initialGamePrefabProviders)
            {
                if (wrapper is Object obj)
                {
                    obj.SetEditorDirty();
                }
            }
            
            this.EnforceSave();
        }

        #region Open Scripts

        public void OpenGamePrefabScript()
        {
            BaseGamePrefabType.OpenScriptOfType();
        }
        
        public void OpenInitialGamePrefabScripts()
        {
            foreach (var wrapper in initialGamePrefabProviders)
            {
                wrapper.OpenGamePrefabScripts();
            }
        }

        public void OpenGameItemsOfInitialGamePrefabsScripts()
        {
            foreach (var wrapper in initialGamePrefabProviders)
            {
                wrapper.OpenGameItemScripts();
            }
        }

        #endregion
    }
}
#endif