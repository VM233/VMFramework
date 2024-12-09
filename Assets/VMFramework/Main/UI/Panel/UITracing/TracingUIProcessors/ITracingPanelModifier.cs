using UnityEngine;

namespace VMFramework.UI
{
    public interface ITracingPanelModifier : IPanelModifier
    {
        public bool TryUpdatePosition(Vector2 screenPosition);

        public void SetPivot(Vector2 pivot);
    }
}
