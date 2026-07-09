namespace VMFramework.Core.JSON
{
    public struct ControllerWrapper<TController>
    {
        public TController gameObject;

        public ControllerWrapper(TController gameObject)
        {
            this.gameObject = gameObject;
        }
    }
}