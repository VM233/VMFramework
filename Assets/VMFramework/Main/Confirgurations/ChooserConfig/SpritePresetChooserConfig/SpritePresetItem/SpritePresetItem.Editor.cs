#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.ResourcesManagement;

namespace VMFramework.Configuration
{
    public partial class SpritePresetItem
    {
        [HideLabel, HorizontalGroup(SPRITE_PREVIEW_GROUP)]
        [PreviewField(40, ObjectFieldAlignment.Right)]
        [OnValueChanged(nameof(OnSpriteChanged))]
        [ShowInInspector]
        private Sprite _spriteCache;

        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            if (spritePresetID.IsNullOrEmpty() == false)
            {
                _spriteCache = SpriteManager.GetSprite(spritePresetID, (flipX, flipY).ToFlipType2D());
            }
        }

        private void OnSpriteChanged()
        {
            if (_spriteCache == null)
            {
                spritePresetID = null;
                return;
            }

            if (SpriteManager.HasSpritePreset(_spriteCache) == false)
            {
                ResourcesManagementSetting.SpriteGeneralSetting.AddSpritePreset(_spriteCache);
            }

            EditorApplication.delayCall += () =>
            {
                spritePresetID = SpriteManager.GetSpritePreset(_spriteCache)?.id;
            };
        }

        private void OnIDChanged()
        {
            if (spritePresetID.IsNullOrEmpty())
            {
                _spriteCache = null;
                return;
            }
            
            _spriteCache = SpriteManager.GetSprite(spritePresetID, (flipX, flipY).ToFlipType2D());
        }
    }
}
#endif