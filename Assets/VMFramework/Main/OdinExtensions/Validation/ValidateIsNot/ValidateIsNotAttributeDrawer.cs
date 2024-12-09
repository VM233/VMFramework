#if UNITY_EDITOR
using System.Linq;
using VMFramework.Core;
using UnityEngine;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;
using VMFramework.Core.Linq;

namespace VMFramework.OdinExtensions
{
    [DrawerPriority(DrawerPriorityLevel.WrapperPriority)]
    public sealed class ValidateIsNotAttributeDrawer : SingleValidationAttributeDrawer<ValidateIsNotAttribute>
    {
        private ValueResolver[] contentResolvers;

        private object[] contents;

        protected override void Initialize()
        {
            base.Initialize();

            if (Attribute.ContentGetters.IsNullOrEmpty() == false)
            {
                contentResolvers = new ValueResolver[Attribute.ContentGetters.Length];

                for (int i = 0; i < Attribute.ContentGetters.Length; i++)
                {
                    contentResolvers[i] = ValueResolver.Get(Property.ValueEntry.TypeOfValue, Property,
                        Attribute.ContentGetters[i]);
                }
                
                contents = new object[Attribute.ContentGetters.Length];
            }
            else
            {
                contents = Attribute.Contents;
            }
        }

        protected override bool Validate(object value)
        {
            foreach (var content in contents)
            {
                if (content == null)
                {
                    return value != null;
                }

                if (value == null)
                {
                    return true;
                }
                
                return value.Equals(content) == false;
            }
            
            return true;
        }

        protected override string GetDefaultMessage(GUIContent label)
        {
            var propertyName = label == null ? Property.Name : label.text;
            string contentNames = Attribute.ContentGetters.IsNullOrEmpty()
                ? contents.ToFormattedString()
                : Attribute.ContentGetters.ToFormattedString();
            return $"{propertyName} cannot be equal to {contentNames}";
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (contentResolvers.IsNullOrEmpty() == false)
            {
                for (int i = 0; i < contentResolvers.Length; i++)
                {
                    if (contentResolvers[i].HasError)
                    {
                        SirenixEditorGUI.ErrorMessageBox(contentResolvers[i].ErrorMessage);
                        CallNextDrawer(label);
                        return;
                    }
                    
                    contents[i] = contentResolvers[i].GetWeakValue();
                }
            }

            base.DrawPropertyLayout(label);
        }
    }
}
#endif