using Sirenix.OdinInspector;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public partial class DebugPanel
    {
        [Button]
        private void AddEntry([GamePrefabID(typeof(IDebugEntry))] string debugEntryID)
        {
            AddEntry(GamePrefabManager.GetGamePrefab<IDebugEntry>(debugEntryID));
        }
    }
}