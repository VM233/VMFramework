using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace VMFramework.UI
{
    [UxmlElement]
    public sealed partial class IconLabelVisualElement : VisualElement
    {
        [ShowInInspector]
        [UxmlAttribute]
        public bool IconAlwaysDisplay { get; set; } = false;

        public VisualElement Icon { get; }
        public Label Label { get; }

        public IconLabelVisualElement() : base()
        {
            Icon = new VisualElement
            {
                name = "Icon"
            };
            Icon.AddToClassList("icon-label-icon");
            Add(Icon);

            Label = new Label
            {
                name = "Content",
                text = "Content"
            };
            Label.AddToClassList("icon-label-content");
            Add(Label);
            
            AddToClassList("icon-label");
        }

        public void SetIcon(Sprite icon)
        {
            if (IconAlwaysDisplay == false)
            {
                if (icon == null)
                {
                    this.Icon.style.display = DisplayStyle.None;
                }
                else
                {
                    this.Icon.style.display = DisplayStyle.Flex;
                }
            }

            this.Icon.style.backgroundImage = new StyleBackground(icon);
        }

        public void SetContent(string content)
        {
            Label.text = content;
        }
    }
}
