#if UNITY_EDITOR
using System.Runtime.CompilerServices;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Editor
{
    public static class EditorIconProviderUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetIconFromGamePrefab(string gamePrefabID, out EditorIcon icon)
        {
            if (gamePrefabID.IsNullOrEmpty())
            {
                icon = EditorIcon.None;
                return false;
            }

            if (GamePrefabManager.TryGetGamePrefab(gamePrefabID, out var gamePrefab) == false)
            {
                icon = EditorIcon.None;
                return false;
            }

            if (gamePrefab is not IEditorIconProvider iconProvider)
            {
                icon = EditorIcon.None;
                return false;
            }
            
            icon = iconProvider.Icon;
            return icon.IsNone() == false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetIconFromGamePrefabs(out EditorIcon icon, params string[] gamePrefabIDs)
        {
            foreach (var gamePrefabID in gamePrefabIDs)
            {
                if (TryGetIconFromGamePrefab(gamePrefabID, out icon))
                {
                    return true;
                }
            }
            
            icon = EditorIcon.None;
            return false;
        }
    }
}
#endif