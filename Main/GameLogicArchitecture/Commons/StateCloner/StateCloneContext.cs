using System;

namespace VMFramework.GameLogicArchitecture
{
    public readonly struct StateCloneContext
    {
        private readonly ulong tagMask;

        public static StateCloneContext Empty => default;

        public StateCloneContext(ReadOnlySpan<StateCloneTag> tags)
        {
            ulong tagMask = 0;
            for (int i = 0; i < tags.Length; i++)
            {
                var tag = tags[i];
                if (tag.IsValid == false)
                {
                    throw new ArgumentException("State clone tags must be created with StateCloneTag.Create().",
                        nameof(tags));
                }

                tagMask |= tag.Mask;
            }

            this.tagMask = tagMask;
        }

        private StateCloneContext(ulong tagMask)
        {
            this.tagMask = tagMask;
        }

        public bool HasTag(StateCloneTag tag)
        {
            if (tag.IsValid == false)
            {
                throw new ArgumentException("State clone tags must be created with StateCloneTag.Create().",
                    nameof(tag));
            }

            return (tagMask & tag.Mask) != 0;
        }

        public StateCloneContext WithTag(StateCloneTag tag)
        {
            if (tag.IsValid == false)
            {
                throw new ArgumentException("State clone tags must be created with StateCloneTag.Create().",
                    nameof(tag));
            }

            return new StateCloneContext(tagMask | tag.Mask);
        }

        public override string ToString()
        {
            return $"[Tags: 0x{tagMask:X16}]";
        }
    }
}
