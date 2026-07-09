#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;

namespace VMFramework.Editor.BatchProcessor
{
    public static class ObjectsNamingUtility
    {
        public static string GetTargetObjectPath<TObject>(this IReadOnlyCollection<TObject> objects)
            where TObject : Object
        {
            if (SelectionUtility.TryGetSelectedFolderPath(out var selectedFolderPath) == false)
            {
                selectedFolderPath = objects.First().GetAssetFolderPath();
            }

            string targetName;

            if (objects.Count > 1)
            {
                var names = objects.Select(obj => obj.name).ToList();
                var sameWords = names.GetSameWords();

                if (sameWords.Count > 0)
                {
                    targetName = sameWords.ToPascalCase(" ");
                }
                else
                {
                    targetName = objects.First().name;
                }
            }
            else
            {
                targetName = objects.First().name.ToPascalCase(" ");
            }
            
            var path = selectedFolderPath.PathCombine(targetName);
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            
            return path;
        }
    }
}
#endif