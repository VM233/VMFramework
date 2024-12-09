#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Core;

namespace VMFramework.UI
{
    public partial class UIPanel
    {
        [Button]
        private void _Toggle()
        {
            this.Toggle();
        }

        [Button]
        private void _Open()
        {
            this.Open(null);
        }
        
        [Button]
        private void _Close()
        {
            this.Close();
        }
    }
}
#endif