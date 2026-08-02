using VMFramework.Core;

namespace VMFramework.Configuration
{
    [FixedCommonPreset(NAME, typeof(int))]
    [FixedCommonPresetValue(NAME, nameof(PriorityDefines.TINY), PriorityDefines.TINY)]
    [FixedCommonPresetValue(NAME, nameof(PriorityDefines.LOW), PriorityDefines.LOW)]
    [FixedCommonPresetValue(NAME, nameof(PriorityDefines.MEDIUM), PriorityDefines.MEDIUM)]
    [FixedCommonPresetValue(NAME, nameof(PriorityDefines.HIGH), PriorityDefines.HIGH)]
    [FixedCommonPresetValue(NAME, nameof(PriorityDefines.SUPER), PriorityDefines.SUPER)]
    [FixedCommonPresetValue(NAME, nameof(PriorityDefines.ULTRA), PriorityDefines.ULTRA)]
    public static class PriorityDefinesPreset
    {
        public const string NAME = "General Priority";
    }
}
