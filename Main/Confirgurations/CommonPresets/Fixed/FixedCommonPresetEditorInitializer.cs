#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Procedure;
using VMFramework.Procedure.Editor;

namespace VMFramework.Configuration
{
    internal sealed class FixedCommonPresetEditorInitializer : IEditorInitializer
    {
        public static readonly Dictionary<Type, Type> presetTypeLookup = new()
        {
            {
                typeof(int), typeof(IntCommonPreset)
            },
            {
                typeof(string), typeof(StringCommonPreset)
            }
        };

        public static readonly Dictionary<Type, Type> simplifiedPresetTypeLookup = new()
        {
            {
                typeof(string), typeof(SimpleStringCommonPreset)
            }
        };

        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.AfterInitComplete, OnAfterInitComplete, this));
        }

        private static UniTask OnAfterInitComplete(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            FixedCommonPresetInfo.presets.Clear();

            var attributes = new List<FixedCommonPresetRegisterAttribute>();

            foreach (var classType in AppDomain.CurrentDomain.GetAssemblies().GetAllClasses())
            {
                foreach (var attribute in classType.GetAttributes<FixedCommonPresetRegisterAttribute>(
                             includingInherit: false))
                {
                    attribute.ClassType = classType;
                    attributes.Add(attribute);
                }
            }

            foreach (var attribute in attributes)
            {
                if (attribute.Mode is not FixedCommonPresetRegisterAttribute.RegisterMode.CreatePreset)
                {
                    continue;
                }

                if (attribute.ValueType == null)
                {
                    continue;
                }

                if (attribute.Key.IsNullOrEmpty())
                {
                    continue;
                }

                Type presetType = null;

                if (attribute.SimplifiedValueType)
                {
                    if (simplifiedPresetTypeLookup.TryGetValue(attribute.ValueType, out var simplifiedPresetType))
                    {
                        presetType = simplifiedPresetType;
                    }
                }

                if (presetType is null)
                {
                    if (presetTypeLookup.TryGetValue(attribute.ValueType, out var normalPresetType))
                    {
                        presetType = normalPresetType;
                    }
                }

                if (presetType is null)
                {
                    continue;
                }

                var asset = ScriptableObject.CreateInstance(presetType);

                if (asset is CommonPreset commonPreset)
                {
                    commonPreset.ClearItems();
                    FixedCommonPresetInfo.presets.Add(attribute.Key, commonPreset);
                }
            }

            foreach (var attribute in attributes)
            {
                if (attribute.Mode is not FixedCommonPresetRegisterAttribute.RegisterMode.RegisterEntry)
                {
                    continue;
                }

                if (attribute.Key.IsNullOrEmpty())
                {
                    continue;
                }

                if (FixedCommonPresetInfo.presets.TryGetValue(attribute.Key, out var preset) == false)
                {
                    continue;
                }

                preset.AddItem(attribute.PresetKey, attribute.Value);
            }

            foreach (var attribute in attributes)
            {
                if (attribute.Mode is not FixedCommonPresetRegisterAttribute.RegisterMode.RegisterConstValues)
                {
                    continue;
                }

                if (attribute.ClassType is null)
                {
                    continue;
                }

                if (attribute.Key.IsNullOrEmpty())
                {
                    continue;
                }

                if (FixedCommonPresetInfo.presets.TryGetValue(attribute.Key, out var preset) == false)
                {
                    continue;
                }

                foreach (var info in attribute.ClassType
                             .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                             .Where(fieldInfo => fieldInfo.IsLiteral && !fieldInfo.IsInitOnly))
                {
                    var fieldName = info.Name;

                    if (attribute.ExcludedFields is { } excludedFields)
                    {
                        if (excludedFields.Contains(fieldName))
                        {
                            continue;
                        }
                    }
                    
                    var value = info.GetRawConstantValue();
                    
                    preset.AddItem(fieldName, value);
                }
            }

            return UniTask.CompletedTask;
        }
    }
}
#endif
