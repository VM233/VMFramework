using System.Collections.Generic;
using VMFramework.Core.Linq;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Parameters
{
    public class TaggedParameterSource<TValue> : IParameterSource<TValue>
        where TValue : IGameTagsOwner
    {
        public IParameterSource<TValue> source;
        public List<string> validTags;

        public TaggedParameterSource(IParameterSource<TValue> source, List<string> validTags)
        {
            this.source = source;
            this.validTags = validTags;
        }

        public bool TryGetValue(ref TValue value)
        {
            if (source.TryGetValue(out var validValue) == false || validValue == null)
            {
                return false;
            }

            if (validTags.IsNullOrEmpty() == false)
            {
                if (validValue.HasAnyTags(validTags) == false)
                {
                    return false;
                }
            }

            value = validValue;
            return true;
        }
    }
}