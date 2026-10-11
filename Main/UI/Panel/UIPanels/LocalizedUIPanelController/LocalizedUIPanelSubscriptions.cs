using System;
using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace VMFramework.UI
{
    internal sealed class LocalizedUIPanelSubscriptions
    {
        private readonly IUIPanel panel;
        private readonly LocalizationSettings localization;
        private IUIPanel.OpenHandler open;
        private IUIPanel.CloseHandler close;
        private readonly List<Action<Locale>> localeEndpoints = new();

        public LocalizedUIPanelSubscriptions(IUIPanel panel, LocalizationSettings localization)
        { this.panel = panel; this.localization = localization; }

        public void Acquire(IUIPanel.OpenHandler onOpen, IUIPanel.CloseHandler onClose)
        {
            panel.OnOpen += onOpen; open = onOpen;
            panel.OnPostClose += onClose; close = onClose;
        }

        public void Subscribe(ILocalizedPanelModifier modifier)
        {
            modifier.OnCurrentLanguageChanged(localization.GetSelectedLocale());
            Action<Locale> endpoint = modifier.OnCurrentLanguageChanged;
            localization.OnSelectedLocaleChanged += endpoint;
            try { localeEndpoints.Add(endpoint); }
            catch (Exception primary)
            {
                var failures = new UIPanelReleaseFailures(nameof(LocalizedUIPanelSubscriptions), primary);
                failures.Attempt("unadopted-locale.unsubscribe", () => localization.OnSelectedLocaleChanged -= endpoint);
                failures.ThrowIfFailed(); throw;
            }
        }

        public void ReleaseLocale()
        {
            var actual = localeEndpoints.ToArray(); localeEndpoints.Clear();
            var failures = new UIPanelReleaseFailures(nameof(LocalizedUIPanelSubscriptions));
            foreach (var endpoint in actual)
                failures.Attempt("locale.unsubscribe", () => localization.OnSelectedLocaleChanged -= endpoint);
            failures.ThrowIfFailed();
        }

        public void Release()
        {
            var opened = open; open = null;
            var closed = close; close = null;
            var failures = new UIPanelReleaseFailures(nameof(LocalizedUIPanelSubscriptions));
            if (opened != null) failures.Attempt("panel-open.unsubscribe", () => panel.OnOpen -= opened);
            if (closed != null) failures.Attempt("panel-close.unsubscribe", () => panel.OnPostClose -= closed);
            failures.Attempt("locale.release", ReleaseLocale);
            failures.ThrowIfFailed();
        }
    }
}
