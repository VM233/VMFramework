namespace VMFramework.Procedure
{
    public readonly struct InitializationAction
    {
        public readonly int order;
        public readonly InitActionHandler action;
        public readonly IInitializer initializer;
        
        public InitializationAction(int order, InitActionHandler action, IInitializer initializer)
        {
            this.order = order;
            this.action = action;
            this.initializer = initializer;
        }

        public InitializationAction(InitializationOrder order, InitActionHandler action, IInitializer initializer)
        {
            this.order = (int)order;
            this.action = action;
            this.initializer = initializer;
        }
    }
}