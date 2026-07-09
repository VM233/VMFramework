using VMFramework.Configuration;

namespace VMFramework.GameLogicArchitecture
{
    [CommonPresetAutoRegister(TYPE_PRESET_KEY, typeof(string))]
    public static class DescriptionMeta
    {
        public const string TYPE_PRESET_KEY = "Description Type";
    }
}