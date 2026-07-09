namespace VMFramework.GameLogicArchitecture
{
    public readonly struct GlobalSettingFileLoaderTargetConfig
    {
        public readonly bool isTarget;

        public readonly int priority;

        public GlobalSettingFileLoaderTargetConfig(bool isTarget, int priority)
        {
            this.isTarget = isTarget;
            this.priority = priority;
        }
    }
}