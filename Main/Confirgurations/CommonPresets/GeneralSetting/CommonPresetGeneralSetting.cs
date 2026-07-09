using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Configuration
{
    public sealed partial class CommonPresetGeneralSetting : GeneralSetting
    {
        public const string PRESETS_CATEGORY = "Presets";

        [TabGroup(TAB_GROUP_NAME, PRESETS_CATEGORY)]
        public Dictionary<string, CommonPreset> presets = new();

        public override void CheckSettings()
        {
            base.CheckSettings();

            foreach (var (key, preset) in presets)
            {
                if (preset == null)
                {
                    throw new ArgumentNullException($"Preset with key {key} is null.");
                }
            }
        }
    }
}