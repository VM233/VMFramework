using System.Collections.Generic;
using System.Linq;
using VMFramework.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;

namespace VMFramework.ResourcesManagement
{
    [ManagerCreationProvider(ManagerType.ResourcesCore)]
    public sealed class SpriteManager : ManagerBehaviour<SpriteManager>
    {
        [ShowInInspector]
        private static readonly Dictionary<string, FlippedSprites> spriteCache = new();
        
        [ShowInInspector]
        private static readonly Dictionary<Sprite, string> spriteIDLookup = new();

        [Button]
        public static void ClearCache()
        {
            spriteCache.Clear();
            spriteIDLookup.Clear();
        }

        #region Get Sprite

        public static Sprite GetSprite(string spritePresetID, FlipType2D flipType)
        {
            if (spritePresetID.IsNullOrEmpty())
            {
                return null;
            }
            
            if (spriteCache.TryGetValue(spritePresetID, out var flippedSprites))
            {
                if (flippedSprites.TryGetSprite(flipType, out var existedSprite))
                {
                    return existedSprite;
                }

                var sprite = GamePrefabManager.GetGamePrefabStrictly<SpritePreset>(spritePresetID)
                    .GenerateSprite(flipType);

                var newFlippedSprites = flippedSprites.AddSprite(sprite, flipType);
                
                spriteCache[spritePresetID] = newFlippedSprites;
                
                return sprite;
            }

            if (GamePrefabManager.TryGetGamePrefab(spritePresetID, out SpritePreset spritePreset) == false)
            {
                return null;
            }
            
            var newSprite = spritePreset.GenerateSprite(flipType);

            flippedSprites = new(newSprite, flipType);
            
            spriteCache.Add(spritePresetID, flippedSprites);
            spriteIDLookup.TryAdd(newSprite, spritePresetID);
            
            return newSprite;
        }

        #endregion

        #region Sprite Preset

        public static bool HasSpritePreset(Sprite sprite)
        {
            if (sprite == null)
            {
                return false;
            }
            
            if (spriteIDLookup.TryGetValue(sprite, out var spritePresetID))
            {
                if (GamePrefabManager.ContainsGamePrefab(spritePresetID))
                {
                    return true;
                }
                
                spriteIDLookup.Remove(sprite);
                    
                return false;
            }

            var spritePreset = GamePrefabManager.GetAllGamePrefabs<SpritePreset>()
                .FirstOrDefault(prefab => prefab.sprite == sprite);

            if (spritePreset == null)
            {
                return false;
            }
            
            spriteIDLookup.Add(sprite, spritePreset.id);

            return true;
        }
        
        public static SpritePreset GetSpritePreset(Sprite sprite)
        {
            if (sprite == null)
            {
                return null;
            }
            
            if (spriteIDLookup.TryGetValue(sprite, out var spritePresetID))
            {
                var existedSpritePreset = GamePrefabManager.GetGamePrefab<SpritePreset>(spritePresetID);
                
                if (existedSpritePreset == null)
                {
                    spriteIDLookup.Remove(sprite);
                }
                
                return existedSpritePreset;
            }

            var spritePreset = GamePrefabManager.GetAllGamePrefabs<SpritePreset>()
                .FirstOrDefault(prefab => prefab.sprite == sprite);
            
            if (spritePreset == null)
            {
                return null;
            }
            
            spriteIDLookup.Add(sprite, spritePreset.id);

            return spritePreset;
        }

        #endregion
    }
}
