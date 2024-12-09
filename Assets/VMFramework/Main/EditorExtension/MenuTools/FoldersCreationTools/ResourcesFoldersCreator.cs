#if UNITY_EDITOR
using UnityEditor;
using VMFramework.Core;

namespace VMFramework.Editor
{
    internal static class ResourcesFoldersCreator
    {
        [MenuItem(UnityMenuItemNames.FOLDERS_CREATION_TOOLS + "Create Resources Folders")]
        private static void CreateResourcesFolders()
        {
            var resourcesPath = CommonFolders.ResourcesFolderPath;

            resourcesPath.CreateDirectory();

            var folders = new[]
            {
                "Animations", "Audios", "Fonts", "Materials", "Prefabs", "Shaders", "Images", "Models",
                "UIDocuments"
            };

            foreach (var folder in folders)
            {
                var folderPath = resourcesPath + "/" + folder;
                folderPath.CreateDirectory();
            }
            
            AssetDatabase.Refresh();
        }
    }
}
#endif