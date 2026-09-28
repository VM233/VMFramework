using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.GameEvents;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    [RequireComponent(typeof(IUIToolkitPanel))]
    public class UIToolkitContainerToggleOnEventTriggeredModifier : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [VisualElementName]
        [IsNotNullOrEmpty]
        [DisallowDuplicateElements]
        public List<string> containersName = new();
        
        [TitleGroup(ComponentNames.CONFIG)]
        [ListDrawerSettings(ShowFoldout = false)]
        [GamePrefabID(typeof(IGameEventConfig))]
        [DisallowDuplicateElements]
        public List<string> containerToggleGameEventIDs = new();

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly List<VisualElement> containers = new();

        private readonly List<IReadOnlyParameterlessGameEvent> subscribedEvents = new();

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
            containers.Clear();
            foreach (var containerName in containersName)
            {
                var container = this.RootVisualElement().QueryStrictly(containerName, nameof(containerName));
                containers.Add(container);
            }

            foreach (var gameEventID in containerToggleGameEventIDs)
            {
                var gameEvent = GameEventManager.Instance.GetGameEventStrictly<IReadOnlyParameterlessGameEvent>(gameEventID);
                gameEvent.AddCallback(OnContainerToggle, PriorityDefines.TINY);
                subscribedEvents.Add(gameEvent);
            }
        }

        protected virtual void OnClose(IUIPanel panel)
        {
            RetireOpenLifetime();
        }

        private void RetireOpenLifetime()
        {
            foreach (var gameEvent in subscribedEvents)
            {
                gameEvent.RemoveCallback(OnContainerToggle);
            }
            subscribedEvents.Clear();
            containers.Clear();
        }
        
        protected virtual void OnContainerToggle()
        {
            foreach (var container in containers)
            {
                container.ToggleDisplay();
            }
        }
    }
}
