using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VMFramework.Core.Editor
{
    public static class AssetPathUtility
    {
        #region Convert Asset Path To Absolute Path

        /// <summary>
        /// AssetPath starts with "Assets/"
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ConvertAssetPathToAbsolutePath(this string assetPath)
        {
            return CommonFolders.ProjectFolderPath.PathCombine(assetPath).ReplaceToDirectorySeparator();
        }

        #endregion

        #region Convert Relative Asset Path To Asset Path

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetAbsoluteFolderPathByRelativeFolderPath(this string relativeFolderPath,
            bool completeMatch, out string assetFolderPath)
        {
            foreach (var result in relativeFolderPath.GetAbsoluteFolderPathsByRelativeFolderPath(completeMatch))
            {
                assetFolderPath = result;
                return true;
            }

            assetFolderPath = null;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string> GetAbsoluteFolderPathsByRelativeFolderPath(this string relativeFolderPath,
            bool completeMatch)
        {
            if (relativeFolderPath.IsAssetPath())
            {
                yield return relativeFolderPath.ConvertAssetPathToAbsolutePath();
                yield break;
            }

            if (completeMatch == false)
            {
                foreach (var (directoryAbsolutePath, _) in CommonFolders.AssetsFolderPath.EnumerateAllDirectories()
                             .PathEndsWith(relativeFolderPath, true))
                {
                    yield return directoryAbsolutePath;
                }
            }
            else
            {
                foreach (var directoryAbsolutePath in CommonFolders.AssetsFolderPath.EnumerateAllDirectories())
                {
                    if (directoryAbsolutePath.PathEndsWith(relativeFolderPath))
                    {
                        yield return directoryAbsolutePath;
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetAssetFolderPathByRelativeFolderPath(this string relativeAssetFolderPath,
            bool completeMatch, out string assetFolderPath)
        {
            foreach (var result in relativeAssetFolderPath.GetAssetFolderPathsByRelativeFolderPath(completeMatch))
            {
                assetFolderPath = result;
                return true;
            }

            assetFolderPath = null;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string> GetAssetFolderPathsByRelativeFolderPath(this string relativeAssetFolderPath,
            bool completeMatch)
        {
            if (relativeAssetFolderPath.IsAssetPath())
            {
                yield return relativeAssetFolderPath;
                yield break;
            }

            if (completeMatch == false)
            {
                foreach (var (directoryAbsolutePath, matchCount) in CommonFolders.AssetsFolderPath
                             .EnumerateAllDirectories().PathEndsWith(relativeAssetFolderPath, true))
                {
                    var assetFolderPath = directoryAbsolutePath.PathTrimEnd(matchCount);
                    assetFolderPath = assetFolderPath.PathCombine(relativeAssetFolderPath);
                    assetFolderPath = assetFolderPath.GetRelativePath(CommonFolders.ProjectFolderPath);
                    assetFolderPath = assetFolderPath.MakeAssetPath();
                    yield return assetFolderPath;
                }
            }
            else
            {
                foreach (var directoryFullPath in CommonFolders.AssetsFolderPath.EnumerateAllDirectories())
                {
                    if (directoryFullPath.PathEndsWith(relativeAssetFolderPath))
                    {
                        var assetFolderPath = directoryFullPath.GetRelativePath(CommonFolders.ProjectFolderPath);
                        assetFolderPath = assetFolderPath.MakeAssetPath();
                        yield return assetFolderPath;
                    }
                }
            }
        }

        #endregion

        #region Is Asset Path

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAssetPath(this string path)
        {
            if (path == "Assets")
            {
                return true;
            }

            if (path.StartsWith("Assets" + Path.DirectorySeparatorChar) ||
                path.StartsWith("Assets" + Path.AltDirectorySeparatorChar))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Make Asset Path

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string MakeAssetPath(this string path)
        {
            return path.ReplaceToAltDirectorySeparator();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string MakeAssetPath(this string path, string extension)
        {
            path = path.ReplaceToAltDirectorySeparator();

            if (extension.StartsWith(".") == false)
            {
                extension = "." + extension;
            }

            if (path.EndsWith(extension) == false)
            {
                path += extension;
            }

            return path;
        }

        #endregion
        
        #region Get Asset By Path

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Object GetAssetByPath(this string path)
        {
            var result = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            
            return result;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetAssetByPath<T>(this string path) where T : Object
        {
            var result = AssetDatabase.LoadAssetAtPath(path, typeof(T));
            
            return (T) result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Object GetAssetByPath(this string path, Type type)
        {
            var result = AssetDatabase.LoadAssetAtPath(path, type);
            
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetAssetByPath(this string path, Type type, out Object result)
        {
            result = AssetDatabase.LoadAssetAtPath(path, type);

            return result != null;
        }

        #endregion
        
        #region Get Asset Path

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetAssetPath(this Object obj, out string assetPath)
        {
            if (obj.IsAsset() == false)
            {
                assetPath = null;
                return false;
            }
            
            assetPath = obj.GetAssetPath();
            
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetAssetPathWithWarning(this Object obj, out string assetPath)
        {
            if (obj.IsAssetWithWarning() == false)
            {
                assetPath = null;
                return false;
            }
            
            assetPath = obj.GetAssetPath();
            
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetAssetAbsolutePath(this Object obj, out string absolutePath)
        {
            if (obj.IsAsset() == false)
            {
                absolutePath = null;
                return false;
            }
            
            absolutePath = obj.GetAssetAbsolutePath();
            
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetAssetAbsolutePathWithWarning(this Object obj, out string absolutePath)
        {
            if (obj.IsAssetWithWarning() == false)
            {
                absolutePath = null;
                return false;
            }
            
            absolutePath = obj.GetAssetAbsolutePath();
            
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetAssetPath(this Object obj)
        {
            return AssetDatabase.GetAssetPath(obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetAssetFolderPath(this Object obj)
        {
            return AssetDatabase.GetAssetPath(obj).GetDirectoryPath();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GUIDToAssetPath(this string guid)
        {
            return AssetDatabase.GUIDToAssetPath(guid);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetAssetAbsolutePath(this Object asset)
        {
            return asset == null
                ? string.Empty
                : CommonFolders.ProjectFolderPath.PathCombine(asset.GetAssetPath()).ReplaceToDirectorySeparator();
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetFirstValidAssetPath(this string assetFolderPath, IEnumerable<string> names,
            out string assetPath, string extension = "")
        {
            assetPath = null;
            
            foreach (var name in names)
            {
                var assetName = name;
                
                if (extension.IsNullOrEmpty() == false && assetName.EndsWith(extension) == false)
                {
                    assetName += extension;
                }
                
                assetPath = assetFolderPath.PathCombine(assetName);

                if (assetPath.ExistsAsset() == false)
                {
                    return true;
                }
            }
            
            return false;
        }

        #region Rename

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Rename(this Object obj, string newName)
        {
            if (newName.IsNullOrWhiteSpace())
            {
                Debug.LogWarning($"{obj.name}'s New Name cannot be Null or Empty.");
                return;
            }
            
            string selectedAssetPath = AssetDatabase.GetAssetPath(obj);

            if (selectedAssetPath.IsNullOrEmpty())
            {
                obj.name = newName;
            }
            else
            {
                AssetDatabase.RenameAsset(selectedAssetPath, newName);
            }

            Undo.RecordObject(obj, "Rename");
            
            obj.EnforceSave();
        }

        #endregion
    }
}