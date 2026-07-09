#if UNITY_EDITOR
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using VMFramework.Core.Editor;

namespace VMFramework.GameLogicArchitecture
{
    public static class GlobalSettingFileEditorUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OpenGlobalSettingScript([DisallowNull] this IGlobalSettingFile globalSettingFile)
        {
            foreach (var globalSetting in GlobalSettingCollector.Collect())
            {
                if (globalSetting.GlobalSettingFile == null)
                {
                    continue;
                }
                
                if (globalSetting.GlobalSettingFile.GetType() == globalSettingFile.GetType())
                {
                    globalSetting.OpenScriptOfObject();
                }
            }
        }
    }
}
#endif