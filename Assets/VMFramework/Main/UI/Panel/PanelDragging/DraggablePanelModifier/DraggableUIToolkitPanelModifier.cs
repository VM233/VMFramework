using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;

namespace VMFramework.UI
{
    public class DraggableUIToolkitPanelModifier : UIToolkitPanelModifier, IDraggablePanelModifier
    {
        protected DraggableUIToolkitPanelModifierConfig DraggableUIToolkitPanelModifierConfig =>
            (DraggableUIToolkitPanelModifierConfig)GamePrefab;

        public virtual bool enableDragging => true;

        public Vector2 referenceResolution => UIToolkitPanel.UIDocument.panelSettings.referenceResolution;

        public bool draggableOverflowScreen => DraggableUIToolkitPanelModifierConfig.draggableOverflowScreen;

        [ShowInInspector]
        private VisualElement draggableArea;

        [ShowInInspector]
        private VisualElement draggingContainer;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpenEvent += OnOpen;
        }

        private void OnOpen(IUIPanel panel)
        {
            draggableArea = RootVisualElement.QueryStrictly(DraggableUIToolkitPanelModifierConfig.draggableAreaName,
                nameof(DraggableUIToolkitPanelModifierConfig.draggableAreaName));

            draggingContainer = RootVisualElement.QueryStrictly(
                DraggableUIToolkitPanelModifierConfig.draggingContainerName,
                nameof(DraggableUIToolkitPanelModifierConfig.draggingContainerName));
            
            draggableArea.RegisterCallback<MouseDownEvent>(_ => PanelDraggingManager.StartDrag(this));
            draggableArea.RegisterCallback<MouseUpEvent>(_ => PanelDraggingManager.StopDrag(this));
        }

        protected virtual void OnDragStart()
        {
            
        }
        
        protected virtual void OnDragStop()
        {
            
        }

        protected virtual void OnDrag(Vector2 mouseDelta, Vector2 mousePosition)
        {
            Vector2 screenSize = new(Screen.width, Screen.height);

            if (mousePosition.IsOverflow(Vector2.zero, screenSize))
            {
                PanelDraggingManager.StopDrag(this);
            }

            Vector2 boundsSize = referenceResolution;

            Vector2 delta = mouseDelta.Divide(screenSize).Multiply(boundsSize);

            var left = draggingContainer.resolvedStyle.left;
            var bottom = -draggingContainer.resolvedStyle.bottom;

            var width = draggingContainer.resolvedStyle.width;
            var height = draggingContainer.resolvedStyle.height;

            var resultLeft = left + delta.x;
            var resultBottom = bottom + delta.y;

            if (draggableOverflowScreen == false)
            {
                resultLeft = resultLeft.Clamp(0, boundsSize.x - width);
                resultBottom = resultBottom.Clamp(0, boundsSize.y - height);
            }

            draggingContainer.SetPosition(new Vector2(resultLeft, resultBottom));
        }

        void IDraggablePanelModifier.OnDragStart()
        {
            OnDragStart();
        }

        void IDraggablePanelModifier.OnDragStop()
        {
            OnDragStop();
        }

        void IDraggablePanelModifier.OnDrag(Vector2 mouseDelta, Vector2 mousePosition)
        {
            OnDrag(mouseDelta, mousePosition);
        }
    }
}