using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Localization;

namespace VMFramework.Configuration
{
    public class ColorWeightedSelectChooserConfig : WeightedSelectChooserConfig<Color>
    {
        [EnumToggleButtons]
        public ColorStringFormat colorStringFormat = ColorStringFormat.Name;

        protected override string WrapperToString(Color value)
        {
            return value.ToLocalizedString(colorStringFormat);
        }
    }
}