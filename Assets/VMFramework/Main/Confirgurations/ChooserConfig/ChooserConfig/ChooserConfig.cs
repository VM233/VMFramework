using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.OdinExtensions;
using Random = System.Random;

namespace VMFramework.Configuration
{
    [PreviewComposite]
    public abstract class ChooserConfig<TItem> : BaseConfig, IChooserConfig<TItem>
    {
        public abstract TItem GetRandomItem(Random random);

        public abstract IChooser<TItem> GenerateNewChooser();
    }
}