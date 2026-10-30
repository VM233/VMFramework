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
        public bool IconAlwaysDisplay
        {
            get => iconAlwaysDisplay;
            set
            {
                iconAlwaysDisplay = value;
                RefreshIconVisibility();
            }
        }

        private bool iconAlwaysDisplay;

        public VisualElement Icon { get; }
        public Label Label { get; }

        [UxmlAttribute]
        public string Text
        {
            get => Label.text;
            set => SetContent(value);
        }

        [UxmlAttribute]
        public Sprite IconSprite
        {
            get => Icon.style.backgroundImage.value.sprite;
            set => SetIcon(value);
        }

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
            SetIcon(null);
        }

        public void SetIcon(Sprite icon)
        {
            Icon.style.backgroundImage = new StyleBackground(icon);
            RefreshIconVisibility();
        }

        private void RefreshIconVisibility() => Icon.style.display =
            iconAlwaysDisplay || IconSprite != null ? DisplayStyle.Flex : DisplayStyle.None;

        public void SetContent(string content)
        {
            Label.text = content;
        }
    }
}
