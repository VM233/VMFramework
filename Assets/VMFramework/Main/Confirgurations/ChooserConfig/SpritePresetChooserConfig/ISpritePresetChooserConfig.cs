namespace VMFramework.Configuration
{
    public interface ISpritePresetChooserConfig : IWrapperChooserConfig<SpritePresetItem, SpritePresetItem>
    {
        public ISpritePresetChooserConfig GetFlipChooserConfig(bool flipXReversed, bool flipYReversed);
    }
}