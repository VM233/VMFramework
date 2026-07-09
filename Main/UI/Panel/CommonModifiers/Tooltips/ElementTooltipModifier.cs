using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Localization;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class ElementTooltipModifier : PanelModifier
    {
        [Serializable]
        public class Config
        {
            [IsNotNullOrEmpty]
            public VisualElementPath elementPath = new();

            public LocalizedString text = new();
        }

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        public List<Config> configs = new();

        protected readonly Dictionary<VisualElement, LocalizedString> textLookup = new();
        protected readonly Dictionary<VisualElement, string> openedTooltips = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpen += OnOpen;
            Panel.OnPostClose += OnClose;
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            textLookup.Clear();
            foreach (var config in configs)
            {
                var element = config.elementPath.MandatoryQuery(this.RootVisualElement(), nameof(config.elementPath));
                textLookup[element] = config.text;
            }

            foreach (var element in textLookup.Keys)
            {
                element.RegisterCallback<MouseEnterEvent>(OnMouseEnterElement);
                element.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveElement);
            }
        }

        protected virtual void OnClose(IUIPanel panel)
        {
            foreach (var element in textLookup.Keys)
            {
                element.UnregisterCallback<MouseEnterEvent>(OnMouseEnterElement);
                element.UnregisterCallback<MouseLeaveEvent>(OnMouseLeaveElement);
            }

            textLookup.Clear();
        }

        protected virtual void OnMouseEnterElement(MouseEnterEvent evt)
        {
            var element = (VisualElement)evt.currentTarget;

            if (textLookup.TryGetValue(element, out var localizedText))
            {
                var text = localizedText.GetLocalizedString();
                openedTooltips[element] = text;
                TooltipManager.Instance.Open(text, Panel);
            }
        }

        protected virtual void OnMouseLeaveElement(MouseLeaveEvent evt)
        {
            var element = (VisualElement)evt.currentTarget;

            if (openedTooltips.Remove(element, out var text))
            {
                TooltipManager.Instance.CloseDefault(text);
            }
        }
    }
}