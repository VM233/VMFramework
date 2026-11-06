using System;

namespace VMFramework.Procedure
{
    internal interface IInitializationActivity : IDisposable
    {
        bool Active { get; }
        event Action<bool> Changed;
    }
}
