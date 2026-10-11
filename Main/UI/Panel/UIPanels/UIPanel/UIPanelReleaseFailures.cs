using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace VMFramework.UI
{
    internal struct UIPanelReleaseFailures
    {
        private readonly string owner;
        private Exception primary;
        private List<UIPanelReleaseFailure> failures;
        public Exception PrimaryFailure => primary;
        public bool HasReleaseFailures => failures != null;

        public UIPanelReleaseFailures(string owner, Exception primary = null)
        { this.owner = owner; this.primary = primary; failures = null; }

        public void Capture(string stage, Exception error)
        {
            primary ??= error;
            (failures ??= new()).Add(new UIPanelReleaseFailure(owner, stage, error));
        }

        public void Attempt(string stage, Action release)
        { try { release(); } catch (Exception error) { Capture(stage, error); } }

        public void ThrowIfFailed()
        {
            if (primary == null) return;
            if (failures != null)
                primary.Data[typeof(UIPanelRetirementFailure)] = new UIPanelRetirementFailure(
                    UIPanelRetirementFailure.From(primary), failures.ToArray());
            ExceptionDispatchInfo.Capture(primary).Throw();
        }

        public void ThrowIfReleaseFailed()
        {
            if (failures != null) ThrowIfFailed();
        }
    }
}
