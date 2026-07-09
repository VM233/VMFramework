using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Procedure;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

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
#if ENABLE_INPUT_SYSTEM
            lastMousePosition = Mouse.current.position.ReadValue();
#else
            lastMousePosition = Input.mousePosition;
#endif
        }

        private void Update()
        {
#if ENABLE_INPUT_SYSTEM
            Vector2 mousePosition = Mouse.current.position.ReadValue();
#else
            Vector2 mousePosition = Input.mousePosition;
#endif
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
            if (draggablePanel.EnableDragging)
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
