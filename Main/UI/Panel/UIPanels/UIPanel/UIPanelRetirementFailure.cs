using System;
using System.Collections.Generic;

namespace VMFramework.UI
{
    public sealed class UIPanelRetirementFailure
    {
        public UIPanelRetirementFailure Previous { get; }
        public IReadOnlyList<UIPanelReleaseFailure> Releases { get; }
        internal UIPanelRetirementFailure(UIPanelRetirementFailure previous, UIPanelReleaseFailure[] releases)
        { Previous = previous; Releases = Array.AsReadOnly(releases); }
        public static UIPanelRetirementFailure From(Exception error) =>
            error.Data[typeof(UIPanelRetirementFailure)] as UIPanelRetirementFailure;
    }
}
