using VMFramework.Properties;

namespace VMFramework.Parameters
{
    public class BaseBoostFloatSeparateParameterSource : IParameterSource<BaseBoostFloat>
    {
        public IParameterSource<float> baseSource;
        public IParameterSource<float> boostSource;

        public BaseBoostFloatSeparateParameterSource(IParameterSource<float> baseSource,
            IParameterSource<float> boostSource)
        {
            this.baseSource = baseSource;
            this.boostSource = boostSource;
        }

        public bool TryGetValue(ref BaseBoostFloat value)
        {
            if (baseSource.TryGetValue(out var baseValue) == false)
            {
                return false;
            }

            if (boostSource.TryGetValue(out var boostValue) == false)
            {
                return false;
            }

            value = new BaseBoostFloat(baseValue, boostValue);
            return true;
        }
    }
}