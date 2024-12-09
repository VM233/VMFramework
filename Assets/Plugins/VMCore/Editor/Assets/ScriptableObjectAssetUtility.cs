using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace VMFramework.Core.Editor
{
    public static class ScriptableObjectAssetUtility
    {
        public static ScriptableObject FindScriptableObject(this Type type)
        {
            if (type.IsDerivedFrom<ScriptableObject>(false) == false)
            {
                Debug.LogWarning($"{type}不是{nameof(ScriptableObject)}的子类");
                return default;
            }

            var result = type.FindAssetOfType() as ScriptableObject;

            return result;
        }

        public static ScriptableObject FindOrCreateScriptableObject(this Type type, string newPath, 
            string newName)
        {
            if (type.IsDerivedFrom<ScriptableObject>(false) == false)
            {
                Debug.LogWarning($"{type} is not a subclass of {nameof(ScriptableObject)}");
                return default;
            }
            
            var result = type.FindAssetOfType() as ScriptableObject;

            if (result == null)
            {
                var temp = ScriptableObject.CreateInstance(type);
                
                newPath.CreateDirectory();
                
                if (newName.EndsWith(".asset") == false)
                {
                    newName += ".asset";
                }
                
                AssetDatabase.CreateAsset(temp, Path.Combine(newPath, newName));
                AssetDatabase.Refresh();

                result = type.FindAssetOfType() as ScriptableObject;

                if (result == null)
                {
                    Debug.LogWarning($"种类为:{type}" +
                                     $"的{nameof(ScriptableObject)}在{newPath}/{newName}下创建失败");
                }
            }

            return result;
        }

        #region Find Or Create Scriptable Object At Path

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FindOrCreateScriptableObjectAtPath<T>(this string path) where T : ScriptableObject
        {
            return FindOrCreateScriptableObjectAtPath(typeof(T), path, out _) as T;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ScriptableObject FindOrCreateScriptableObjectAtPath(this Type type, string path)
        {
            return FindOrCreateScriptableObjectAtPath(type, path, out _);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FindOrCreateScriptableObjectAtPath<T>(this string path, out bool isNewlyCreated) where T : ScriptableObject
        {
            return FindOrCreateScriptableObjectAtPath(typeof(T), path, out isNewlyCreated) as T;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ScriptableObject FindOrCreateScriptableObjectAtPath(this Type type, string path, out bool isNewlyCreated)
        {
            if (CheckAssetTypeIsScriptableObject(type) == false)
            {
                isNewlyCreated = false;
                return null;
            }
            
            path = path.MakeAssetPath(".asset");

            if (path.TryGetAssetByPath(type, out var existedAsset))
            {
                isNewlyCreated = false;
                return (ScriptableObject)existedAsset;
            }
            
            var result = type.CreateScriptableObjectAsset(path);

            isNewlyCreated = result != null;

            return result;
        }

        #endregion

        #region Create Scriptable Object Asset

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T CreateScriptableObjectAsset<T>(this string path) where T : ScriptableObject
        {
            var asset = CreateScriptableObjectAsset(typeof(T), path);

            if (asset == null)
            {
                return null;
            }

            if (asset is T result)
            {
                return result;
            }
            
            Debug.LogWarning($"Failed to cast {asset} of type {asset.GetType()} to target type {typeof(T)}");
            
            return null;
        }

        public static ScriptableObject CreateScriptableObjectAsset(this Type type, string path)
        {
            if (CheckAssetTypeIsScriptableObject(type) == false)
            {
                return null;
            }
            
            path = path.MakeAssetPath(".asset");
            
            var directoryPath = path.GetDirectoryPath();
            var absoluteDirectoryPath = directoryPath.ConvertAssetPathToAbsolutePath();

            if (absoluteDirectoryPath.ExistsDirectory())
            {
                if (path.ExistsAssetWithWarning())
                {
                    return null;
                }
            }
            else
            {
                absoluteDirectoryPath.CreateDirectory();
            }
            
            var result = ScriptableObject.CreateInstance(type);

            if (result == null)
            {
                Debug.LogWarning($"Failed to create {type} instance");
                
                return null;
            }
            
            AssetDatabase.CreateAsset(result, path);
            AssetDatabase.Refresh();
            
            if (result == null)
            {
                Debug.LogWarning($"Failed to create {type} asset at {path}");
                return null;
            }
            
            return result;
        }

        #endregion

        #region Utility

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CheckAssetTypeIsScriptableObject(Type type)
        {
            if (type.IsDerivedFrom<ScriptableObject>(false) == false)
            {
                Debug.LogWarning($"{type} is not a subclass of {nameof(ScriptableObject)}");
                return false;
            }
            
            return true;
        }

        #endregion
    }
}