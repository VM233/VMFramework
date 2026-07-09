using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    [CreateAssetMenu(menuName = FrameworkMeta.NAME + "/" + FILE_NAME, fileName = "New " + FILE_NAME)]
    public class StringCommonPreset : GeneralCommonPreset<string>
    {
        public const string FILE_NAME = "String Common Priority Preset";

        public override IEnumerable<ValueDropdownItem> GetDropdownItems()
        {
            foreach (var entry in entries)
            {
                yield return new ValueDropdownItem(entry.presetName, entry.value);
            }
        }
    }
}