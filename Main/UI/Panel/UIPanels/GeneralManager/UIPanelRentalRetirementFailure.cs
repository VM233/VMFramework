using System;
using System.Threading.Tasks;

namespace VMFramework.UI
{
    public sealed class UIPanelRentalRetirementFailure
    {
        public Task Layout { get; }
        public Exception SourceCause { get; }
        public Exception LayoutFailure { get; }
        public Exception RootReleaseFailure { get; }
        public Exception ReturnFailure { get; }

        internal UIPanelRentalRetirementFailure(Task layout, Exception sourceCause,
            Exception layoutFailure, Exception rootReleaseFailure, Exception returnFailure)
        {
            Layout = layout;
            SourceCause = sourceCause;
            LayoutFailure = layoutFailure;
            RootReleaseFailure = rootReleaseFailure;
            ReturnFailure = returnFailure;
        }

        public static UIPanelRentalRetirementFailure From(Exception failure) =>
            failure.Data[typeof(UIPanelRentalRetirementFailure)] as UIPanelRentalRetirementFailure;
    }
}
