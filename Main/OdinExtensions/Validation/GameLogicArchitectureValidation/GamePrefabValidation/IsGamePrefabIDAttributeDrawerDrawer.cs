#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VMFramework.OdinExtensions
{
    internal sealed class IsGamePrefabIDAttributeDrawer : MultipleValidationAttributeDrawer<IsGamePrefabIDAttribute>
    {
        protected override IEnumerable<ValidationResult> GetValidationResults(object value, GUIContent label)
        {
            if (value is not string id)
            {
                return Enumerable.Empty<ValidationResult>();
            }

            return GameLogicArchitectureAttributeUtility.ValidateID(id);
        }
    }
}
#endif