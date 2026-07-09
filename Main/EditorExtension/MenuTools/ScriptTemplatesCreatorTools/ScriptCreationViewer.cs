#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.Editor
{
    public abstract class ScriptCreationViewer : BaseConfig, IScriptCreationViewer
    {
        [FolderPath]
        public string assetFolderPath;
        
        [IsClassName]
        [SuffixLabel("@" + nameof(NameSuffix))]
        public string name;
        
        [Namespace]
        public string namespaceName;

        protected virtual string NameSuffix => string.Empty;

        public string ClassName => name + NameSuffix;
        
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            if (name.IsNullOrEmpty())
            {
                name = assetFolderPath.GetFileNameWithoutExtensionFromPath();
            }
        }

        #region Interface Implementation

        string IScriptCreationViewer.AssetFolderPath
        {
            get => assetFolderPath;
            set => assetFolderPath = value;
        }

        string INameOwner.Name => name;

        string IScriptCreationViewer.NamespaceName => namespaceName;

        #endregion
    }
}
#endif