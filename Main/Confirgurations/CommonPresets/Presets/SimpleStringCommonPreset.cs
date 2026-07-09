using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    public class SimpleStringCommonPreset : CommonPreset
    {
        [DisallowDuplicateElements]
        [ListDrawerSettings(ShowFoldout = false)]
        [Searchable]
        public List<string> presets = new()
        {
            "Default"
        };

        public override IEnumerable<ValueDropdownItem> GetDropdownItems()
        {
            return presets.Select(preset => new ValueDropdownItem(preset, preset));
        }

        public override void AddItem(string key, object value)
        {
            presets.Add(key);
        }

        public override void ClearItems()
        {
            presets.Clear();
        }
    }
}