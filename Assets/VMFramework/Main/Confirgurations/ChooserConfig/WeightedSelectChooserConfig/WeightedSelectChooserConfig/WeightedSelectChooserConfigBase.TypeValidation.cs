#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using VMFramework.Core.Linq;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    [TypeValidation]
    public partial class WeightedSelectChooserConfigBase<TWrapper, TItem> : ITypeValidationProvider
    {
        protected virtual IEnumerable<ValidationResult> GetValidationResults(GUIContent label)
        {
            if (items.IsNullOrEmpty())
            {
                yield return new("The probability weight list cannot be empty!", ValidateType.Error);
                yield break;
            }

            if (IsItemsContainsSameValue())
            {
                yield return new("The probability weight list cannot contain duplicate values!",
                    ValidateType.Error);
            }

            if (IsRatiosCoprime() == false)
            {
                yield return new(
                    "The probability weight list contains non-coprime ratios, which can be simplified!",
                    ValidateType.Warning);
            }

            if (IsRatiosAllZero())
            {
                yield return new(
                    "The probability weight list contains all zero ratios, which is not allowed!",
                    ValidateType.Error);
            }

            if (items.Count == 1)
            {
                yield return new("The probability weight list contains only one item, which is not allowed!",
                    ValidateType.Error);
            }
        }

        IEnumerable<ValidationResult> ITypeValidationProvider.GetValidationResults(GUIContent label)
        {
            return GetValidationResults(label);
        }
    }
}
#endif