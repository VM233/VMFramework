namespace VMFramework.Procedure
{
    public enum InitializationOrder
    {
        BeforeInitStart = 0,
        InitStart = 1000,
        PreInit = 2000,
        Init = 3000,
        PostInit = 4000,
        InitComplete = 5000,
        AfterInitComplete = 6000
    }
}