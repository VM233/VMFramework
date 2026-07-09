using UnityEngine.Localization;

namespace VMFramework.UI
{
    public interface ILocalizedPanelModifier
    {
        public void OnCurrentLanguageChanged(Locale currentLocale);
    }
}