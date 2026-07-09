using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    [InfoBox("需要绑定" + nameof(Sprite))]
    public class UIToolkitIconBindModifier : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName(SingleModeLimit = BindObjectsNameAttribute.SingleModeLimitType.Single)]
        [IsNotNullOrEmpty]
        public string bindObjectsName;
        
        [TitleGroup(ComponentNames.CONFIG)]
        public bool autoHideContainer = true;

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        public VisualElementPath iconPath = new();

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        public VisualElementPath iconContainerPath = new();

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        public VisualElement Icon { get; protected set; }

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        public VisualElement IconContainer { get; protected set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpen += OnOpen;
            Panel.OnPostClose += OnClose;
            Panel.BindObjectsManager.OnBindObjectChanged += OnBindObjectChanged;
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            Icon = iconPath.MandatoryQuery(this.RootVisualElement(), nameof(iconPath));
            IconContainer = iconContainerPath.MandatoryQuery(this.RootVisualElement(), nameof(iconContainerPath));

            if (autoHideContainer)
            {
                IconContainer.DisplayNone();
            }
        }

        protected virtual void OnClose(IUIPanel panel)
        {
            Icon.style.backgroundImage = null;
        }
        
        protected virtual void OnBindObjectChanged(string bindName, object bindObject, object parentObject, bool added)
        {
            if (bindName != bindObjectsName)
            {
                return;
            }

            if (added)
            {
                var icon = (Sprite)bindObject;

                Icon.style.backgroundImage = new StyleBackground(icon);
                IconContainer.DisplayFlex();
            }
            else
            {
                Icon.style.backgroundImage = null;
                if (autoHideContainer)
                {
                    IconContainer.DisplayNone();
                }
            }
        }
    }
}