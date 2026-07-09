using UnityEngine;
using VMFramework.Core;

namespace VMFramework.UI
{
    public class UGUITracingPanelModifier : TracingPanelModifier
    {
        protected IUGUIPanel UGUIPanel => (IUGUIPanel)Panel;
        
        protected Vector2 ReferenceResolution => UGUIPanel.CanvasScaler.referenceResolution;
        
        protected RectTransform TracingContainer => UGUIPanel.UIMainRectTransform;

        protected override void OnOpen(IUIPanel panel)
        {
            UGUIPanel.UIMainRectTransform.anchorMin = Vector2.zero;
            UGUIPanel.UIMainRectTransform.anchorMax = Vector2.zero;
            
            base.OnOpen(panel);
        }

        public override bool TryUpdateScreenPosition(Vector2 screenPosition)
        {
            Vector2 screenSize = new(Screen.width, Screen.height);
            var boundsSize = ReferenceResolution;

            var position = screenPosition.Divide(screenSize).Multiply(boundsSize);

            var width = TracingContainer.GetWidth();
            var height = TracingContainer.GetHeight();

            var pivot = defaultPivot;
            if (enableScreenOverflow == false)
            {
                position = position.Clamp(boundsSize);

                if (position.x < defaultPivot.x * width)
                {
                    pivot.x = (position.x / width).ClampMin(0);
                }
                else if (position.x > boundsSize.x - (1 - defaultPivot.x) * width)
                {
                    pivot.x = (1 - (boundsSize.x - position.x) / width).ClampMax(1);
                }

                if (position.y < defaultPivot.y * height)
                {
                    pivot.y = (position.y / height).ClampMin(0);
                }
                else if (position.y > boundsSize.y - (1 - defaultPivot.y) * height)
                {
                    pivot.y = (1 - (boundsSize.y - position.y) / height).ClampMax(1);
                }
            }

            SetPivot(pivot);
            TracingContainer.anchoredPosition = position;

            return true;
        }

        public override void SetPivot(Vector2 pivot)
        {
            TracingContainer.pivot = pivot;
        }
    }
}