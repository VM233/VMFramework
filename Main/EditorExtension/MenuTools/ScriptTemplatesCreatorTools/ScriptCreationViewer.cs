#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.Editor
{
    public abstract class ScriptCreationViewer : IScriptCreationViewer
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
        
        #region Interface Implementation

        string IScriptCreationViewer.AssetFolderPath
        {
            get => assetFolderPath;
            set
            {
                assetFolderPath = value;
                if (name.IsNullOrEmpty())
                {
                    name = assetFolderPath.GetFileNameWithoutExtensionFromPath();
                }
            }
        }

        string INameOwner.Name => name;

        string IScriptCreationViewer.NamespaceName => namespaceName;

        #endregion
    }
}
#endif
