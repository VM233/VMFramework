#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.Editor
{
    public sealed class DerivedGamePrefabScriptCreationViewer : ScriptCreationViewer
    {
        public bool createSubFolders = true;
        
        [DerivedType(typeof(IGamePrefab), IncludingInterfaces = false, IncludingSealed = false)]
        [OnValueChanged(nameof(OnParentGamePrefabTypeChanged))]
        public Type parentGamePrefabType;

        public bool withGamePrefabInterface = true;
        
        [EnableIf(nameof(IsGamePrefabHasGameItem))]
        public bool withGameItem = true;
        
        [EnableIf(nameof(IsGamePrefabHasGameItem))]
        public bool withGameItemInterface = true;
        
        protected override string NameSuffix => withGameItem ? "Config" : string.Empty;

        private bool IsGamePrefabHasGameItem()
        {
            return GamePrefabTypeQuery.HasGameItem(parentGamePrefabType);
        }

        #region Parent GamePrefab Type Change Event

        private Type oldParentGamePrefabType;

        private void OnParentGamePrefabTypeChanged()
        {
            if (oldParentGamePrefabType != null && oldParentGamePrefabType.Namespace == namespaceName)
            {
                if (parentGamePrefabType != null)
                {
                    namespaceName = parentGamePrefabType.Namespace;
                }
            }
            else if (parentGamePrefabType != null && namespaceName.IsNullOrWhiteSpace())
            {
                namespaceName = parentGamePrefabType.Namespace;
            }
            
            oldParentGamePrefabType = parentGamePrefabType;
        }

        #endregion
    }
}
#endif