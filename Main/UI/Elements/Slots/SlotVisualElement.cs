using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;

namespace VMFramework.UI
{
    [UxmlElement(visibility = LibraryVisibility.Visible)]
    public partial class SlotVisualElement : BaseBoolField, IToken
    {
        private const string DEEPER_BACKGROUND_UI_NAME = "deeper-background";
        private const string BACKGROUND_UI_NAME = "background";
        private const string FORE_BACKGROUND_UI_NAME = "fore-background";
        private const string BORDER_UI_NAME = "border";
        private const string BORDER_CONTENT_UI_NAME = "border-content";
        private const string ICON_UI_NAME = "icon";
        private const string DESCRIPTION_UI_NAME = "description";

        private const string CLASS_NAME = "slot";

        private const string DEEPER_BACKGROUND_CLASS_STYLE = "slot-deeper-background";
        private const string BACKGROUND_CLASS_STYLE = "slot-background";
        private const string FORE_BACKGROUND_CLASS_STYLE = "slot-fore-background";
        private const string BORDER_CLASS_STYLE = "slot-border";
        private const string BORDER_CONTENT_CLASS_STYLE = "slot-border-content";
        private const string ICON_CLASS_STYLE = "slot-icon";
        private const string COUNT_CLASS_STYLE = "slot-description";
        private const string INPUT_CLASS_STYLE = "slot-input";

        public readonly VisualElementTooltip tooltipManager;

        public VisualElement DeeperBackgroundElement { get; protected set; }
        public VisualElement BackgroundElement { get; protected set; }
        public VisualElement ForeBackgroundElement { get; protected set; }
        public VisualElement BorderElement { get; protected set; }
        public Label BorderContentLabel { get; protected set; }
        public VisualElement IconElement { get; protected set; }
        public Label DescriptionLabel { get; protected set; }

        [UxmlAttribute]
        public Sprite DefaultBackgroundImage
        {
            get => defaultBackgroundImage;
            set
            {
                defaultBackgroundImage = value;

                BackgroundElement.TryCoverBackgroundImage(value);
            }
        }

        public Sprite Icon
        {
            set => IconElement.style.backgroundImage = new StyleBackground(value);
        }

        public string Description
        {
            set => DescriptionLabel.text = value;
        }

        public override VisualElement contentContainer => this;

        private Sprite defaultBackgroundImage;

        public SlotVisualElement() : base(null)
        {
            ConfigureInheritedFieldVisual();

            tooltipManager = new VisualElementTooltip(this);

            AddToClassList(CLASS_NAME);

            DeeperBackgroundElement = new VisualElement
            {
                name = DEEPER_BACKGROUND_UI_NAME,
                pickingMode = PickingMode.Ignore
            };
            BackgroundElement = new VisualElement
            {
                name = BACKGROUND_UI_NAME
            };
            ForeBackgroundElement = new VisualElement
            {
                name = FORE_BACKGROUND_UI_NAME,
                pickingMode = PickingMode.Ignore
            };
            BorderElement = new VisualElement
            {
                name = BORDER_UI_NAME,
                pickingMode = PickingMode.Ignore
            };
            BorderContentLabel = new Label
            {
                name = BORDER_CONTENT_UI_NAME,
                pickingMode = PickingMode.Ignore
            };
            IconElement = new VisualElement
            {
                name = ICON_UI_NAME,
                pickingMode = PickingMode.Ignore
            };
            DescriptionLabel = new Label
            {
                name = DESCRIPTION_UI_NAME,
                text = 64.ToString(),
                pickingMode = PickingMode.Ignore
            };

            DeeperBackgroundElement.AddToClassList(DEEPER_BACKGROUND_CLASS_STYLE);
            BackgroundElement.AddToClassList(BACKGROUND_CLASS_STYLE);
            ForeBackgroundElement.AddToClassList(FORE_BACKGROUND_CLASS_STYLE);
            BorderElement.AddToClassList(BORDER_CLASS_STYLE);
            BorderContentLabel.AddToClassList(BORDER_CONTENT_CLASS_STYLE);
            IconElement.AddToClassList(ICON_CLASS_STYLE);
            DescriptionLabel.AddToClassList(COUNT_CLASS_STYLE);

            hierarchy.Add(DeeperBackgroundElement);
            hierarchy.Add(BackgroundElement);
            hierarchy.Add(ForeBackgroundElement);
            hierarchy.Add(IconElement);
            hierarchy.Add(BorderElement);
            BorderElement.Add(BorderContentLabel);
            hierarchy.Add(DescriptionLabel);
        }

        private void ConfigureInheritedFieldVisual()
        {
            var inputElement = this.Q<VisualElement>(className: BaseField<bool>.inputUssClassName);
            inputElement.Clear();
            inputElement.RemoveFromClassList(BaseField<bool>.inputUssClassName);
            inputElement.AddToClassList(INPUT_CLASS_STYLE);
            inputElement.pickingMode = PickingMode.Ignore;
            inputElement.style.position = Position.Absolute;
            inputElement.style.left = 0;
            inputElement.style.top = 0;
            inputElement.style.right = 0;
            inputElement.style.bottom = 0;

            RemoveFromClassList(BaseField<bool>.ussClassName);
            RemoveFromClassList(BaseField<bool>.noLabelVariantUssClassName);
        }
    }
}
