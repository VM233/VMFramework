namespace VMFramework.Procedure
{
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

        internal void MarkSucceeded() => Status = InitializationActionStatus.Succeeded;

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
