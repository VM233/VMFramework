using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.GameEvents;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class EventsDisabledOnMouseEnterContainerModifier : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        public List<VisualElementPath> containerPaths = new();

        [TitleGroup(ComponentNames.CONFIG)]
        [ListDrawerSettings(ShowFoldout = false)]
        [GamePrefabID(typeof(IGameEventConfig))]
        public List<string> gameEventsDisabledOnMouseEnter = new();

        protected EventCallback<MouseEnterEvent> onMouseEnterFunc;
        protected EventCallback<MouseLeaveEvent> onMouseLeaveFunc;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly List<VisualElement> containers = new();

        protected readonly Dictionary<VisualElement, IToken> tokens = new();

        private readonly List<IReadOnlyGameEvent> disabledEvents = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            onMouseEnterFunc = OnMouseEnterElement;
            onMouseLeaveFunc = OnMouseLeaveElement;

            Panel.OnOpen += OnOpen;
            Panel.OnPostClose += OnPostClose;
        }

        protected override void OnDeinitialize()
        {
            Panel.OnOpen -= OnOpen;
            Panel.OnPostClose -= OnPostClose;
            RetireOpenLifetime();
            base.OnDeinitialize();
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            if (gameEventsDisabledOnMouseEnter.IsNullOrEmpty())
            {
                return;
            }

            containers.Clear();
            containers.AddRange(containerPaths.MandatoryQuery(this.RootVisualElement(), nameof(containerPaths)));

            foreach (var gameEventID in gameEventsDisabledOnMouseEnter)
            {
                disabledEvents.Add(GameEventManager.Instance.GetGameEventStrictly(gameEventID));
            }

            tokens.Clear();
            foreach (var container in containers)
            {
                container.RegisterCallback(onMouseEnterFunc);
                container.RegisterCallback(onMouseLeaveFunc);

                var token = new Token
                {
                    Source = this
                };
                tokens[container] = token;
            }
        }

        protected virtual void OnPostClose(IUIPanel panel)
        {
            RetireOpenLifetime();
        }

        private void RetireOpenLifetime()
        {
            foreach (var container in containers)
            {
                container.UnregisterCallback(onMouseEnterFunc);
                container.UnregisterCallback(onMouseLeaveFunc);
            }

            containers.Clear();

            foreach (var token in tokens.Values)
            {
                foreach (var gameEvent in disabledEvents)
                {
                    gameEvent.IsEnabled.RemoveToken(token);
                }
            }
            tokens.Clear();
            disabledEvents.Clear();
        }

        protected virtual void OnMouseEnterElement(MouseEnterEvent evt)
        {
            var target = (VisualElement)evt.target;
            if (target == null)
            {
                return;
            }

            if (tokens.TryGetValue(target, out var token) == false)
            {
                UnityEngine.Debug.LogError($"Could not find token for {target.name}");
                return;
            }

            foreach (var gameEvent in disabledEvents)
            {
                gameEvent.IsEnabled.AddToken(token);
            }
        }

        protected virtual void OnMouseLeaveElement(MouseLeaveEvent evt)
        {
            var target = (VisualElement)evt.target;
            if (target == null)
            {
                return;
            }

            if (tokens.TryGetValue(target, out var token) == false)
            {
                UnityEngine.Debug.LogError($"Could not find token for {target.name}");
                return;
            }

            foreach (var gameEvent in disabledEvents)
            {
                gameEvent.IsEnabled.RemoveToken(token);
            }
        }
    }
}
