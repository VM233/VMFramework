using System;
using System.Collections.Generic;
using UnityEngine.Localization;
using VMFramework.Configuration;

namespace VMFramework.Localization
{
    [Serializable]
    public class SingleArgumentLocalizedString : BaseConfig
    {
        public LocalizedString localizedString = new();

        public string argumentName;

        private readonly Dictionary<string, object> arguments = new();

        protected override void OnInit()
        {
            base.OnInit();

            localizedString.Arguments = new List<object> { arguments };
        }

        public string GetLocalizedString(object argument)
        {
            arguments[argumentName] = argument;
            return localizedString.GetLocalizedString();
        }
    }
}