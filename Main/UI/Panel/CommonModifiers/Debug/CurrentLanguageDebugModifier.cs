using Sirenix.OdinInspector;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class CurrentLanguageDebugModifier : PanelModifier, IRefreshable
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

            if (LocalizationSettings.SelectedLocale == null)
            {
                container.DisplayNone();
                return;
            }

            container.DisplayFlex();
            label.text = LocalizationSettings.SelectedLocale.Identifier.CultureInfo.Name;
        }
    }
}