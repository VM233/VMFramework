#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace VMFramework.OdinExtensions
{
    [DrawerPriority(0, 0.0, 5000)]
    internal sealed class EnabledIfHasFlagAttributeDrawer : OdinAttributeDrawer<EnabledIfHasFlagAttribute>
    {
        private ValueResolver<object> valueResolver;

        protected override void Initialize()
        {
            valueResolver = ValueResolver.Get<object>(Property, Attribute.Condition);
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (valueResolver.HasError)
            {
                SirenixEditorGUI.ErrorMessageBox(valueResolver.ErrorMessage);
                CallNextDrawer(label);
                return;
            }

            var value = valueResolver.GetValue();

            var enumValue = (Enum)value;

            var enabled = false;
            foreach (var flag in Attribute.Flags)
            {
                if (enumValue.HasFlag(flag))
                {
                    enabled = true;
                    break;
                }
            }

            Property.State.Enabled = enabled;

            CallNextDrawer(label);
        }
    }
}
#endif