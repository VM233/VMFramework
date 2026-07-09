using UnityEngine;

namespace VMFramework.UI
{
    public interface IDraggablePanelModifier : IPanelModifier
    {
        public bool EnableDragging { get; }

        void OnDragStart();

        void OnDrag(Vector2 mouseDelta, Vector2 mousePosition);

        void OnDragStop();
    }
}
