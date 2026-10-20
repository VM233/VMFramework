namespace VMFramework.Procedure
{
    public enum InitializationActionStatus
    {
        Pending,
        Running,
        Succeeded,
        Failed,
        Canceled,
    }

    public readonly struct InitializationAction
    {
        public readonly int order;
        public readonly InitActionHandler action;
        public readonly IInitializer initializer;
        public readonly IInitializationProgressSource progressSource;
        
        public InitializationAction(int order, InitActionHandler action, IInitializer initializer,
            IInitializationProgressSource progressSource = null)
        {
            this.order = order;
            this.action = action ?? throw new System.ArgumentNullException(nameof(action));
            this.initializer = initializer ?? throw new System.ArgumentNullException(nameof(initializer));
            this.progressSource = progressSource;
        }

        public InitializationAction(InitializationOrder order, InitActionHandler action, IInitializer initializer,
            IInitializationProgressSource progressSource = null) : this((int)order, action, initializer, progressSource)
        {
        }
    }

    public sealed class InitializationActionExecution
    {
        public InitializationAction InitializationAction { get; }

        public InitializationActionStatus Status { get; private set; } = InitializationActionStatus.Pending;

        public System.Exception Exception { get; private set; }

        internal InitializationActionExecution(InitializationAction initializationAction)
        {
            InitializationAction = initializationAction;
        }

        internal void MarkRunning()
        {
            Status = InitializationActionStatus.Running;
            Exception = null;
        }

        internal void MarkSucceeded()
        {
            Status = InitializationActionStatus.Succeeded;
        }

        internal void MarkFailed(System.Exception exception)
        {
            Status = InitializationActionStatus.Failed;
            Exception = exception;
        }

        internal void MarkCanceled(System.OperationCanceledException exception)
        {
            Status = InitializationActionStatus.Canceled;
            Exception = exception;
        }
    }
}
