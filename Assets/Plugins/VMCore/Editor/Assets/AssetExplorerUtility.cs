using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core.Linq;

namespace VMFramework.Core.Editor
{
    public static class AssetExplorerUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OpenInExplorer(this Object obj)
        {
            if (obj.TryGetAssetAbsolutePathWithWarning(out var absolutePath) == false)
            {
                return;
            }
            
            absolutePath.OpenFileInExplorer();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OpenInExplorer(this IEnumerable<Object> objects)
        {
            objects.Examine(OpenInExplorer);
        }
    }
}