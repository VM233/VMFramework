using System.Collections.Generic;

namespace VMFramework.Parameters
{
    public struct PriorityCompositeParameterSource<TValue> : IParameterSource<TValue>
    {
        public List<IParameterSource<TValue>> sources;

        public PriorityCompositeParameterSource(List<IParameterSource<TValue>> sources)
        {
            this.sources = sources;
        }

        public bool TryGetValue(ref TValue value)
        {
            foreach (var source in sources)
            {
                if (source.TryGetValue(out var validValue))
                {
                    value = validValue;
                    return true;
                }
            }

            return false;
        }
    }
}