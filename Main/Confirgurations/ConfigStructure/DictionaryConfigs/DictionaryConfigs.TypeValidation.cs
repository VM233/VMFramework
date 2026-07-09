#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VMFramework.Core.Linq;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    public partial class DictionaryConfigs<TID, TConfig>
    {
        protected override IEnumerable<ValidationResult> GetValidationResults(GUIContent label)
        {
            foreach (var result in base.GetValidationResults(label))
            {
                yield return result;
            }

            if (configs.IsAnyNull())
            {
                yield return new("All Configs must be non-null.", ValidateType.Error);
                yield break;
            }
            
            if (configs.Any(config => config.id == null || config.id is ""))
            {
                yield return new("All Configs must have a non-empty ID.", ValidateType.Error);
            }

            if (configs.Select(config => config.id).ContainsSame())
            {
                yield return new("All Configs must have unique IDs.", ValidateType.Error);
            }
        }
    }
}
#endif