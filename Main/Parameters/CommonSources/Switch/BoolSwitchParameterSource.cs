using System.Diagnostics.CodeAnalysis;

namespace VMFramework.Parameters
{
    public struct BoolSwitchParameterSource<TValue> : IParameterSource<TValue>
    {
        public IParameterSource<bool> boolSource;
        public IParameterSource<TValue> trueSource;
        public IParameterSource<TValue> falseSource;

        public BoolSwitchParameterSource([DisallowNull] IParameterSource<bool> boolSource,
            IParameterSource<TValue> trueSource, IParameterSource<TValue> falseSource)
        {
            this.boolSource = boolSource;
            this.trueSource = trueSource;
            this.falseSource = falseSource;
        }

        public bool TryGetValue(ref TValue value)
        {
            if (boolSource.TryGetValue(out bool boolValue) == false)
            {
                return false;
            }

            if (boolValue)
            {
                if (trueSource != null && trueSource.TryGetValue(out var trueValue))
                {
                    value = trueValue;
                    return true;
                }
            }
            else
            {
                if (falseSource != null && falseSource.TryGetValue(out var falseValue))
                {
                    value = falseValue;
                    return true;
                }
            }

            return false;
        }
    }
}