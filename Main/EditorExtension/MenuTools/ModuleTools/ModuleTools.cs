#if UNITY_EDITOR
using UnityEditor;
using VMFramework.Core.Editor;

namespace VMFramework.Editor
{
    public static class ModuleTools
    {
        [MenuItem(UnityMenuItemNames.MODULE_TOOLS + "/Enable DOTween Extension Module")]
        public static void EnableDOTweenExtensionModule()
        {
            ScriptingDefineSymbolsUtility.AddSymbol("DOTWEEN", "UNITASK_DOTWEEN_SUPPORT");
        }
        
        [MenuItem(UnityMenuItemNames.MODULE_TOOLS + "/Disable DOTween Extension Module")]
        public static void DisableDOTweenExtensionModule()
        {
            ScriptingDefineSymbolsUtility.RemoveSymbol("DOTWEEN", "UNITASK_DOTWEEN_SUPPORT");
        }
    }
}
#endif