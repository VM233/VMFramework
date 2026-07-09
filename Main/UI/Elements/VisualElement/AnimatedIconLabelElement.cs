using UnityEngine.UIElements;

namespace VMFramework.UI
{
    [UxmlElement]
    public partial class AnimatedIconLabelElement : VisualElement
    {
        public const string CLASS_NAME = "icon-label";

        public const string ICON_CLASS_NAME = "icon-label-icon";
        public const string CONTENT_CLASS_NAME = "icon-label-content";

        public AnimatedSpritesElement Icon { get; }
        public Label Label { get; }

        public AnimatedIconLabelElement() : base()
        {
            AddToClassList(CLASS_NAME);

            Icon = new AnimatedSpritesElement
            {
                name = "Icon"
            };
            Icon.AddToClassList(ICON_CLASS_NAME);
            hierarchy.Add(Icon);

            Label = new Label
            {
                name = "Content",
                text = "Content"
            };
            Label.AddToClassList(CONTENT_CLASS_NAME);
            hierarchy.Add(Label);
        }

        public void SetContent(string content)
        {
            Label.text = content;
        }
    }
}