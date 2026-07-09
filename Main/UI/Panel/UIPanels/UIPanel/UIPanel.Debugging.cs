#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Core;

namespace VMFramework.UI
{
    public partial class UIPanel
    {
        [TitleGroup(ComponentNames.RUNTIME)]
        [Button]
        private void _Toggle()
        {
            this.Toggle();
        }

        [TitleGroup(ComponentNames.RUNTIME)]
        [Button]
        private void _Open()
        {
            this.Open(null);
        }
        
        [TitleGroup(ComponentNames.RUNTIME)]
        [Button]
        private void _Close()
        {
            this.Close();
        }
    }
}
#endif