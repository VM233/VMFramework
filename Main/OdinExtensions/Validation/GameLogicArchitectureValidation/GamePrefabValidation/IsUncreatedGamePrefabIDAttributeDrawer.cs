#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.OdinExtensions
{
    internal sealed class IsUncreatedGamePrefabIDAttributeDrawer : 
        MultipleValidationAttributeDrawer<IsUncreatedGamePrefabIDAttribute>
    {
        protected override IEnumerable<ValidationResult> GetValidationResults(object value, GUIContent label)
        {
            if (value is not string id)
            {
                yield break;
            }

            if (id.IsNullOrWhiteSpace())
            {
                yield break;
            }

            if (GamePrefabManager.ContainsGamePrefab(id))
            {
                yield return new("ID has already been used, please use a different ID", ValidateType.Error);
            }

            foreach (var result in GameLogicArchitectureAttributeUtility.ValidateID(id))
            {
                yield return result;
            }
        }
    }
}
#endif