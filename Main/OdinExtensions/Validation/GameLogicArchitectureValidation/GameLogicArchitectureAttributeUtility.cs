#if UNITY_EDITOR
using System.Collections.Generic;
using VMFramework.Core;

namespace VMFramework.OdinExtensions
{
    public static class GameLogicArchitectureAttributeUtility
    {
        public static IEnumerable<ValidationResult> ValidateID(string gamePrefabID)
        {
            if (gamePrefabID.IsNullOrEmpty())
            {
                yield break;
            }
            
            if (gamePrefabID.EndsWith("_"))
            {
                yield return new("ID is not recommended to end with an underscore : _", ValidateType.Warning);
            }
            
            if (gamePrefabID.Contains(" "))
            {
                yield return new("ID is not recommended to contain spaces", ValidateType.Warning);
            }
            
            if (gamePrefabID.Contains("-"))
            {
                yield return new("ID is not recommended to contain hyphens : -", ValidateType.Warning);
            }

            if (gamePrefabID.HasUppercaseLetter())
            {
                yield return new("ID is not recommended to contain uppercase letters", ValidateType.Warning);
            }
        }
    }
}
#endif