using System;
using UnityEngine.UIElements;

namespace VMFramework.UI
{
    [UxmlElement]
    public partial class SlotVisualElement : BasicVisualElement
    {
        public enum ContentContainerType
        {
            None,
            DeeperBackground,
            Background,
            Border,
            BorderContent,
            Icon,
            Description
        }

        private const string DEEPER_BACKGROUND_UI_NAME = "deeper-background";
        private const string BACKGROUND_UI_NAME = "background";
        private const string BORDER_UI_NAME = "border";
        private const string BORDER_CONTENT_UI_NAME = "border-content";
        private const string ICON_UI_NAME = "icon";
        private const string COUNT_UI_NAME = "description";

        private const string DEEPER_BACKGROUND_CLASS_STYLE = "slot-deeper-background";
        private const string BACKGROUND_CLASS_STYLE = "slot-background";
        private const string BORDER_CLASS_STYLE = "slot-border";
        private const string BORDER_CONTENT_CLASS_STYLE = "slot-border-content";
        private const string ICON_CLASS_STYLE = "slot-icon";
        private const string COUNT_CLASS_STYLE = "slot-description";

        public VisualElement DeeperBackground { get; }
        public VisualElement Background { get; }
        public VisualElement Border { get; }
        public Label BorderContent { get; }
        public VisualElement Icon { get; }
        public Label Description { get; }

        private bool displayNoneIfNull;

        [UxmlAttribute]
        public bool DisplayNoneIfNull
        {
            get => displayNoneIfNull;
            set
            {
                displayNoneIfNull = value;
                if (currentSlotProvider == null)
                {
                    style.display = displayNoneIfNull
                        ? DisplayStyle.None
                        : DisplayStyle.Flex;
                }
            }
        }

        [UxmlAttribute]
        public bool IgnoreBorderChange { get; set; }
        
        [UxmlAttribute]
        public bool IgnoreBackgroundChange { get; set; }

        [UxmlAttribute]
        public ContentContainerType contentContainerType { get; set; } =
            ContentContainerType.None;

        public override VisualElement contentContainer =>
            GetContentContainer(contentContainerType);

        public event Action<SlotVisualElement> OnLeftMouseClick;
        public event Action<SlotVisualElement> OnRightMouseClick;
        public event Action<SlotVisualElement> OnMiddleMouseClick;

        private ISlotProvider currentSlotProvider;

        private readonly StyleBackground defaultBackgroundImage, defaultBorderImage;

        public SlotVisualElement() : base()
        {
            DeeperBackground = new VisualElement
            {
                name = DEEPER_BACKGROUND_UI_NAME
            };
            Background = new VisualElement
            {
                name = BACKGROUND_UI_NAME
            };
            Border = new VisualElement
            {
                name = BORDER_UI_NAME
            };
            BorderContent = new Label
            {
                name = BORDER_CONTENT_UI_NAME
            };
            Icon = new VisualElement
            {
                name = ICON_UI_NAME
            };
            Description = new Label
            {
                name = COUNT_UI_NAME,
                text = 64.ToString()
            };
            
            DeeperBackground.AddToClassList(DEEPER_BACKGROUND_CLASS_STYLE);
            Background.AddToClassList(BACKGROUND_CLASS_STYLE);
            Border.AddToClassList(BORDER_CLASS_STYLE);
            BorderContent.AddToClassList(BORDER_CONTENT_CLASS_STYLE);
            Icon.AddToClassList(ICON_CLASS_STYLE);
            Description.AddToClassList(COUNT_CLASS_STYLE);

            Add(DeeperBackground);
            Add(Background);
            Add(Icon);
            Add(Border);
            Border.Add(BorderContent);
            Add(Description);

            RegisterCallback<MouseUpEvent>(OnMouseUp);

            OnMouseEnter += () =>
            {
                if (currentSlotProvider != null)
                {
                    currentSlotProvider.HandleMouseEnterEvent(source);
                }
            };

            OnMouseLeave += () =>
            {
                if (currentSlotProvider != null)
                {
                    currentSlotProvider.HandleMouseLeaveEvent(source);
                }
            };

            OnRightMouseClick += slot =>
            {
                if (currentSlotProvider != null)
                {
                    currentSlotProvider.HandleRightMouseClickEvent(source);
                }
            };

            defaultBackgroundImage = Background.style.backgroundImage;
            defaultBorderImage = Border.style.backgroundImage;
        }

        private void OnMouseUp(MouseUpEvent e)
        {
            if (e.button == 0)
            {
                OnLeftMouseClick?.Invoke(this);
            }
            else if (e.button == 1)
            {
                OnRightMouseClick?.Invoke(this);
            }
            else if (e.button == 2)
            {
                OnMiddleMouseClick?.Invoke(this);
            }
        }

        public void SetSlotProvider(ISlotProvider slotProvider)
        {
            StyleBackground backgroundImage, borderImage;

            if (slotProvider == null)
            {
                backgroundImage = defaultBackgroundImage;
                borderImage = defaultBorderImage;

                Icon.style.backgroundImage = null;
                Description.text = "";

                if (IgnoreBackgroundChange == false)
                {
                    Background.style.backgroundImage = backgroundImage;
                }

                if (IgnoreBorderChange == false)
                {
                    Border.style.backgroundImage = borderImage;
                }

                if (DisplayNoneIfNull)
                {
                    style.display = DisplayStyle.None;
                }
                
                SetTooltip(default(ITooltipProvider));
            }
            else
            {
                style.display = DisplayStyle.Flex;

                if (slotProvider.enableBackgroundImageOverride)
                {
                    backgroundImage = slotProvider.GetBackgroundImage();

                    if (IgnoreBackgroundChange == false)
                    {
                        Background.style.backgroundImage = backgroundImage;
                    }
                }

                if (slotProvider.enableBorderImageOverride)
                {
                    borderImage = slotProvider.GetBorderImage();

                    if (IgnoreBorderChange == false)
                    {
                        Border.style.backgroundImage = borderImage;
                    }
                }

                Icon.style.backgroundImage = slotProvider.GetIconImage();
                Description.text = slotProvider.GetDescriptionText();
                
                SetTooltip(slotProvider as ITooltipProvider);
            }

            currentSlotProvider = slotProvider;
        }

        private VisualElement GetContentContainer(ContentContainerType contentContainerType)
        {
            return contentContainerType switch
            {
                ContentContainerType.DeeperBackground => DeeperBackground,
                ContentContainerType.Background => Background,
                ContentContainerType.Border => Border,
                ContentContainerType.BorderContent => BorderContent,
                ContentContainerType.Icon => Icon,
                ContentContainerType.Description => Description,
                _ => this
            };
        }
    }
}
