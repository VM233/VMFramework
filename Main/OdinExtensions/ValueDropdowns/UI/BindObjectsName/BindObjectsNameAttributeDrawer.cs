#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.UI;

namespace VMFramework.OdinExtensions
{
    public class BindObjectsNameAttributeDrawer : GeneralValueDropdownAttributeDrawer<BindObjectsNameAttribute>
    {
        protected override void Validate()
        {
            base.Validate();

            foreach (var parent in Property.TraverseToRoot(false, property => property.Parent))
            {
                if (parent.ParentValues.IsNullOrEmpty() == false)
                {
                    var parentValue = parent.ParentValues.First();

                    if (parentValue is Component component)
                    {
                        var manager = component.GetComponentInParent<BindObjectsManager>();

                        if (manager != null)
                        {
                            return;
                        }
                    }
                }
            }

            SirenixEditorGUI.ErrorMessageBox(
                $"The property {Property.Name} is not a child of a {nameof(BindObjectsManager)} component.");
        }

        protected override IEnumerable<ValueDropdownItem> GetValues()
        {
            foreach (var parent in Property.TraverseToRoot(false, property => property.Parent))
            {
                BindObjectsManager bindObjectsManager = null;

                string thisGameObjectName = null;
                string parentGameObjectName = null;

                if (parent.ParentValues.IsNullOrEmpty() == false)
                {
                    var parentValue = parent.ParentValues.First();

                    if (parentValue is Component component)
                    {
                        bindObjectsManager = component.GetComponentInParent<BindObjectsManager>();
                        thisGameObjectName = component.name;

                        if (component.transform.parent != null)
                        {
                            parentGameObjectName = component.transform.parent.name;
                        }
                    }
                }

                if (bindObjectsManager == null)
                {
                    continue;
                }

                bindObjectsManager.Collect();

                IEnumerable<string> targetNames = bindObjectsManager.BindNames;

                if (Attribute.SingleModeLimit is BindObjectsNameAttribute.SingleModeLimitType.Single)
                {
                    targetNames = targetNames.Where(name => bindObjectsManager.SingleModeNames.Contains(name));
                }
                else if (Attribute.SingleModeLimit is BindObjectsNameAttribute.SingleModeLimitType.NotSingle)
                {
                    targetNames = targetNames.Where(name => bindObjectsManager.SingleModeNames.Contains(name) == false);
                }

                if (Attribute.AutoFirstThisGameObject)
                {
                    if (thisGameObjectName.IsNullOrWhiteSpace() == false)
                    {
                        targetNames = targetNames.OrderBy(name => name == thisGameObjectName ? 0 : 1);
                    }
                }

                if (Attribute.AutoFirstParentGameObject)
                {
                    if (parentGameObjectName.IsNullOrWhiteSpace() == false)
                    {
                        targetNames = targetNames.OrderBy(name => name == parentGameObjectName ? 0 : 1);
                    }
                }

                return targetNames.ToValueDropdownItems();
            }

            return Array.Empty<ValueDropdownItem>();
        }
    }
}
#endif