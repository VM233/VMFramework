#if UNITY_EDITOR
namespace VMFramework.GameLogicArchitecture
{
    public partial interface IGlobalSettingFile
    {
        public void AutoFindSettings();

        public void AutoFindAndCreateSettings();
    }
}
#endif