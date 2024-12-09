using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.Core;

namespace VMFramework.Maps
{
    public partial class SpriteConfig : BaseConfig
    {
        public ISpritePresetChooserConfig sprite;

        public override void CheckSettings()
        {
            base.CheckSettings();
            
            sprite.AssertIsNotNull(nameof(sprite));
            sprite.CheckSettings();
        }

        protected override void OnInit()
        {
            base.OnInit();
            
            sprite.Init();
        }
    }
}