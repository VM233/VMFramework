using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class ToggleDisplayModifier : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [VisualElementPathSettings(typeof(Toggle))]
        [IsNotNullOrEmpty]
        public VisualElementPath togglePath = new();
        
        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        public List<VisualElementPath> targetElementsPaths = new();

        protected Toggle toggle;
        protected readonly List<VisualElement> targetElements = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpen += OnOpen;
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            toggle = togglePath.MandatoryQuery<Toggle>(this.RootVisualElement(), nameof(togglePath));
            targetElements.Clear();
            targetElements.AddRange(
                targetElementsPaths.MandatoryQuery(this.RootVisualElement(), nameof(targetElementsPaths)));

            toggle.RegisterValueChangedCallback(OnToggleChanged);

            Refresh();
        }

        protected virtual void OnToggleChanged(ChangeEvent<bool> evt)
        {
            Refresh();
        }

        protected virtual void Refresh()
        {
            var value = toggle.value;
            targetElements.Display(value);
        }
    }
}