using VMFramework.Core;

namespace VMFramework.Parameters
{
    public class FloatCompareParameterSource : IParameterSource<bool>
    {
        public IParameterSource<float> source;

        public IParameterSource<float> comparand;

        public CompareResult compareResult;

        public FloatCompareParameterSource(IParameterSource<float> source, IParameterSource<float> comparand,
            CompareResult compareResult)
        {
            this.source = source;
            this.comparand = comparand;
            this.compareResult = compareResult;
        }

        public bool TryGetValue(ref bool value)
        {
            if (source.TryGetValue(out var sourceValue) == false)
            {
                return false;
            }

            if (comparand.TryGetValue(out var comparandValue) == false)
            {
                return false;
            }

            value = sourceValue.SatisfyCompareResult(comparandValue, compareResult);
            return true;
        }
    }
}