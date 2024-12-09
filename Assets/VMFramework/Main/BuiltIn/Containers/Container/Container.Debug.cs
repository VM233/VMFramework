#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.OdinExtensions;

namespace VMFramework.Containers
{
    public partial class Container
    {
        [Button]
        private void AddItem([HideLabel] [GamePrefabID(typeof(IContainerItemConfig))] string id, int amount = 1)
        {
            var item = ContainerItemFactory.Create(id, amount);
            this.TryAddItem(item);
        }

        [Button]
        private void _StackItems()
        {
            StackItems();
        }

        [Button]
        private void _Compress()
        {
            Compress();
        }

        [Button]
        private void _Shuffle()
        {
            Shuffle();
        }
    }
}
#endif