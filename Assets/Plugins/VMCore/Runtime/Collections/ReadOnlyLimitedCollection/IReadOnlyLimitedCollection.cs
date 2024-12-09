using System.Collections.Generic;

namespace VMFramework.Core
{
    public interface IReadOnlyLimitedCollection<out T> : IReadOnlyCollection<T>
    {
        public int Capacity { get; }
    }
}