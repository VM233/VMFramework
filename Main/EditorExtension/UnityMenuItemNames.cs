#if UNITY_EDITOR
using VMFramework.Core;

namespace VMFramework.Editor
{
    public static class UnityMenuItemNames
    {
        public const string ASSETS_CREATE = "Assets/Create/";

        public const string SCRIPT_TEMPLATE = ASSETS_CREATE + FrameworkMeta.NAME + " Script Template/";
        
        public const string VMFRAMEWORK = FrameworkMeta.NAME + "/";
        
        public const string EDITOR_INITIALIZATION = VMFRAMEWORK + "Editor Initialization/";
        
        public const string FOLDERS_CREATION_TOOLS = VMFRAMEWORK + "Folders Creation Tools/";
        
        public const string MODULE_TOOLS = VMFRAMEWORK + "Module Tools/";
        
        public const string GLOBAL_SETTINGS = VMFRAMEWORK + "Global Settings/";
        
        public const string GAME_PREFABS_TOOLS = VMFRAMEWORK + "Game Prefabs Tools/";
    }
}
#endif