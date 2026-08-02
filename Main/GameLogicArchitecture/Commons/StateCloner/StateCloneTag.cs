using System;
using System.Threading;

namespace VMFramework.GameLogicArchitecture
{
    public readonly struct StateCloneTag : IEquatable<StateCloneTag>
    {
        private const int MAX_TAG_COUNT = sizeof(ulong) * 8;

        private static int nextBitIndex = -1;

        internal ulong Mask { get; }

        public bool IsValid => Mask != 0;

        private StateCloneTag(ulong mask)
        {
            Mask = mask;
        }

        /// <summary>
        /// Creates one process-local tag. Store the result in a static field and reuse it.
        /// </summary>
        public static StateCloneTag Create()
        {
            var bitIndex = Interlocked.Increment(ref nextBitIndex);
            if (bitIndex >= MAX_TAG_COUNT)
            {
                throw new InvalidOperationException(
                    $"A maximum of {MAX_TAG_COUNT} state clone tags can be registered.");
            }

            return new StateCloneTag(1UL << bitIndex);
        }

        public bool Equals(StateCloneTag other)
        {
            return Mask == other.Mask;
        }

        public override bool Equals(object obj)
        {
            return obj is StateCloneTag other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Mask.GetHashCode();
        }

        public static bool operator ==(StateCloneTag left, StateCloneTag right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StateCloneTag left, StateCloneTag right)
        {
            return left.Equals(right) == false;
        }
    }
}
