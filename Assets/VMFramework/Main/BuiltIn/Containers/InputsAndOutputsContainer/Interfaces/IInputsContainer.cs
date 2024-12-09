using VMFramework.Core;

namespace VMFramework.Containers
{
    public interface IInputsContainer : IContainer
    {
        public RangeInteger InputsRange { get; }
    }
}