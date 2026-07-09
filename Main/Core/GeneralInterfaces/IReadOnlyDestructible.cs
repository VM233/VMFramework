using System;

namespace VMFramework.Core
{
    public interface IReadOnlyDestructible
    {
        public bool IsDestroyed { get; }

        public event Action<IReadOnlyDestructible> OnDestructed;
    }
}