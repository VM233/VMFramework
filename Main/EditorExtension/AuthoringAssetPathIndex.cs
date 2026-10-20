#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;

namespace VMFramework.Editor
{
    internal sealed class AuthoringAssetPathIndex
    {
        private readonly Dictionary<string, bool> paths = new(StringComparer.Ordinal);

        internal void Clear() => paths.Clear();

        internal void Add(UnityEngine.Object source)
        {
            string path = AssetDatabase.GetAssetPath(source);
            if (path.Length == 0) return;
            paths[path] = true;
            int separator;
            while ((separator = path.LastIndexOf('/')) >= 0)
            {
                path = path.Substring(0, separator);
                paths.TryAdd(path, false);
            }
        }

        internal bool ContainsSource(string path) => paths.TryGetValue(path, out bool source) && source;

        internal bool ContainsSourceOrAncestor(string path) => paths.ContainsKey(path);

        internal bool ContainsAnySources(IEnumerable<string> changes) => ContainsAny(changes, false);

        internal bool ContainsAnyRemovedSources(IEnumerable<string> changes) => ContainsAny(changes, true);

        private bool ContainsAny(IEnumerable<string> changes, bool includesAncestors)
        {
            foreach (string path in changes)
                if (paths.TryGetValue(path, out bool source) && (source || includesAncestors)) return true;
            return false;
        }
    }
}
#endif
