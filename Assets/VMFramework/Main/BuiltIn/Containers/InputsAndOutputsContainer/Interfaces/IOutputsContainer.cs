using VMFramework.Core;

namespace VMFramework.Containers
{
    public interface IOutputsContainer : IContainer
    {
        public RangeInteger OutputsRange { get; }
    }
}