#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Core;

namespace VMFramework.Containers
{
    public abstract partial class ContainerItem
    {
        [TitleGroup(ComponentNames.RUNTIME)]
        [HideInEditorMode]
        [Button]
        private void RemoveFromContainer()
        {
            if (SourceContainer != null)
            {
                SourceContainer.SetItem(SlotIndex, null);
                this.ReturnIfNoOwner();
            }
        }
    }
}
#endif