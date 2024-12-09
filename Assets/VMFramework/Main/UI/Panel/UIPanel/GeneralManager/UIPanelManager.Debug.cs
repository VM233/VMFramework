using Sirenix.OdinInspector;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public partial class UIPanelManager
    {
        [Button]
        private static void _CreateAndOpen([GamePrefabID(typeof(IUIPanelConfig))] string id)
        {
            CreateAndOpen(id);
        }
    }
}