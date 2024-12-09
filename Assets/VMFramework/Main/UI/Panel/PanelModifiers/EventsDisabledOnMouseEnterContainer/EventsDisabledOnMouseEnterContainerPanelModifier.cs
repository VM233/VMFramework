using System;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.GameEvents;

namespace VMFramework.UI
{
    public sealed class EventsDisabledOnMouseEnterContainerPanelModifier : UIToolkitPanelModifier, IToken
    {
        private EventsDisabledOnMouseEnterContainerPanelModifierConfig ModifierConfig =>
            (EventsDisabledOnMouseEnterContainerPanelModifierConfig)GamePrefab;

        private EventCallback<MouseEnterEvent> onMouseEnterFunc;
        private EventCallback<MouseLeaveEvent> onMouseLeaveFunc;

        [ShowInInspector]
        private VisualElement container;

        protected override void OnCreate()
        {
            base.OnCreate();

            onMouseEnterFunc = OnMouseEnterElement;
            onMouseLeaveFunc = OnMouseLeaveElement;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpenEvent += OnOpen;
            Panel.OnPostCloseEvent += OnClose;
        }

        private void OnOpen(IUIPanel panel)
        {
            if (ModifierConfig.gameEventsDisabledOnMouseEnter.IsNullOrEmpty())
            {
                return;
            }

            container = RootVisualElement.QueryStrictly(ModifierConfig.containerName,
                nameof(ModifierConfig.containerName));

            container.RegisterCallback(onMouseEnterFunc);
            container.RegisterCallback(onMouseLeaveFunc);
        }

        private void OnClose(IUIPanel panel)
        {
            container.UnregisterCallback(onMouseEnterFunc);
            container.UnregisterCallback(onMouseLeaveFunc);

            OnMouseLeaveElement(null);

            container = null;
        }

        private void OnMouseEnterElement(MouseEnterEvent evt)
        {
            GameEventManager.Disable(ModifierConfig.gameEventsDisabledOnMouseEnter, this);
        }

        private void OnMouseLeaveElement(MouseLeaveEvent evt)
        {
            GameEventManager.Enable(ModifierConfig.gameEventsDisabledOnMouseEnter, this);
        }
    }
}