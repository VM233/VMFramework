using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class EmptyListViewAutoHideModifier : PanelModifier
    {
        [Serializable]
        public class Config
        {
            [VisualElementName]
            [IsNotNullOrEmpty]
            public string elementName;

            [VisualElementName]
            [IsNotNullOrEmpty]
            public string containerName;
        }

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        public List<Config> configs = new();

        protected readonly List<(VisualElement element, VisualElement container)> elements = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpen += OnOpen;
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            elements.Clear();
            foreach (var config in configs)
            {
                var element = this.RootVisualElement().QueryStrictly(config.elementName, nameof(config.elementName));
                var container = this.RootVisualElement()
                    .QueryStrictly(config.containerName, nameof(config.containerName));
                if (element == null || container == null)
                {
                    continue;
                }

                elements.Add((element, container));
            }
        }

        protected virtual void Update()
        {
            if (Panel.IsOpened == false)
            {
                return;
            }

            foreach (var (element, container) in elements)
            {
                var display = container.childCount > 0;
                element.Display(display);
            }
        }
    }
}