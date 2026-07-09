#if UNITY_EDITOR
namespace VMFramework.Editor
{
    public sealed class GeneralSettingScriptCreationViewer : ScriptCreationViewer
    {
        protected override string NameSuffix => "GeneralSetting";
    }
}
#endif