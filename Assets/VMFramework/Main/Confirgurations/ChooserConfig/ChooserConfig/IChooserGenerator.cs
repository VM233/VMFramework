using VMFramework.Core;

namespace VMFramework.Configuration
{
    public interface IChooserGenerator
    {
        public IChooser GenerateNewObjectChooser();
    }

    public interface IChooserGenerator<out T> : IChooserGenerator
    {
        public IChooser<T> GenerateNewChooser();

        IChooser IChooserGenerator.GenerateNewObjectChooser()
        {
            return GenerateNewChooser();
        }
    }
}