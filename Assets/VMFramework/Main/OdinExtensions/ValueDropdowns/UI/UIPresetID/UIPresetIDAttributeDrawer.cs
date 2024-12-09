#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.GameLogicArchitecture;
using VMFramework.UI;

namespace VMFramework.OdinExtensions
{
    internal sealed class UIPresetIDAttributeDrawer : GamePrefabIDAttributeDrawer<UIPresetIDAttribute>
    {
        protected override IEnumerable<ValueDropdownItem> GetValues()
        {
            foreach (var uiPreset in GamePrefabManager.GetGamePrefabsByTypes<IUIPanelConfig>(Attribute
                         .GamePrefabTypes))
            {
                if (Attribute.IsUnique != null)
                {
                    if (uiPreset.IsUnique != Attribute.IsUnique)
                    {
                        continue;
                    }
                }

                yield return uiPreset.GetNameIDDropDownItem();
            }
        }
    }
}
#endif