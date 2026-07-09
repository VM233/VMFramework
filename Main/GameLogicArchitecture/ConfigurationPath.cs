namespace VMFramework.GameLogicArchitecture
{
    public static class ConfigurationPath
    {
        public const string INTERNAL_RESOURCES_PATH = "VMFramework/GameResources/";
        
        public const string INTERNAL_SCRIPT_TEMPLATES_PATH = INTERNAL_RESOURCES_PATH + "ScriptTemplates/";

        public const string DEFAULT_RESOURCES_PATH = "Assets/GameResources";
        
        public const string DEFAULT_CONFIGURATIONS_PATH = DEFAULT_RESOURCES_PATH + "/Configurations";

        public const string DEFAULT_GLOBAL_SETTINGS_PATH = DEFAULT_CONFIGURATIONS_PATH + "/GlobalSettings";
        
        public const string DEFAULT_GENERAL_SETTINGS_PATH = DEFAULT_CONFIGURATIONS_PATH + "/GeneralSettings";
        
        public const string DEFAULT_GAME_PREFABS_PATH = DEFAULT_CONFIGURATIONS_PATH + "/GamePrefabs";
        
        public const string DEFAULT_COMMON_PRIORITIES_PATH = DEFAULT_CONFIGURATIONS_PATH + "/CommonPriorities";

        public const string INTERNAL_GLOBAL_SETTINGS_PATH = DEFAULT_GLOBAL_SETTINGS_PATH;
    }
}
