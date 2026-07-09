using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    [CreateAssetMenu(menuName = FrameworkMeta.NAME + "/" + FILE_NAME, fileName = "New " + FILE_NAME)]
    public class IntCommonPreset : GeneralCommonPreset<int>
    {
        public const string FILE_NAME = "Integer Common Priority Preset";
    }
}