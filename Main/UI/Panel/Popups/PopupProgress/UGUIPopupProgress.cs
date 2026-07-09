using VMFramework.Core;
using Sirenix.OdinInspector;
using VMFramework.UI.Components;

namespace VMFramework.UI
{
    public class UGUIPopupProgress : UGUIPanel
    {
        protected UGUIPopupProgressConfig UGUIPopupProgressConfig => (UGUIPopupProgressConfig)GamePrefab;

        [ShowInInspector]
        public ProgressUIComponent Progress { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            
            Progress = UIMainRectTransform.QueryFirstComponentInChildren<ProgressUIComponent>(
                UGUIPopupProgressConfig.progressName, true);

            Progress.AssertIsNotNull(nameof(Progress));
        }
    }
}
