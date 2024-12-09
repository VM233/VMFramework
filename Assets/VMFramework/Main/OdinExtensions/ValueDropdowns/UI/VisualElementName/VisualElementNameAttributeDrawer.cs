#if UNITY_EDITOR && ODIN_INSPECTOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using VMFramework.Core;
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
                var value = parent?.ValueEntry?.WeakSmartValue;

                if (value is IVisualTreeAssetProvider)
                {
                    return;
                }
            }

            SirenixEditorGUI.ErrorMessageBox(
                $"The property {Property.Name} is not a child of a {nameof(IVisualTreeAssetProvider)}.");
        }

        protected override IEnumerable<ValueDropdownItem> GetValues()
        {
            foreach (var parent in Property.TraverseToRoot(false, property => property.Parent))
            {
                var value = parent?.ValueEntry?.WeakSmartValue;

                if (value is IVisualTreeAssetProvider provider)
                {
                    return provider.VisualTree.GetAllNamesByTypes(Attribute.VisualElementTypes)
                        .Where(name => name.IsLowercaseAndHyphenOnly() == false).ToValueDropdownItems();
                }
            }

            return Enumerable.Empty<ValueDropdownItem>();
        }
    }
}
#endif