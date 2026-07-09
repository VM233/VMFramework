using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class ReenterOnCloseModifier : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [GamePrefabID(typeof(IUIPanelConfig))]
        [UIPanelConfigID(isUnique: true)]
        [IsNotNullOrEmpty]
        public List<string> panelIDs = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnPreClose += OnPreClose;
        }

        protected virtual void OnPreClose(IUIPanel panel)
        {
            foreach (var panelID in panelIDs)
            {
                UIPanelManager.Instance.Reenter(panelID);
            }
        }
    }
}