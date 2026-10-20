#if UNITY_EDITOR
using System.Linq;
using VMCommonPreset.Editor;
using VMFramework.Configuration;

namespace VMFramework.GameLogicArchitecture
{
    internal sealed class CommonPresetMenuSnapshot
    {
        private readonly (string Key, CommonPreset Preset)[] inputs = Read();

        internal bool IsCurrent => inputs.SequenceEqual(Read());

        private static (string, CommonPreset)[] Read() => CommonPresetProjectSettings.Presets
            .Where(entry => entry?.Preset != null)
            .Select(entry => (entry.Key, entry.Preset)).ToArray();
    }
}
#endif
