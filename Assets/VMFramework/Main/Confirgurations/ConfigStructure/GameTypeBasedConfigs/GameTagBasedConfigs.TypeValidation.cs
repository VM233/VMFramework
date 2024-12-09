#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    public partial class GameTagBasedConfigs<TConfig>
    {
        protected override IEnumerable<ValidationResult> GetValidationResults(GUIContent label)
        {
            foreach (var result in base.GetValidationResults(label))
            {
                yield return result;
            }

            if (configs.Count == 0)
            {
                yield break;
            }

            if (configs.Any(config => config.GameTags == null))
            {
                yield return new($"There is null GameTags Collections in the configurations.", ValidateType.Error);
                yield break;
            }

            if (configs.Any(config => config.GameTags.Any() == false))
            {
                yield return new($"Each configuration must have at least one {nameof(GameTag)} ID assigned.",
                    ValidateType.Error);
            }

            if (configs.Any(config => config.GameTags.Any(id => id.IsNullOrEmpty())))
            {
                yield return new($"Each {nameof(GameTag)} ID must be a valid string.", ValidateType.Error);
            }
            
            if (configs.Select(config => config.GameTags.AsEnumerable()).Aggregate((one, two) => one.Concat(two))
                    .IsUnique() == false)
            {
                yield return new("Each game type ID must be unique across all configurations.",
                    ValidateType.Error);
            }
        }
    }
}
#endif