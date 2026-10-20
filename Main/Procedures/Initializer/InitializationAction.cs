namespace VMFramework.Procedure
{
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

}
