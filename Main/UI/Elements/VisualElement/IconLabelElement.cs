using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

namespace VMFramework.UI
{
    [UxmlElement]
    public sealed partial class IconLabelElement : VisualElement
    {
        public const string CLASS_NAME = "icon-label";

        public const string ICON_CLASS_NAME = "icon-label__icon";
        public const string CONTENT_CLASS_NAME = "icon-label__content";

        [ShowInInspector]
        [UxmlAttribute]
        public bool IconAlwaysDisplay { get; set; } = false;

        public VisualElement Icon { get; }
        public Label Label { get; }

        public IconLabelElement() : base()
        {
            AddToClassList(CLASS_NAME);

            Icon = new VisualElement
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

        public void SetIcon(Sprite icon)
        {
            if (IconAlwaysDisplay == false)
            {
                if (icon == null)
                {
                    Icon.style.display = DisplayStyle.None;
                }
                else
                {
                    Icon.style.display = DisplayStyle.Flex;
                }
            }

            Icon.style.backgroundImage = new StyleBackground(icon);
        }

        public void SetContent(string content)
        {
            Label.text = content;
        }
    }
}