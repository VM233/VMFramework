#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;
using VMFramework.Procedure;
using VMFramework.Procedure.Editor;

namespace VMFramework.Localization
{
    internal sealed class LocalizationEditorInitializer : IEditorInitializer
    {
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.BeforeInitStart, OnBeforeInitStart, this));
        }

        private static void OnBeforeInitStart(Action onDone)
        {
            LocalizationSettings.StringDatabase.GetTable(LocalizationTableNames.EDITOR,
                LocalizationSettings.ProjectLocale);

            onDone();
        }
    }
}

#endif