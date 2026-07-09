namespace VMFramework.Parameters
{
    public class BranchParameterSource<TValue> : IParameterSource<TValue>
    {
        public IParameterSource<TValue> trueSource;
        public IParameterSource<TValue> falseSource;
        public IParameterSource<bool> condition;

        public BranchParameterSource(IParameterSource<TValue> trueSource, IParameterSource<TValue> falseSource,
            IParameterSource<bool> condition)
        {
            this.trueSource = trueSource;
            this.falseSource = falseSource;
            this.condition = condition;
        }

        public bool TryGetValue(ref TValue value)
        {
            if (condition.TryGetValue(out var conditionValue) == false)
            {
                return false;
            }

            if (conditionValue == false)
            {
                return falseSource.TryGetValue(ref value);
            }

            return trueSource.TryGetValue(ref value);
        }
    }
}