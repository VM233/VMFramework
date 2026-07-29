using NUnit.Framework;
using UnityEngine;
using VMFramework.Configuration;

namespace VMFramework.Tests
{
    public sealed class CommonPresetRegistrationTests
    {
        [Test]
        public void ExistingPresetRestoresMissingInitialItemsOnlyOnce()
        {
            var generalSetting = ScriptableObject.CreateInstance<CommonPresetGeneralSetting>();
            var preset = ScriptableObject.CreateInstance<SimpleStringCommonPreset>();

            try
            {
                generalSetting.presets["Area Field Type"] = preset;

                Assert.That(
                    generalSetting.EnsurePreset(
                        "Area Field Type",
                        typeof(SimpleStringCommonPreset),
                        new[] { "Poison" },
                        null),
                    Is.True);
                Assert.That(preset.presets, Is.EqualTo(new[] { "Default", "Poison" }));

                Assert.That(
                    generalSetting.EnsurePreset(
                        "Area Field Type",
                        typeof(SimpleStringCommonPreset),
                        new[] { "Poison" },
                        null),
                    Is.False);
                Assert.That(preset.presets, Is.EqualTo(new[] { "Default", "Poison" }));
            }
            finally
            {
                Object.DestroyImmediate(preset);
                Object.DestroyImmediate(generalSetting);
            }
        }
    }
}
