using UnityEngine.Localization;

namespace VMFramework.UI
{
    public interface ILocalizedUIPanel : IUIPanel
    {
        public void OnCurrentLanguageChanged(Locale currentLocale);
    }
}