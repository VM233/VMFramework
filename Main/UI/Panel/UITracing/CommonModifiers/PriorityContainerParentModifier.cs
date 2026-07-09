using System;
using System.Collections.Generic;
using System.Linq;
using EnumsNET;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class PriorityContainerParentModifier : PanelModifier
    {
        [Serializable]
        public class ContainerConfig
        {
            public VisualElementPath containerPath = new();

            public FourTypesDirection nonsupportOverflowDirections = FourTypesDirection.None;
        }

        [TitleGroup(ComponentNames.CONFIG)]
        [VisualElementName]
        [IsNotNullOrEmpty]
        public string targetElementName;

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        public List<ContainerConfig> containerConfigs = new();

        [TitleGroup(ComponentNames.CONFIG)]
        [MinValue(0)]
        public float maxOverflow = 10f;

        protected VisualElement targetElement;

        protected readonly List<VisualElement> containers = new();
        protected readonly Dictionary<VisualElement, FourTypesDirection> nonsupportOverflowDirections = new();

        protected VisualElement currentContainer;

        protected readonly HashSet<VisualElement> invalidContainers = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpen += OnOpen;
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            targetElement = this.RootVisualElement().QueryStrictly(targetElementName, nameof(targetElementName));

            containers.Clear();
            nonsupportOverflowDirections.Clear();
            foreach (var containerConfig in containerConfigs)
            {
                var container = containerConfig.containerPath.MandatoryQuery(this.RootVisualElement(),
                    nameof(containerConfig.containerPath));
                containers.Add(container);
                nonsupportOverflowDirections.Add(container, containerConfig.nonsupportOverflowDirections);
            }

            invalidContainers.Clear();

            currentContainer = null;
        }

        protected void Update()
        {
            if (Panel.IsOpened == false)
            {
                return;
            }

            if (containers.Count == 0)
            {
                return;
            }

            if (targetElement.worldBound.size.x <= 0 || targetElement.worldBound.size.y <= 0)
            {
                return;
            }

            if (currentContainer == null)
            {
                var firstContainer = containers[0];
                firstContainer.Add(targetElement);
                currentContainer = firstContainer;
                return;
            }

            var screenRect = new RectangleFloat(targetElement.panel.visualTree.worldBound);
            screenRect.Expand(maxOverflow);

            var elementRect = new RectangleFloat(targetElement.worldBound);

            if (screenRect.Contains(elementRect, out var overflowDirections))
            {
                invalidContainers.Clear();
                return;
            }

            invalidContainers.Add(currentContainer);

            bool anyValidContainer = false;
            foreach (var container in containers)
            {
                if (invalidContainers.Contains(container))
                {
                    continue;
                }

                if (nonsupportOverflowDirections.TryGetValue(container, out var nonsupportedDirections))
                {
                    if (nonsupportedDirections.HasAnyFlags(overflowDirections))
                    {
                        continue;
                    }
                }

                currentContainer = container;
                anyValidContainer = true;
                break;
            }

            if (anyValidContainer == false)
            {
                invalidContainers.Clear();
                currentContainer = containers.First();
            }

            currentContainer.Add(targetElement);
        }
    }
}