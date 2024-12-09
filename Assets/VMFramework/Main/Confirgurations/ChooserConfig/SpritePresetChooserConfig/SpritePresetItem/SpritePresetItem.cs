using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;
using VMFramework.ResourcesManagement;

namespace VMFramework.Configuration
{
    public sealed partial class SpritePresetItem : BaseConfig, IIDOwner<string>
    {
        private const string SPRITE_PREVIEW_GROUP = "SpritePreview";
        
        private const string SPRITE_PREVIEW_LEFT_GROUP = SPRITE_PREVIEW_GROUP + "/SpritePreviewLeft";
        
        private const string SPRITE_PREVIEW_FLIP_GROUP = SPRITE_PREVIEW_GROUP + "/SpritePreviewLeft/Flip";

        [HideLabel, HorizontalGroup(SPRITE_PREVIEW_GROUP, 200), VerticalGroup(SPRITE_PREVIEW_LEFT_GROUP)]
        [GamePrefabID(typeof(SpritePreset))]
#if UNITY_EDITOR
        [OnValueChanged(nameof(OnIDChanged))]
#endif
        [SerializeField]
        private string spritePresetID;

        public string id
        {
            get => spritePresetID;
            init => spritePresetID = value;
        }

        [field: LabelWidth(50), HorizontalGroup(SPRITE_PREVIEW_FLIP_GROUP, width: 65)]
        [field: SerializeField]
        public bool flipX { get; init; } = false;

        [field: LabelWidth(50), HorizontalGroup(SPRITE_PREVIEW_FLIP_GROUP)]
        [field: SerializeField]
        public bool flipY { get; init; } = false;
        
        public Sprite Sprite => SpriteManager.GetSprite(spritePresetID, (flipX, flipY).ToFlipType2D());

        #region Constructor

        public SpritePresetItem() : this(null) { }

        public SpritePresetItem(Sprite sprite)
        {
            if (sprite == null)
            {
                return;
            }

            if (SpriteManager.HasSpritePreset(sprite) == false)
            {
                ResourcesManagementSetting.SpriteGeneralSetting.AddSpritePreset(sprite);
            }

            var spritePreset = SpriteManager.GetSpritePreset(sprite);

            spritePresetID = spritePreset?.id;
        }

        #endregion

        #region Conversion

        public static implicit operator Sprite(SpritePresetItem item)
        {
            return item?.Sprite;
        }

        #endregion

        public override string ToString()
        {
            return spritePresetID;
        }
    }
}