using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace VMFramework.Configuration
{
    public class GeneralCommonPreset<TValue> : CommonPreset
    {
        [ListDrawerSettings(ShowFoldout = false)]
        [Searchable]
        public List<CommonPresetEntry<TValue>> entries = new()
        {
            new("Default", default)
        };

        public override IEnumerable<ValueDropdownItem> GetDropdownItems()
        {
            foreach (var entry in entries)
            {
                yield return new ValueDropdownItem($"{entry.presetName}:{entry.value}", entry.value);
            }
        }

        public override void AddItem(string key, object value)
        {
            entries.Add(new CommonPresetEntry<TValue>(key, (TValue)value));
        }

        public override void ClearItems()
        {
            entries.Clear();
        }
    }
}