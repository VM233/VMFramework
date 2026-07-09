using VMFramework.Core;

namespace VMFramework.Configuration
{
    public interface IChooserGenerator
    {
        
    }

    public interface IChooserGenerator<out T> : IChooserGenerator
    {
        
    }
}