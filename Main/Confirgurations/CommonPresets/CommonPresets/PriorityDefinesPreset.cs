using VMFramework.Core;

namespace VMFramework.Configuration
{
    [FixedCommonPresetRegister(NAME, typeof(int))]
    [FixedCommonPresetRegister(NAME, nameof(PriorityDefines.TINY), PriorityDefines.TINY)]
    [FixedCommonPresetRegister(NAME, nameof(PriorityDefines.LOW), PriorityDefines.LOW)]
    [FixedCommonPresetRegister(NAME, nameof(PriorityDefines.MEDIUM), PriorityDefines.MEDIUM)]
    [FixedCommonPresetRegister(NAME, nameof(PriorityDefines.HIGH), PriorityDefines.HIGH)]
    [FixedCommonPresetRegister(NAME, nameof(PriorityDefines.SUPER), PriorityDefines.SUPER)]
    [FixedCommonPresetRegister(NAME, nameof(PriorityDefines.ULTRA), PriorityDefines.ULTRA)]
    public static class PriorityDefinesPreset
    {
        public const string NAME = "General Priority";
    }
}