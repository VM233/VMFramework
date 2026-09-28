using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.GameEvents;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class EventsDisabledOnOpenModifier : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [GamePrefabID(typeof(IGameEventConfig))]
        [ListDrawerSettings(ShowFoldout = false)]
        [DisallowDuplicateElements]
        public List<string> gameEventDisabledOnOpen = new();

        private readonly List<IReadOnlyGameEvent> disabledEvents = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpen += OnOpen;
            Panel.OnPostClose += OnClose;
        }

        protected override void OnDeinitialize()
        {
            Panel.OnOpen -= OnOpen;
            Panel.OnPostClose -= OnClose;
            RetireOpenLifetime();
            base.OnDeinitialize();
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            foreach (var gameEventID in gameEventDisabledOnOpen)
            {
                var gameEvent = GameEventManager.Instance.GetGameEventStrictly(gameEventID);
                gameEvent.IsEnabled.AddToken(Panel);
                disabledEvents.Add(gameEvent);
            }
        }

        protected virtual void OnClose(IUIPanel panel)
        {
            RetireOpenLifetime();
        }

        private void RetireOpenLifetime()
        {
            foreach (var gameEvent in disabledEvents)
            {
                gameEvent.IsEnabled.RemoveToken(Panel);
            }
            disabledEvents.Clear();
        }
    }
}
