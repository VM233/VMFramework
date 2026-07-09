#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;
using VMFramework.Procedure.Editor;

namespace VMFramework.Configuration
{
    internal sealed class CommonPresetEditorInitializer : IEditorInitializer
    {
        public static readonly Dictionary<Type, Type> presetTypeLookup = new()
        {
            {
                typeof(int), typeof(IntCommonPreset)
            },
            {
                typeof(string), typeof(SimpleStringCommonPreset)
            }
        };

        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.AfterInitComplete, OnAfterInitComplete, this));
        }

        private static void OnAfterInitComplete(Action onDone)
        {
            var generalSetting = CoreSetting.CommonPresetGeneralSetting;

            if (generalSetting == null)
            {
                goto DONE;
            }

            bool anyChange = false;

            foreach (var classType in AppDomain.CurrentDomain.GetAssemblies().GetAllClasses())
            {
                foreach (var attribute in classType.GetAttributes<CommonPresetAutoRegisterAttribute>(
                             includingInherit: false))
                {
                    if (attribute.ValueType == null)
                    {
                        continue;
                    }

                    if (presetTypeLookup.TryGetValue(attribute.ValueType, out var presetType) == false)
                    {
                        continue;
                    }

                    if (generalSetting.AddPresetIfNotExists(attribute.Key, presetType, attribute.InitialKeys,
                            attribute.InitialValues))
                    {
                        anyChange = true;
                    }
                }
            }

            if (anyChange)
            {
                generalSetting.SetEditorDirty();
            }

            DONE:
            onDone();
        }
    }
}
#endif