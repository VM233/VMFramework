using Sirenix.OdinInspector;

namespace VMFramework.Maps
{
    public readonly struct CascadedTilemapBaseOrderSettings
    {
        [MinValue(0)]
        public readonly int maxZ;
        public readonly bool enableUniformlySpaced;
        public readonly short baseOrderOffset;
        
        public CascadedTilemapBaseOrderSettings(int maxZ, bool enableUniformlySpaced, short baseOrderOffset)
        {
            this.maxZ = maxZ;
            this.enableUniformlySpaced = enableUniformlySpaced;
            this.baseOrderOffset = baseOrderOffset;
        }
    }
}