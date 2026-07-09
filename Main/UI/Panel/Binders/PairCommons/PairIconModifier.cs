using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class PairIconModifier : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName]
        [IsNotNullOrEmpty]
        public string bindObjectsName;
        
        [TitleGroup(ComponentNames.CONFIG)]
        [VisualElementPathSettings(IsFromLocalProvider = true)]
        [IsNotNullOrEmpty]
        public VisualElementPath iconPath = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            this.UIToolkitPanel().BindVisualElementsManager.OnBindVisualElementChanged += OnBindVisualElementChanged;
        }

        protected virtual void OnBindVisualElementChanged(string bindName, object bindObject,
            VisualElement visualElement, bool added)
        {
            if (bindName != bindObjectsName)
            {
                return;
            }
            
            if (added)
            {
                var iconElement = iconPath.MandatoryQuery(visualElement, nameof(iconPath));

                var icon = GetIcon(bindObject);

                iconElement.style.backgroundImage = new StyleBackground(icon);
            }
            else
            {
                var iconElement = iconPath.MandatoryQuery(visualElement, nameof(iconPath));
                
                iconElement.style.backgroundImage = StyleKeyword.None;
            }
        }

        protected virtual Sprite GetIcon(object key)
        {
            Sprite icon = null;

            if (key is IIconOwner iconOwner)
            {
                icon = iconOwner.Icon;
            }
            
            return icon;
        }
    }
}