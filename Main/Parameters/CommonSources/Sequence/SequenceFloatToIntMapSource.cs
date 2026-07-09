using System.Diagnostics.CodeAnalysis;
using VMFramework.Core;

namespace VMFramework.Parameters
{
    public struct SequenceFloatToIntMapSource : IParameterSource<int>
    {
        public IParameterSource<float> floatSource;

        public float floatOffset;

        public float floatScale;

        public int intOffset;

        public int intScale;

        public SequenceFloatToIntMapSource([DisallowNull] IParameterSource<float> floatSource, float floatOffset,
            float floatScale, int intOffset, int intScale)
        {
            this.floatSource = floatSource;
            this.floatOffset = floatOffset;
            this.floatScale = floatScale;
            this.intOffset = intOffset;
            this.intScale = intScale;
        }

        public bool TryGetValue(ref int value)
        {
            if (floatSource.TryGetValue(out var floatValue) == false)
            {
                return false;
            }

            var floatIndex = (floatValue + floatOffset) * floatScale;
            var index = floatIndex.Round();
            value = (index + intOffset) * intScale;
            return true;
        }
    }
}