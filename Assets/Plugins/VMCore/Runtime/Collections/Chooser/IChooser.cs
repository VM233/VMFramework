using System;

namespace VMFramework.Core
{
    public interface IChooser : IRandomItemProvider
    {
        public void ResetChooser();
    }

    public interface IChooser<out T> : IChooser, IRandomItemProvider<T>
    {

    }
}