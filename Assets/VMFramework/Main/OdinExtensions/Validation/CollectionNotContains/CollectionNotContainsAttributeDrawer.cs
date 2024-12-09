#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities.Editor;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.OdinExtensions
{
    internal sealed class CollectionNotContainsAttributeDrawer : OdinAttributeDrawer<CollectionNotContainsAttribute>
    {
        private ValueResolver contentResolver;

        protected override void Initialize()
        {
            base.Initialize();
            
            if (Attribute.IsContentValid)
            {
                return;
            }

            var valueType = Property.ValueEntry.TypeOfValue;
            Type itemType = valueType.GetGenericArguments()[0];
            contentResolver = ValueResolver.Get(itemType, Property, Attribute.ContentGetter);
        }

        public override bool CanDrawTypeFilter(Type type)
        {
            if (type.IsGenericType == false)
            {
                return false;
            }

            if (type.IsDerivedFrom(typeof(ICollection<>), false, true) == false)
            {
                return false;
            }
            
            return true;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var value = Property.ValueEntry.WeakSmartValue;

            if (value == null)
            {
                CallNextDrawer(label);
                return;
            }

            if (Attribute.IsContentValid == false && contentResolver.HasError)
            {
                SirenixEditorGUI.ErrorMessageBox(contentResolver.ErrorMessage);
                CallNextDrawer(label);
                return;
            }

            if (value is not IEnumerable enumerable)
            {
                SirenixEditorGUI.ErrorMessageBox($"{label} is not a enumerable");
                CallNextDrawer(label);
                return;
            }

            var collection = enumerable.Cast<object>();

            object itemToCheck = Attribute.IsContentValid ? Attribute.Content : contentResolver.GetWeakValue();
            if (collection.Contains(itemToCheck))
            {
                SirenixEditorGUI.ErrorMessageBox($"{label} should not contains {itemToCheck}");
            }

            CallNextDrawer(label);
        }
    }
}
#endif