using UnityEngine.Localization;

namespace VMFramework.UI
{
    /// <summary>
    /// Refreshes localized state for a modifier while its owning UI panel is open.
    /// </summary>
    public interface ILocalizedPanelModifier
    {
        /// <summary>
        /// Refreshes the modifier for <paramref name="currentLocale"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="LocalizedUIPanelManager"/> invokes this once during panel opening before the
        /// modifier's <see cref="IUIPanel.OnOpen"/> handler, then invokes it for locale changes until
        /// the panel closes. Implementations that bind UI references in <see cref="IUIPanel.OnOpen"/>
        /// therefore receive the opening callback before those references exist. Their open handler
        /// must perform the initial localized refresh; subsequent locale callbacks should update only
        /// references bound for the current open lifetime.
        /// </remarks>
        public void OnCurrentLanguageChanged(Locale currentLocale);
    }
}
