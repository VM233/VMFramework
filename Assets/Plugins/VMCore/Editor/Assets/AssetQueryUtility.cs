using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VMFramework.Core.Editor
{
    public static class AssetQueryUtility
    {
        #region By Asset Path

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Object> FindAssetsByAssetPath(this AssetPath assetPath, bool isExactMatch = false,
            StringComparison comparison = StringComparison.Ordinal)
        {
            if (assetPath.path.IsAssetPath() == false)
            {
                Debug.LogError($"{assetPath} is not a valid asset path.");
                return Enumerable.Empty<Object>();
            }

            return assetPath.fileName.FindAssetsOfName<Object>(isExactMatch, comparison, assetPath.folderPath);
        }

        #endregion

        #region By Relative Asset Path

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TAsset> FindAssetsByRelativeAssetPath<TAsset>(this AssetPath relativeAssetPath,
            bool isExactMatch = false, StringComparison comparison = StringComparison.Ordinal)
            where TAsset : Object
        {
            var relativeAssetFolder = relativeAssetPath.folderPath;
            
            if (relativeAssetFolder.TryGetAssetFolderPathByRelativeFolderPath(true, out var assetFolderPath) == false)
            {
                return EmptyCollections<TAsset>.emptyList;
            }

            return relativeAssetPath.fileName.FindAssetsOfName<TAsset>(isExactMatch: isExactMatch, comparison: comparison,
                searchInFolders: assetFolderPath);
        }

        #endregion
        
        #region Find By Type And By Name

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TAsset FindAssetOfName<TAsset>(this string assetName, bool isExactMatch = false,
            StringComparison comparison = StringComparison.Ordinal, params string[] searchInFolders)
            where TAsset : Object
        {
            return (TAsset)assetName.FindAssetOfName(typeof(TAsset), isExactMatch, comparison, searchInFolders);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryFindAssetOfName<TAsset>(this string assetName, out TAsset asset, bool isExactMatch = false,
            StringComparison comparison = StringComparison.Ordinal, params string[] searchInFolders)
            where TAsset : Object
        {
            if (assetName.TryFindAssetOfName(typeof(TAsset), out var obj, isExactMatch, comparison, searchInFolders))
            {
                asset = (TAsset)obj;
                return true;
            }

            asset = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Object FindAssetOfName(this string assetName, Type type, bool isExactMatch = false,
            StringComparison comparison = StringComparison.Ordinal, params string[] searchInFolders)
        {
            return assetName.FindAssetsOfName(type, isExactMatch, comparison, searchInFolders).FirstOrDefault();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryFindAssetOfName(this string assetName, Type type, out Object asset, bool exactMatch = false,
            StringComparison comparison = StringComparison.Ordinal, params string[] searchInFolders)
        {
            asset = assetName.FindAssetsOfName(type, exactMatch, comparison, searchInFolders).FirstOrDefault();
            
            return asset != null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TAsset> FindAssetsOfName<TAsset>(this string assetName, bool isExactMatch = false,
            StringComparison comparison = StringComparison.Ordinal, params string[] searchInFolders)
            where TAsset : Object
        {
            return FindAssetsOfName(assetName, typeof(TAsset), isExactMatch, comparison, searchInFolders).Cast<TAsset>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Object> FindAssetsOfName(this string assetName, Type type, bool isExactMatch = false,
            StringComparison comparison = StringComparison.Ordinal, params string[] searchInFolders)
        {
            var assets = FindAssets(assetName, type, searchInFolders);

            if (isExactMatch == false)
            {
                foreach (var asset in assets)
                {
                    yield return asset;
                }
                
                yield break;
            }

            int resultCount = 0;

            foreach (var asset in assets)
            {
                if (string.Compare(asset.name, assetName, comparison) == 0)
                {
                    yield return asset;

                    resultCount++;
                }
            }

            if (resultCount == 0)
            {
                Debug.LogWarning(
                    $"Failed to find Asset of name {assetName} of type {type} " +
                    $"in folders {searchInFolders.ToFormattedString(",")}");
            }
        }

        #endregion

        #region Find Assets Of Type

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Object> FindAssetsOfType(this Type type, params string[] searchInFolders)
        {
            return FindAssets(null, type, searchInFolders);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Object FindAssetOfType(this Type type, params string[] searchInFolders)
        {
            return FindAssets(null, type, searchInFolders).FirstOrDefault();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryFindAssetOfType(this Type type, out Object asset, params string[] searchInFolders)
        {
            asset = FindAssets(null, type, searchInFolders).FirstOrDefault();
            
            return asset != null;
        }
       
        /// <summary>
        /// 在指定文件夹中查找指定类型的资源，文件夹路径为Assets/xxx/xxx
        /// </summary>
        /// <param name="searchInFolder"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> FindAssetsOfType<T>(this string searchInFolder)
        {
            return FindAssetsOfType(typeof(T), searchInFolder).Cast<T>();
        }

        #endregion

        #region Find Assets

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Object> FindAssets(string name, Type type, string[] searchInFolders = null, bool withWarning = true)
        {
            int resultCount = 0;

            string content = string.Empty;

            if (name.IsNullOrEmpty() == false)
            {
                content += " " + name;
            }

            if (type.IsDerivedFrom<Component>(false))
            {
                content = "t:GameObject" + content;
                
                foreach (var guid in AssetDatabase.FindAssets(content, searchInFolders))
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);

                    var targetObject = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                    if (targetObject != null && targetObject.GetComponent(type) != null)
                    {
                        yield return targetObject.GetComponent(type);

                        resultCount++;
                    }
                }
            }
            else
            {
                content = $"t:{type.Name}" + content;

                foreach (var guid in AssetDatabase.FindAssets(content, searchInFolders))
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);

                    var asset = AssetDatabase.LoadAssetAtPath(path, type);

                    if (asset != null)
                    {
                        yield return asset;
                        resultCount++;
                    }
                }
            }

            if (withWarning == false || resultCount > 0)
            {
                yield break;
            }
            
            string searchInFoldersWarning = "";

            if (searchInFolders.IsNullOrEmpty() == false)
            {
                searchInFoldersWarning = $" in folders: {searchInFolders.ToFormattedString(",")}";
            }

            if (type.IsDerivedFrom<Component>(false))
            {
                Debug.LogWarning($"Failed to find GameObject with Component of type {type}{searchInFoldersWarning}");
            }
            else
            {
                Debug.LogWarning($"Failed to find Asset of type {type}{searchInFoldersWarning}");
            }
        }

        #endregion
    }
}