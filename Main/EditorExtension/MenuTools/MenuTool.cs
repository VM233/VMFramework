#if UNITY_EDITOR
using UnityEditor;

namespace VMFramework.Editor
{
    public static class MenuTool
    {
        [MenuItem(UnityMenuItemNames.VMFRAMEWORK + "Reserialize All")]
        public static void ReserializeAll()
        {
            AssetDatabase.ForceReserializeAssets();
        }
    }
}
#endif