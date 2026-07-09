namespace VMFramework.Parameters
{
    public class ConditionalParameterSource<TValue> : IParameterSource<TValue>
    {
        public IParameterSource<TValue> source;
        public IParameterSource<bool> condition;

        public ConditionalParameterSource(IParameterSource<TValue> source, IParameterSource<bool> condition)
        {
            this.source = source;
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
                return false;
            }

            return source.TryGetValue(ref value);
        }
    }
}