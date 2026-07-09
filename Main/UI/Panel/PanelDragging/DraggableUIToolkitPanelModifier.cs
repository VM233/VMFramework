using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    [RequireComponent(typeof(IUIToolkitPanel))]
    public class DraggableUIToolkitPanelModifier : PanelModifier, IDraggablePanelModifier
    {
        public virtual bool EnableDragging => true;

        private Vector2 ReferenceResolution => this.UIDocument().panelSettings.referenceResolution;

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        public VisualElementPath draggableAreaPath = new();

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        public VisualElementPath draggingContainerPath = new();

        [TitleGroup(ComponentNames.CONFIG)]
        public bool draggableOverflowScreen = false;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        private VisualElement draggableArea;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        private VisualElement draggingContainer;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpen += OnOpen;
        }

        private void OnOpen(IUIPanel panel)
        {
            draggableArea = draggableAreaPath.MandatoryQuery(this.RootVisualElement(), nameof(draggableAreaPath));

            draggingContainer =
                draggingContainerPath.MandatoryQuery(this.RootVisualElement(), nameof(draggingContainerPath));

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

            Vector2 boundsSize = ReferenceResolution;

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