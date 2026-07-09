using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.GameEvents;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class EventsDisabledOnFocusModifier : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        public List<VisualElementPath> containerPaths = new();

        [TitleGroup(ComponentNames.CONFIG)]
        [ListDrawerSettings(ShowFoldout = false)]
        [GamePrefabID(typeof(IGameEventConfig))]
        public List<string> gameEventsToDisable = new();

        protected EventCallback<FocusInEvent> onFocusInFunc;
        protected EventCallback<FocusOutEvent> onFocusOutFunc;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly List<VisualElement> containers = new();

        protected readonly Dictionary<VisualElement, IToken> tokens = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            onFocusInFunc = OnFocusInElement;
            onFocusOutFunc = OnFocusOutElement;

            Panel.OnOpen += OnOpen;
            Panel.OnPostClose += OnPostClose;
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            if (gameEventsToDisable.IsNullOrEmpty())
            {
                return;
            }

            containers.Clear();
            containers.AddRange(containerPaths.MandatoryQuery(this.RootVisualElement(), nameof(containerPaths)));

            tokens.Clear();
            foreach (var container in containers)
            {
                container.RegisterCallback(onFocusInFunc);
                container.RegisterCallback(onFocusOutFunc);

                var token = new Token
                {
                    Source = this
                };
                tokens[container] = token;
            }
        }

        protected virtual void OnPostClose(IUIPanel panel)
        {
            foreach (var container in containers)
            {
                container.UnregisterCallback(onFocusInFunc);
                container.UnregisterCallback(onFocusOutFunc);
            }

            containers.Clear();

            foreach (var token in tokens.Values)
            {
                GameEventManager.Instance.Enable(gameEventsToDisable, token);
            }
        }

        protected virtual void OnFocusInElement(FocusInEvent evt)
        {
            var target = (VisualElement)evt.currentTarget;
            if (target == null)
            {
                return;
            }

            if (tokens.TryGetValue(target, out var token) == false)
            {
                UnityEngine.Debug.LogError($"Could not find token for {target.name}");
                return;
            }

            GameEventManager.Instance.Disable(gameEventsToDisable, token);
        }

        protected virtual void OnFocusOutElement(FocusOutEvent evt)
        {
            var target = (VisualElement)evt.currentTarget;
            if (target == null)
            {
                return;
            }

            if (tokens.TryGetValue(target, out var token) == false)
            {
                UnityEngine.Debug.LogError($"Could not find token for {target.name}");
                return;
            }

            GameEventManager.Instance.Enable(gameEventsToDisable, token);
        }
    }
}