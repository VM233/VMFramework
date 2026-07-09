#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.OdinExtensions
{
    internal sealed class CommonPresetAttributeDrawer : GeneralValueDropdownAttributeDrawer<CommonPresetAttribute>
    {
        protected override IEnumerable<ValueDropdownItem> GetValues()
        {
            if (Attribute.Keys.IsNullOrEmpty())
            {
                yield break;
            }
            
            var setting = CoreSetting.CommonPresetGeneralSetting;
            
            foreach (var key in Attribute.Keys)
            {
                if (FixedCommonPresetInfo.presets.TryGetValue(key, out var fixedPreset))
                {
                    foreach (var item in fixedPreset.GetDropdownItems())
                    {
                        yield return item;
                    }
                }
                
                if (setting == null)
                {
                    continue;
                }

                if (setting.presets == null)
                {
                    continue;
                }
                
                if (setting.presets.TryGetValue(key, out var preset))
                {
                    foreach (var item in preset.GetDropdownItems())
                    {
                        yield return item;
                    }
                }
            }
        }

        protected override void DrawCustomButtons()
        {
            base.DrawCustomButtons();

            var setting = CoreSetting.CommonPresetGeneralSetting;

            if (setting == null)
            {
                return;
            }

            var presets = CoreSetting.CommonPresetGeneralSetting.presets;

            if (presets == null)
            {
                return;
            }

            if (Button(GameEditorNames.JUMP_TO_GAME_EDITOR, SdfIconType.Search))
            {
                var firstKey = Attribute.Keys.FirstOrDefault() ?? string.Empty;

                if (presets.TryGetValue(firstKey, out var preset))
                {
                    var gameEditor = EditorWindow.GetWindow<GameEditor>();

                    gameEditor.SelectValue(preset);
                }
            }
        }
    }
}
#endif