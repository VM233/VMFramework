using VMFramework.Configuration;

namespace VMFramework.GameLogicArchitecture
{
    [CommonPresetAutoRegister(TYPE_PRESET_KEY, typeof(string))]
    public static class IconMeta
    {
        public const string TYPE_PRESET_KEY = "Icon Type";
    }
}