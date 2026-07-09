#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class FilterObjectsContainSpecificNameUnit : DoubleButtonBatchProcessorUnit
    {
        protected override string ProcessButtonOneName => "Remove Containing Specific Name";

        protected override string ProcessButtonTwoName => "Keep Containing Specific Name";

        public string specificName;

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.Any(o =>
                o is Object);
        }

        protected override IEnumerable<object> OnProcessOne(
            IEnumerable<object> selectedObjects)
        {
            if (specificName.IsNullOrEmpty())
            {
                return selectedObjects;
            }
            
            return selectedObjects.Where(o =>
                o is Object obj && obj.name.Contains(specificName) == false);
        }

        protected override IEnumerable<object> OnProcessTwo(
            IEnumerable<object> selectedObjects)
        {
            if (specificName.IsNullOrEmpty())
            {
                return selectedObjects;
            }
            
            return selectedObjects.Where(o =>
                o is Object obj && obj.name.Contains(specificName));
        }
    }
}

#endif