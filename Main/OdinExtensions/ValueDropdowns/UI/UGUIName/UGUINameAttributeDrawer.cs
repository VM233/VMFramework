#if UNITY_EDITOR && ODIN_INSPECTOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using VMFramework.Core;
using VMFramework.UI;

namespace VMFramework.OdinExtensions
{
    internal sealed class UGUINameAttributeDrawer : GeneralValueDropdownAttributeDrawer<UGUINameAttribute>
    {
        protected override void Validate()
        {
            base.Validate();
            
            foreach (var parent in Property.TraverseToRoot(false, property => property.Parent))
            {
                var value = parent?.ValueEntry?.WeakSmartValue;

                if (value is IUGUIAssetProvider)
                {
                    return;
                }
            }

            SirenixEditorGUI.ErrorMessageBox(
                $"The property {Property.Name} is not a child of a {nameof(IUGUIAssetProvider)}.");
        }

        protected override IEnumerable<ValueDropdownItem> GetValues()
        {
            foreach (var parent in Property.TraverseToRoot(false, property => property.Parent))
            {
                var value = parent?.ValueEntry?.WeakSmartValue;

                if (value is IUGUIAssetProvider provider)
                {
                    if (provider.UGUIAsset == null)
                    {
                        return Enumerable.Empty<ValueDropdownItem>();
                    }
                    
                    return provider.UGUIAsset.transform.GetAllChildrenNames(Attribute.UGUITypes, true)
                        .ToValueDropdownItems();
                }
            }
            
            return Enumerable.Empty<ValueDropdownItem>();
        }
    }
}
#endif