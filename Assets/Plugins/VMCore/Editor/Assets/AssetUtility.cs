using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEditor;

namespace VMFramework.Core.Editor
{
    public static class AssetUtility
    {
        #region Open Asset

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OpenAsset(this Object obj)
        {
            AssetDatabase.OpenAsset(obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OpenAssets(this IEnumerable<Object> objects)
        {
            AssetDatabase.OpenAsset(objects.ToArray());
        }

        #endregion

        #region Copy Asset

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T CopyAssetTo<T>(this T obj, string newPath) where T : Object
        {
            if (obj == null)
            {
                Debug.LogWarning("复制对象为空，无法复制");
                return null;
            }

            var sourcePath = obj.GetAssetPath();

            if (sourcePath.IsNullOrEmpty())
            {
                Debug.LogWarning("复制对象路径为空，无法复制");
                return null;
            }

            if (newPath.IsNullOrEmpty())
            {
                Debug.LogWarning("复制目标路径为空，无法复制");
                return null;
            }

            var absoluteDirectoryPath = newPath.ConvertAssetPathToAbsolutePath().GetDirectoryPath();

            if (absoluteDirectoryPath.ExistsDirectory() == false)
            {
                Debug.LogWarning($"复制目标路径{absoluteDirectoryPath}不存在，无法复制");
                return null;
            }

            if (AssetDatabase.CopyAsset(sourcePath, newPath) == false)
            {
                Debug.LogWarning($"复制{obj.GetType()}从{sourcePath}到{newPath}失败");
                return null;
            }
            
            var newObj = AssetDatabase.LoadAssetAtPath<T>(newPath);
            
            return newObj;
        }

        #endregion

        #region Exists Asset

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExistsAsset(this string path)
        {
            path = path.MakeAssetPath();
            
            var absolutePath = path.ConvertAssetPathToAbsolutePath();
            
            return absolutePath.ExistsFile();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExistsAssetWithWarning(this string path)
        {
            if (path.ExistsAsset())
            {
                Debug.LogWarning($"The asset already exists at {path}");
                return true;
            }
            
            return false;
        }

        #endregion

        #region Move Asset

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MoveAssetToNewFolder(this Object obj, string newFolder)
        {
            if (obj == null)
            {
                Debug.LogWarning($"Object is null, cannot move to {newFolder}");
                return;
            }
            
            var sourcePath = obj.GetAssetPath();
            
            if (sourcePath.IsNullOrEmpty())
            {
                Debug.LogWarning($"Object path is null, cannot move to {newFolder}");
                return;
            }
            
            if (newFolder.IsNullOrEmpty())
            {
                Debug.LogWarning($"New folder path is null or empty, cannot move to {newFolder}");
                return;
            }

            newFolder = newFolder.MakeAssetPath();
            
            if (newFolder.IsAssetPath() == false)
            {
                Debug.LogWarning($"New folder path {newFolder} is not an asset path, cannot move!");
                return;
            }
            
            var absoluteDirectoryPath = newFolder.ConvertAssetPathToAbsolutePath();
            absoluteDirectoryPath.CreateDirectory();

            var fileName = sourcePath.GetFileNameFromPath();
            var newPath = newFolder.PathCombine(fileName);
            
            AssetDatabase.MoveAsset(sourcePath, newPath);
            
            obj.EnforceSave();
        }

        #endregion

        #region Create Asset

        public static bool TryCreateAsset(this Object obj, string path)
        {
            if (CommonFolders.ProjectFolderPath.TryMakeRelative(path, out path) == false)
            {
                Debug.LogWarning($"保存路径{path}不在Assets文件夹下，创建{obj.GetType()}失败");

                Object.DestroyImmediate(obj);
                return false;
            }
            
            obj.CreateAsset(path);
            return true;
        }
        
        public static void CreateAsset(this Object obj, string path)
        {
            AssetDatabase.CreateAsset(obj, path);
            obj.EnforceSave();
            AssetDatabase.Refresh();
        }

        #endregion

        #region Is Asset
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAsset(this Object obj)
        {
            return AssetDatabase.Contains(obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAssetWithWarning(this Object obj)
        {
            if (obj == null)
            {
                Debug.LogWarning("Object is null, cannot check if it is an asset");
                return false;
            }

            if (AssetDatabase.Contains(obj) == false)
            {
                Debug.LogWarning($"Object {obj.name} is not an asset");
                return false;
            }
            
            return true;
        }

        #endregion

        #region Get Asset GUID

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetAssetGUID(this Object obj)
        {
            return AssetDatabase.AssetPathToGUID(obj.GetAssetPath());
        }

        #endregion
    }
}