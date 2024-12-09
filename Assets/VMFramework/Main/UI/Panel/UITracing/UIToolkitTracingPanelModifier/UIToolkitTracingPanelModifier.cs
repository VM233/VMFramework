using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;

namespace VMFramework.UI
{
    public class UIToolkitTracingPanelModifier : TracingPanelModifier
    {
        protected UIToolkitTracingPanelModifierConfig UIToolkitTracingProcessorConfig =>
            (UIToolkitTracingPanelModifierConfig)GamePrefab;

        protected IUIToolkitPanel UIToolkitPanel => (IUIToolkitPanel)Panel;

        protected Vector2 ReferenceResolution => UIToolkitPanel.UIDocument.panelSettings.referenceResolution;

        protected bool UseRightPosition => UIToolkitTracingProcessorConfig.useRightPosition;

        protected bool UseTopPosition => UIToolkitTracingProcessorConfig.useTopPosition;

        protected VisualElement TracingContainer { get; private set; }

        protected override void OnOpen(IUIPanel panel)
        {
            TracingContainer = UIToolkitPanel.RootVisualElement.QueryStrictly(
                UIToolkitTracingProcessorConfig.containerVisualElementName,
                nameof(UIToolkitTracingProcessorConfig.containerVisualElementName));
            
            base.OnOpen(panel);
        }

        protected override void OnPostClose(IUIPanel panel)
        {
            base.OnPostClose(panel);
            
            TracingContainer = null;
        }
        
        public override bool TryUpdatePosition(Vector2 screenPosition)
        {
            Vector2 screenSize = new(Screen.width, Screen.height);
            var boundsSize = ReferenceResolution;

            var position = screenPosition.Divide(screenSize).Multiply(boundsSize);

            var width = TracingContainer.resolvedStyle.width;
            var height = TracingContainer.resolvedStyle.height;

            if (width <= 0 || height <= 0 || width.IsNaN() || height.IsNaN())
            {
                return false;
            }

            if (EnableOverflow == false)
            {
                position = position.Clamp(boundsSize);

                if (AutoPivotCorrection)
                {
                    var pivot = DefaultPivot;

                    if (position.x < DefaultPivot.x * width)
                    {
                        pivot.x = (position.x / width).ClampMin(0);
                    }
                    else if (position.x > boundsSize.x - (1 - DefaultPivot.x) * width)
                    {
                        pivot.x = (1 - (boundsSize.x - position.x) / width).ClampMax(1);
                    }

                    if (position.y < DefaultPivot.y * height)
                    {
                        pivot.y = (position.y / height).ClampMin(0);
                    }
                    else if (position.y > boundsSize.y - (1 - DefaultPivot.y) * height)
                    {
                        pivot.y = (1 - (boundsSize.y - position.y) / height).ClampMax(1);
                    }

                    SetPivot(pivot);
                }
            }

            TracingContainer.SetPosition(position, UseRightPosition, UseTopPosition, boundsSize);

            return true;
        }

        public override void SetPivot(Vector2 pivot)
        {
            TracingContainer.style.translate = new StyleTranslate(new Translate(
                new Length(-100 * pivot.x, LengthUnit.Percent), new Length(100 * pivot.y, LengthUnit.Percent)));
        }
    }
}