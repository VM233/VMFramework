using UnityEngine.UIElements;

namespace VMFramework.UI
{
    public interface IVisualTreeAssetProvider
    {
        public VisualTreeAsset VisualTree { get; }
    }
}