using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class FPSDebugModifier : PanelModifier, IRefreshable
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [VisualElementPathSettings(typeof(Label))]
        [IsNotNullOrEmpty]
        public VisualElementPath labelPath = new();

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        public VisualElementPath containerPath = new();

        protected Label label;
        protected VisualElement container;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpen += OnOpen;
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            label = labelPath.MandatoryQuery<Label>(this.RootVisualElement(), nameof(labelPath));
            container = containerPath.Query(this.RootVisualElement()) ?? label;

            Refresh();
        }

        public virtual void Refresh()
        {
            if (Panel.IsOpened == false)
            {
                return;
            }

            if (Time.unscaledDeltaTime <= 0)
            {
                container.DisplayNone();
                return;
            }

            container.DisplayFlex();
            label.text = (1 / Time.unscaledDeltaTime).ToString(0);
        }
    }
}