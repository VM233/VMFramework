#if UNITY_EDITOR
using Sirenix.OdinInspector;

namespace VMFramework.Editor
{
    public sealed class GamePrefabScriptCreationViewer : ScriptCreationViewer
    {
        public bool createSubFolders = true;
        
        [EnumToggleButtons]
        public GamePrefabBaseType gamePrefabBaseType;

        public bool withGamePrefabGeneralSetting = true;

        public bool withGameItem = true;
        
        [ShowIf(nameof(withGameItem))]
        [EnumToggleButtons]
        public GameItemBaseType gameItemBaseType;

#if FISHNET
        [EnableIf(nameof(withGameItem))]
        public bool withSerializer = false;
#endif

        protected override string NameSuffix => withGameItem ? "Config" : string.Empty;
    }
}
#endif