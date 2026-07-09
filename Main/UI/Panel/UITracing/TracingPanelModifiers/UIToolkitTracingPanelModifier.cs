using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class UIToolkitTracingPanelModifier : TracingPanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        public Vector2 screenPositionOffset;

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        public VisualElementPath containerPath = new();

        [TitleGroup(ComponentNames.CONFIG)]
        public bool hideContainerUntilFirstUpdate = true;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool resetPositionOnClose = false;

        protected VisualElement TracingContainer { get; private set; }
        
        protected bool setVisibleRequired = false;

        protected override void OnOpen(IUIPanel panel)
        {
            TracingContainer = containerPath.MandatoryQuery(this.RootVisualElement(), nameof(containerPath));

            base.OnOpen(panel);

            if (hideContainerUntilFirstUpdate)
            {
                TracingContainer.visible = false;
                setVisibleRequired = true;
            }
        }

        protected override void OnPostClose(IUIPanel panel)
        {
            base.OnPostClose(panel);

            if (resetPositionOnClose)
            {
                TryUpdateScreenPosition(Vector2.zero);
            }

            TracingContainer = null;
            setVisibleRequired = false;
        }

        public virtual bool TryUpdatePosition(Vector2 position, RectangleFloat bounds)
        {
            var width = TracingContainer.resolvedStyle.width;
            var height = TracingContainer.resolvedStyle.height;

            if (width <= 0 || height <= 0 || width.IsNaN() || height.IsNaN())
            {
                return false;
            }
            
            var pivot = defaultPivot;
            if (enableScreenOverflow == false)
            {
                position = bounds.Clamp(position);
                
                if (position.x < bounds.min.x + defaultPivot.x * bounds.Size.x)
                {
                    pivot.x = ((position.x - bounds.min.x) / width).ClampMin(0);
                }
                else if (position.x > bounds.max.x - (1 - defaultPivot.x) * width)
                {
                    pivot.x = (1 - (bounds.max.x - position.x) / width).ClampMax(1);
                }

                if (position.y < bounds.min.y + defaultPivot.y * height)
                {
                    pivot.y = ((position.y - bounds.min.y) / height).ClampMin(0);
                }
                else if (position.y > bounds.max.y - (1 - defaultPivot.y) * height)
                {
                    pivot.y = (1 - (bounds.max.y - position.y) / height).ClampMax(1);
                }
            }

            SetPivot(pivot);
            
            TracingContainer.style.left = position.x;
            TracingContainer.style.bottom = position.y;

            return true;
        }

        public override bool TryUpdateScreenPosition(Vector2 screenPosition)
        {
            var rootVisualElement = this.RootVisualElement();
            var boundsSize = rootVisualElement.GetResolvedSize();

            if (boundsSize.TryGetPositionFromScreenPosition(screenPosition + screenPositionOffset, out var position) ==
                false)
            {
                return false;
            }

            if (TryUpdatePosition(position, new RectangleFloat(Vector2.zero, boundsSize)) == false)
            {
                return false;
            }

            if (setVisibleRequired)
            {
                TracingContainer.visible = true;
                setVisibleRequired = false;
            }

            return true;
        }

        public override void SetPivot(Vector2 pivot)
        {
            TracingContainer.style.translate = new StyleTranslate(new Translate(
                new Length(-100 * pivot.x, LengthUnit.Percent), new Length(100 * pivot.y, LengthUnit.Percent)));
        }
    }
}