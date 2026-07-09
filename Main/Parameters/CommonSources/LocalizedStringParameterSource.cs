using System;
using UnityEngine.Localization;

namespace VMFramework.Parameters
{
    [Serializable]
    public class LocalizedStringParameterSource : IParameterSourceConfig<string>, IParameterSource<string>
    {
        public LocalizedString localizedString = new();

        public IParameterSource<string> GetParameterSource() => this;

        public virtual bool TryGetValue(ref string value)
        {
            value = localizedString.GetLocalizedString();
            return true;
        }
    }
}