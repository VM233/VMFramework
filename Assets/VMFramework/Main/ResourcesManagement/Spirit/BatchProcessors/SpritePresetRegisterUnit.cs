#if UNITY_EDITOR
using UnityEngine;
using VMFramework.Core;
using VMFramework.Editor.BatchProcessor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.ResourcesManagement
{
    internal sealed class SpritePresetRegisterUnit : GamePrefabRegisterUnit<Sprite>
    {
        protected override string ProcessButtonName => "Register SpritePresets";

        protected override IGamePrefab OnProcessAsset(Sprite sprite)
        {
            if (checkUnique)
            {
                if (SpriteManager.HasSpritePreset(sprite))
                {
                    Debugger.LogWarning($"Ignore adding {sprite.name} because it is already registered");
                    return null;
                }
            }
            
            var id = sprite.name.ToSnakeCase();
            
            if (GamePrefabManager.ContainsGamePrefab(id))
            {
                Debugger.LogWarning($"Sprite Preset with id {id} already exists.");
                return null;
            }
            
            var spritePreset = new SpritePreset()
            {
                id = id,
                sprite = sprite
            };
            
            return spritePreset;
        }
    }
}
#endif