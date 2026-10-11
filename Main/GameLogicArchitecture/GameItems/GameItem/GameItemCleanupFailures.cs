using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace VMFramework.GameLogicArchitecture
{
    internal struct GameItemCleanupFailures
    {
        private readonly IGameItem item;
        private readonly string phase;
        private List<Exception> failures;

        public GameItemCleanupFailures(IGameItem item, string phase)
        {
            this.item = item;
            this.phase = phase;
            failures = null;
        }

        public void Capture(Exception failure) => (failures ??= new()).Add(failure);

        public void ThrowIfFailed()
        {
            if (failures == null) return;
            if (failures.Count == 1) ExceptionDispatchInfo.Capture(failures[0]).Throw();
            throw new GameItemCleanupException(item, phase, failures);
        }
    }
}
