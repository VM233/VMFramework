using System;

namespace VMFramework.UI
{
    public sealed class UIPanelReleaseFailure
    {
        public string Owner { get; }
        public string Stage { get; }
        public Exception Failure { get; }
        internal UIPanelReleaseFailure(string owner, string stage, Exception failure)
        { Owner = owner; Stage = stage; Failure = failure; }
    }
}
