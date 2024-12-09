using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace VMFramework.Core.Editor
{
    public static class TextureImporterUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetImporter(this Texture2D texture, out TextureImporter importer, out string path)
        {
            if (texture.TryGetAssetPath(out path) == false)
            {
                importer = null;
                return false;
            }
            
            importer = AssetImporter.GetAtPath(path) as TextureImporter;
            return importer!= null;
        }
    }
}