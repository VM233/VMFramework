using System;
using System.Collections.Generic;

namespace VMFramework.GameLogicArchitecture
{
    public sealed class GameItemCleanupException : AggregateException
    {
        public IGameItem Item { get; }
        public string Phase { get; }

        public GameItemCleanupException(IGameItem item, string phase, IReadOnlyList<Exception> failures)
            : base($"GameItem cleanup failed during {phase}.", failures)
        {
            Item = item;
            Phase = phase;
        }
    }
}
