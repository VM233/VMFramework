#if UNITY_EDITOR && ODIN_INSPECTOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.UI;

namespace VMFramework.OdinExtensions
{
    internal sealed class VisualElementNameAttributeDrawer
        : GeneralValueDropdownAttributeDrawer<VisualElementNameAttribute>
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
                        if (Attribute.IsProvider)
                        {
                            var assetProvider = component.GetComponentInParent<IVisualTreeAssetProvider>();

                            if (assetProvider != null)
                            {
                                return;
                            }
                        }
                        else
                        {
                            var uiDocument = component.GetComponentInParent<UIDocument>();

                            if (uiDocument != null)
                            {
                                return;
                            }
                        }
                    }
                }
            }

            if (Attribute.IsProvider)
            {
                SirenixEditorGUI.ErrorMessageBox(
                    $"The property {Property.Name} is not a child of a {nameof(IVisualTreeAssetProvider)} component.");
            }
            else
            {
                SirenixEditorGUI.ErrorMessageBox(
                    $"The property {Property.Name} is not a child of a {nameof(IVisualTreeAssetProvider)}" +
                    $" or a {nameof(UIDocument)} component child.");
            }
        }

        protected override IEnumerable<ValueDropdownItem> GetValues()
        {
            foreach (var parent in Property.TraverseToRoot(false, property => property.Parent))
            {
                VisualTreeAsset visualTree = null;

                if (parent.ParentValues.IsNullOrEmpty() == false)
                {
                    var parentValue = parent.ParentValues.First();

                    if (parentValue is Component component)
                    {
                        if (Attribute.IsProvider)
                        {
                            var assetProvider = component.GetComponentInParent<IVisualTreeAssetProvider>();

                            if (assetProvider != null)
                            {
                                visualTree = assetProvider.VisualTree;
                            }
                        }
                        else
                        {
                            var uiDocument = component.GetComponentInParent<UIDocument>(true);

                            if (uiDocument)
                            {
                                visualTree = uiDocument.visualTreeAsset;
                            }
                        }
                    }
                }

                if (visualTree == null)
                {
                    continue;
                }

                return visualTree.GetAllNamesByTypes(Attribute.VisualElementTypes)
                    .Where(name => name.IsLowercaseAndHyphenOnly() == false).ToValueDropdownItems();
            }

            return Enumerable.Empty<ValueDropdownItem>();
        }
    }
}
#endif