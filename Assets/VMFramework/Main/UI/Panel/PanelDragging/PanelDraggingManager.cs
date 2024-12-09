using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public sealed class PanelDraggingManager : ManagerBehaviour<PanelDraggingManager>
    {
        [ShowInInspector]
        private static readonly HashSet<IDraggablePanelModifier> draggingPanels = new();

        [ShowInInspector]
        private static readonly List<IDraggablePanelModifier> removalDraggingPanels = new();

        private static Vector2 lastMousePosition;

        private void Start()
        {
            lastMousePosition = Input.mousePosition;
        }

        private void Update()
        {
            Vector2 mousePosition = Input.mousePosition;

            foreach (var draggablePanel in draggingPanels)
            {
                draggablePanel.OnDrag(mousePosition - lastMousePosition,
                    mousePosition);
            }

            foreach (var draggablePanel in removalDraggingPanels)
            {
                if (draggingPanels.Remove(draggablePanel))
                {
                    draggablePanel.OnDragStop();
                }
            }

            removalDraggingPanels.Clear();

            lastMousePosition = mousePosition;
        }

        public static void StartDrag(IDraggablePanelModifier draggablePanel)
        {
            if (draggablePanel.enableDragging)
            {
                draggingPanels.Add(draggablePanel);
                draggablePanel.OnDragStart();
            }
        }

        public static void StopDrag(IDraggablePanelModifier draggablePanel)
        {
            removalDraggingPanels.Add(draggablePanel);
        }
    }
}
