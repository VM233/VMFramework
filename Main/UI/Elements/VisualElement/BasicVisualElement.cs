using UnityEngine.UIElements;

namespace VMFramework.UI
{
    [UxmlElement]
    public partial class BasicVisualElement : VisualElement
    {
        public readonly VisualElementTooltip tooltipManager;

        public BasicVisualElement() : base()
        {
            tooltipManager = new VisualElementTooltip(this);
        }
    }
}
