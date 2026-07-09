using UnityEngine.UIElements;

namespace VMFramework.UI
{
    [UxmlElement]
    public partial class BasicButton : Button
    {
        private const string CLASS_NAME = "basic-button";

        public readonly VisualElementTooltip tooltipManager;

        public BasicButton()
        {
            tooltipManager = new VisualElementTooltip(this);

            AddToClassList(CLASS_NAME);
        }
    }
}