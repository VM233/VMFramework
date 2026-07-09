#if UNITY_EDITOR
using UnityEngine;

namespace VMFramework.UI
{
    public partial class UGUIPanel
    {
        protected virtual void Reset()
        {
            var rectTransform = gameObject.GetComponent<RectTransform>();

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }
}
#endif