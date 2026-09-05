using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using VMFramework.Core;
#if UNITY_EDITOR
using UnityEngine;
using VMFramework.Core.Editor;
using VMFramework.GameLogicArchitecture.Editor;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.OdinExtensions;
using VMFramework.Localization;
#endif

namespace VMFramework.GameLogicArchitecture
{
    public abstract class GamePrefabGeneralSetting : GeneralSetting, IGamePrefabsProvider
#if UNITY_EDITOR
        , IGameEditorMenuTreeNodesProvider, IGameEditorMenuTreeNode
#endif
    {
        #region Categories

        protected const string INITIAL_GAME_PREFABS_CATEGORY = "Initial GamePrefabs";

        protected const string GAME_TYPE_CATEGORY = "Game Type";

        #endregion

        #region Setting Metadata

        [TabGroup(TAB_GROUP_NAME, METADATA_CATEGORY)]
        [ShowInInspector]
        public virtual string GamePrefabName
        {
            get
            {
                if (BaseGamePrefabType.IsInterface == false)
                {
                    return BaseGamePrefabType.Name;
                }

                if (BaseGamePrefabType.Name.StartsWith("I"))
                {
                    return BaseGamePrefabType.Name[1..];
                }

                return BaseGamePrefabType.Name;
            }
        }

        [TabGroup(TAB_GROUP_NAME, METADATA_CATEGORY)]
        [ShowInInspector]
        public abstract Type BaseGamePrefabType { get; }

        #endregion

#if UNITY_EDITOR
        [LabelText("Initial GamePrefab"),
         TabGroup(TAB_GROUP_NAME, INITIAL_GAME_PREFABS_CATEGORY, SdfIconType.Info, TextColor = "blue")]
        [OnCollectionChanged(nameof(OnInitialGamePrefabProvidersChanged))]
        [Searchable]
        [ValueDropdown(nameof(GetInitialGamePrefabProviderOptions))]
#endif
        [UnityEngine.SerializeField]
        private List<UnityEngine.Object> initialGamePrefabProviderObjects = new();

        public IEnumerable<IGamePrefabsProvider> initialGamePrefabProviders =>
            initialGamePrefabProviderObjects.Cast<IGamePrefabsProvider>();

        public override void CheckSettings()
        {
            base.CheckSettings();
            ValidateInitialGamePrefabProviders();
        }

        private void ValidateInitialGamePrefabProviders()
        {
            foreach (var provider in initialGamePrefabProviderObjects)
            {
                if (provider == null || provider is not IGamePrefabsProvider)
                {
                    throw new InvalidOperationException($"General setting '{name}' has an invalid GamePrefab provider: {provider}.");
                }
            }
        }

        #region Initial Game Prefab Provider

        public void GetGamePrefabs(ICollection<IGamePrefab> gamePrefabsCollection)
        {
            foreach (var wrapper in initialGamePrefabProviders)
            {
                wrapper.GetGamePrefabs(gamePrefabsCollection);
            }
        }

        #endregion
#if UNITY_EDITOR
        [TabGroup(TAB_GROUP_NAME, METADATA_CATEGORY)]
        [ShowInInspector]
        public string GamePrefabFolderPath =>
            EditorSetting.GamePrefabsAssetFolderPath.PathCombine(GamePrefabName);

        private void OnInitialGamePrefabProvidersChanged()
        {
            ValidateInitialGamePrefabProviders();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
        }

        private IEnumerable<UnityEngine.Object> GetInitialGamePrefabProviderOptions() =>
            GamePrefabProviderQueryTools.GetProviders(BaseGamePrefabType).Cast<UnityEngine.Object>();

        public void RefreshInitialGamePrefabProviders()
        {
            // Deleted Unity assets no longer belong to the authoring provider list.
            initialGamePrefabProviderObjects.RemoveAll(provider => provider == null);
        }

        private static UnityEngine.Object GetProviderObject(IGamePrefabsProvider provider)
        {
            if (provider is not UnityEngine.Object providerObject || providerObject == null ||
                !UnityEditor.EditorUtility.IsPersistent(providerObject))
            {
                throw new ArgumentException("A GamePrefab provider must be a persistent Unity object.", nameof(provider));
            }
            return providerObject;
        }

        public void AddToInitialGamePrefabProviders(IGamePrefabsProvider provider)
        {
            var providerObject = GetProviderObject(provider);

            if (initialGamePrefabProviders.Contains(provider))
            {
                return;
            }

            initialGamePrefabProviderObjects.Add(providerObject);

            OnInitialGamePrefabProvidersChanged();
        }

        public void AddToInitialGamePrefabProviders(IEnumerable<IGamePrefabsProvider> providers)
        {
            if (providers == null)
            {
                throw new ArgumentNullException(nameof(providers));
            }

            foreach (var wrapper in providers)
            {
                var providerObject = GetProviderObject(wrapper);

                if (initialGamePrefabProviders.Contains(wrapper) == false)
                {
                    initialGamePrefabProviderObjects.Add(providerObject);
                }
            }

            OnInitialGamePrefabProvidersChanged();
        }

        public void RemoveFromInitialGamePrefabProviders(IGamePrefabsProvider provider)
        {
            initialGamePrefabProviderObjects.Remove(GetProviderObject(provider));

            OnInitialGamePrefabProvidersChanged();
        }

        public void SaveAllGamePrefabs()
        {
            foreach (var wrapper in initialGamePrefabProviders)
            {
                if (wrapper is UnityEngine.Object obj)
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
#endif
#if UNITY_EDITOR
        string INameOwner.Name => name;

        IEnumerable<object> IGameEditorMenuTreeNodesProvider.GetAllMenuTreeNodes()
        {
            return initialGamePrefabProviders;
        }

        #region Icon

        EditorIcon IEditorIconProvider.Icon
        {
            get
            {
                foreach (var gamePrefab in GamePrefabManager.GetAllGamePrefabs(BaseGamePrefabType))
                {
                    if (gamePrefab is not IGameEditorMenuTreeNode node)
                    {
                        continue;
                    }

                    if (node.Icon.IsNull() == false)
                    {
                        return node.Icon;
                    }
                }

                return EditorIcon.None;
            }
        }

        #endregion
#endif
#if UNITY_EDITOR
        #region Collect All Game Prefab Wrappers

        [Button(ButtonSizes.Medium), TabGroup(TAB_GROUP_NAME, INITIAL_GAME_PREFABS_CATEGORY)]
        public void CollectAllGamePrefabProviders()
        {
            AddToInitialGamePrefabProviders(GamePrefabProviderQueryTools.GetProviders(BaseGamePrefabType));
        }

        #endregion

        #region Game Prefab Create

        [Button(ButtonSizes.Medium, ButtonStyle.FoldoutButton, Expanded = true),
         TabGroup(TAB_GROUP_NAME, INITIAL_GAME_PREFABS_CATEGORY)]
        private void CreateGamePrefab([IsNotNullOrEmpty, IsUncreatedGamePrefabID] string gamePrefabID,
            GamePrefabWrapperType wrapperType)
        {
            gamePrefabID.AssertIsNotNullOrWhiteSpace(nameof(gamePrefabID));

            var gamePrefabTypes = BaseGamePrefabType.GetDerivedInstantiableClasses(true);

            new TypeSelector(gamePrefabTypes, selectedType =>
            {
                var wrapper = GamePrefabWrapperCreator.CreateGamePrefabWrapper(gamePrefabID, selectedType, wrapperType);

                if (wrapper == null)
                {
                    return;
                }

                wrapper.OpenInNewInspector();
            }).ShowInPopup();
        }

        #endregion
#endif
#if UNITY_EDITOR
        public override bool LocalizationEnabled =>
            BaseGamePrefabType.IsDerivedFrom<ILocalizedGamePrefab>(false);

        public override void RemoveInvalidLocalizedStrings()
        {
            base.RemoveInvalidLocalizedStrings();

            foreach (var gamePrefabWrapper in initialGamePrefabProviders)
            {
                if (gamePrefabWrapper is not ILocalizedStringOwnerConfig ownerConfig)
                {
                    continue;
                }

                ownerConfig.RemoveInvalidLocalizedStrings();
            }
        }

        public override void SetDefaultKeyValue(LocalizedStringAutoConfigSettings setting)
        {
            base.SetDefaultKeyValue(setting);

            foreach (var gamePrefabWrapper in initialGamePrefabProviders)
            {
                if (gamePrefabWrapper is not ILocalizedStringOwnerConfig ownerConfig)
                {
                    continue;
                }

                ownerConfig.SetDefaultKeyValue(setting);
            }
        }
#endif
    }
}
