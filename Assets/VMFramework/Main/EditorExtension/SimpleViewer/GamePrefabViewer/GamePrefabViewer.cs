﻿#if UNITY_EDITOR
using UnityEditor;
using VMFramework.OdinExtensions;

namespace VMFramework.Editor
{
    internal sealed class GamePrefabViewer : SimpleOdinEditorWindow<GamePrefabViewerContainer>
    {
        public const string EDITOR_NAME = "Game Prefab Viewer";

        [MenuItem(UnityMenuItemNames.GAME_PREFABS_TOOLS + EDITOR_NAME)]
        public static void OpenWindow() => GetSimpleWindow<GamePrefabViewer>(EDITOR_NAME);
    }
}
#endif