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

        private readonly List<IReadOnlyParameterlessGameEvent> subscribedEvents = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            foreach (var gameEventID in uiCloseGameEventIDs)
            {
                var gameEvent = GameEventManager.Instance.GetGameEventStrictly<IReadOnlyParameterlessGameEvent>(gameEventID);
                gameEvent.AddCallback(Panel.Close, PriorityDefines.TINY);
                subscribedEvents.Add(gameEvent);
            }
        }

        protected override void OnDeinitialize()
        {
            base.OnDeinitialize();
            
            foreach (var gameEvent in subscribedEvents)
            {
                gameEvent.RemoveCallback(Panel.Close);
            }
            subscribedEvents.Clear();
        }
    }
}
