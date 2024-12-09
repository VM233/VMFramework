#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.Editor
{
    public class TextureImporterPostProcessor : AssetPostprocessor
    {
        private readonly TextureImporterSettings settings = new();
        
        void OnPostprocessTexture(Texture2D texture)
        {
            TextureImporter importer = assetImporter as TextureImporter;

            if (importer == null)
            {
                return;
            }

            if (EditorSetting.TextureImporterGeneralSetting == null)
            {
                return;
            }

            foreach (var configuration in EditorSetting.TextureImporterGeneralSetting.configurations)
            {
                if (configuration.isOn == false)
                {
                    continue;
                }

                if (assetPath.StartsWith(configuration.textureFolder) == false)
                {
                    continue;
                }
                
                importer.ReadTextureSettings(settings);
                
                settings.readable = configuration.isReadable;
                settings.filterMode = configuration.filterMode;

                if (configuration.ignoreSpriteImportMode == false)
                {
                    importer.spriteImportMode = configuration.spriteImportMode;
                }
                
                if (configuration.ignoreSpritePivot == false)
                {
                    settings.spriteAlignment = (int)SpriteAlignment.Custom;
                    settings.spritePivot = configuration.spritePivot.GetRandomItem();
                }
                
                importer.SetTextureSettings(settings);

                TextureImporterPlatformSettings platformSettings = new()
                {
                    format = configuration.textureFormat,
                    textureCompression = configuration.compression
                };

                importer.SetPlatformTextureSettings(platformSettings);

                importer.SaveAndReimport();
            }
        }
    }
}
#endif