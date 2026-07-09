using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.GameEvents;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public sealed class UICloseOnEventTriggeredModifier : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [ListDrawerSettings(ShowFoldout = false)]
        [GamePrefabID(typeof(IGameEventConfig))]
        [DisallowDuplicateElements]
        public List<string> uiCloseGameEventIDs = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            foreach (var gameEventID in uiCloseGameEventIDs)
            {
                GameEventManager.Instance.AddCallback(gameEventID, Panel.Close, PriorityDefines.TINY);
            }
        }

        protected override void OnDeinitialize()
        {
            base.OnDeinitialize();
            
            foreach (var gameEventID in uiCloseGameEventIDs)
            {
                GameEventManager.Instance.RemoveCallback(gameEventID, Panel.Close);
            }
        }
    }
}