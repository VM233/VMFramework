using System;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace VMFramework.Editor.Tests
{
    public sealed class EditorFormattingLocalizationSettings : LocalizationSettings
    {
        public int InitializationRequests { get; private set; }
        public int SelectedLocaleRequests { get; private set; }

        public override AsyncOperationHandle<LocalizationSettings> GetInitializationOperation()
        {
            InitializationRequests++;
            throw new InvalidOperationException("Editor formatting requested runtime Localization initialization.");
        }

        public override AsyncOperationHandle<Locale> GetSelectedLocaleAsync()
        {
            SelectedLocaleRequests++;
            throw new InvalidOperationException("Editor formatting requested the runtime selected Locale.");
        }
    }
}
