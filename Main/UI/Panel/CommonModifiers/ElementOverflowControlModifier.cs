using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class ElementOverflowControlModifier : PanelModifier
    {
        public enum ContainerMode
        {
            Panel,
            Parent,
            Custom
        }

        [TitleGroup(ComponentNames.CONFIG)]
        public ContainerMode containerMode = ContainerMode.Panel;

        [TitleGroup(ComponentNames.CONFIG)]
        [ShowIf(nameof(containerMode), ContainerMode.Custom)]
        [IsNotNullOrEmpty]
        public VisualElementPath containerPath = new();

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        public VisualElementPath targetPath = new();

        [TitleGroup(ComponentNames.CONFIG)]
        public bool xMinOverflow = false;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool xMaxOverflow = false;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool yMinOverflow = false;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool yMaxOverflow = false;

        [TitleGroup(ComponentNames.CONFIG)]
        public int refreshAfterUpdateCount = 2;

        protected VisualElement target;
        protected VisualElement container;

        protected int updateCount = 0;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpen += OnOpen;
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            target = targetPath.MandatoryQuery(this.RootVisualElement(), nameof(targetPath));
            target.style.translate = new StyleTranslate(StyleKeyword.None);

            if (containerMode is ContainerMode.Custom)
            {
                container = containerPath.MandatoryQuery(this.RootVisualElement(), nameof(containerPath));
            }

            updateCount = 0;
        }

        protected virtual void Update()
        {
            if (Panel.IsOpened == false)
            {
                return;
            }

            if (target == null)
            {
                return;
            }

            updateCount++;

            if (updateCount <= refreshAfterUpdateCount)
            {
                return;
            }

            Rect containerRect;

            if (containerMode is ContainerMode.Panel)
            {
                containerRect = target.panel.visualTree.worldBound;
            }
            else if (containerMode is ContainerMode.Parent)
            {
                if (target.parent is not { } parent)
                {
                    return;
                }

                containerRect = parent.worldBound;
            }
            else if (containerMode is ContainerMode.Custom)
            {
                if (container is null)
                {
                    return;
                }

                containerRect = container.worldBound;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(containerMode), containerMode, null);
            }

            var targetRect = target.worldBound;

            if (containerRect.width <= 0 || containerRect.height <= 0)
            {
                return;
            }

            if (targetRect.width <= 0 || targetRect.height <= 0)
            {
                return;
            }

            var currentOffsetX = target.resolvedStyle.translate.x;
            var offsetX = GetOffset(targetRect.xMin, targetRect.xMax, containerRect.xMin, containerRect.xMax,
                currentOffsetX, xMinOverflow, xMaxOverflow);

            var currentOffsetY = target.resolvedStyle.translate.y;
            var offsetY = GetOffset(targetRect.yMin, targetRect.yMax, containerRect.yMin, containerRect.yMax,
                currentOffsetY, yMinOverflow, yMaxOverflow);

            target.style.translate = new Translate(offsetX, offsetY);
        }

        protected virtual float GetOffset(float currentMin, float currentMax, float containerMin, float containerMax,
            float currentOffset, bool minOverflow, bool maxOverflow)
        {
            if (minOverflow == false && maxOverflow == false)
            {
                if ((currentMax - currentMin).Abs() > (containerMax - containerMin).Abs())
                {
                    return currentOffset;
                }
            }

            if (minOverflow == false && currentMin < containerMin)
            {
                var delta = containerMin - currentMin;
                return currentOffset + delta;
            }

            if (maxOverflow == false && currentMax > containerMax)
            {
                var delta = containerMax - currentMax;
                return currentOffset + delta;
            }

            if (currentMin > containerMin && currentOffset > 0)
            {
                var delta = currentMin - containerMin;
                return (currentOffset - delta).ClampMin(0);
            }

            if (currentMax < containerMax && currentOffset < 0)
            {
                var delta = containerMax - currentMax;
                return (currentOffset + delta).ClampMax(0);
            }

            return currentOffset;
        }
    }
}