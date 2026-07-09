#if UNITY_EDITOR
namespace VMFramework.Editor
{
    public static partial class EditorNames
    {
        #region Script

        public const string SCRIPT = "Script";
        
        public const string SELECT_THIS_SCRIPT = "Select This Script";
        
        public const string SELECT_THIS_SCRIPT_PATH = SCRIPT + "/" + SELECT_THIS_SCRIPT;

        public const string OPEN_THIS_SCRIPT = "Open This Script";

        public const string OPEN_THIS_SCRIPT_PATH = SCRIPT + "/" + OPEN_THIS_SCRIPT;

        public const string OPEN_GAME_PREFAB_SCRIPT = "Open Game Prefab Script";

        public const string OPEN_GAME_PREFAB_SCRIPT_PATH = SCRIPT + "/" + OPEN_GAME_PREFAB_SCRIPT;

        public const string OPEN_GAME_ITEM_SCRIPT = "Open Game Item Script";

        public const string OPEN_GAME_ITEM_SCRIPT_PATH = SCRIPT + "/" + OPEN_GAME_ITEM_SCRIPT;

        public const string OPEN_CONTROLLER_SCRIPT = "Open Controller Script";

        public const string OPEN_CONTROLLER_SCRIPT_PATH = SCRIPT + "/" + OPEN_CONTROLLER_SCRIPT;

        public const string OPEN_GLOBAL_SETTING_SCRIPT = "Open Global Setting Script";

        public const string OPEN_GLOBAL_SETTING_SCRIPT_PATH = SCRIPT + "/" + OPEN_GLOBAL_SETTING_SCRIPT;

        public const string OPEN_INITIAL_GAME_PREFABS_SCRIPTS = "Open All Initial Game Prefabs Script";

        public const string OPEN_INITIAL_GAME_PREFABS_SCRIPTS_PATH =
            SCRIPT + "/" + OPEN_INITIAL_GAME_PREFABS_SCRIPTS;

        public const string OPEN_GAME_ITEMS_OF_INITIAL_GAME_PREFABS_SCRIPTS =
            "Open All Game Items of Initial Game Prefabs Script";

        public const string OPEN_GAME_ITEMS_OF_INITIAL_GAME_PREFABS_SCRIPTS_PATH =
            SCRIPT + "/" + OPEN_GAME_ITEMS_OF_INITIAL_GAME_PREFABS_SCRIPTS;

        #endregion

        #region Save

        public const string SAVE = "Save";

        public const string SAVE_ALL = "Save All";

        #endregion

        #region Asset

        public const string ASSET = "Asset";

        public const string SELECT_ASSET = "Select Asset";

        public const string SELECT_ASSET_PATH = ASSET + "/" + SELECT_ASSET;

        public const string OPEN_ASSET_IN_NEW_INSPECTOR = "Open Asset in New Inspector";

        public const string OPEN_ASSET_IN_NEW_INSPECTOR_PATH = ASSET + "/" + OPEN_ASSET_IN_NEW_INSPECTOR;

        public const string DELETE_ASSET = "Delete Asset";

        public const string DELETE_ASSET_PATH = ASSET + "/" + DELETE_ASSET;

        public const string OPEN_IN_EXPLORER = "Open in Explorer";

        public const string OPEN_IN_EXPLORER_PATH = ASSET + "/" + OPEN_IN_EXPLORER;

        #endregion

        #region Addressable

        public const string ADDRESSABLE = "Addressable";
        
        public const string SELECT_ADDRESSABLE_GROUP = "Select Addressable Group";

        public const string SELECT_ADDRESSABLE_GROUP_PATH = ADDRESSABLE + "/" + SELECT_ADDRESSABLE_GROUP;

        #endregion

        #region General Settings

        public const string GENERAL_SETTINGS = "General Settings";

        public const string AUTO_FIND_SETTINGS = "Auto Find Settings";

        public const string AUTO_FIND_SETTINGS_PATH = GENERAL_SETTINGS + "/" + AUTO_FIND_SETTINGS;

        public const string AUTO_FIND_AND_CREATE_SETTINGS = "Auto Find and Create Settings";

        public const string AUTO_FIND_AND_CREATE_SETTINGS_PATH = GENERAL_SETTINGS + "/" + AUTO_FIND_AND_CREATE_SETTINGS;

        #endregion
    }
}
#endif