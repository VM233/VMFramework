#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class QueryAllInterfacesUnit : SingleButtonBatchProcessorUnit
    {
        protected override string ProcessButtonName => "Get All Interfaces";

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.Any(obj => obj is Type);
        }

        protected override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            foreach (var obj in selectedObjects)
            {
                if (obj is Type type)
                {
                    foreach (var interfaceType in type.GetInterfaces())
                    {
                        yield return interfaceType;
                    }
                    
                    continue;
                }
                
                yield return obj;
            }
        }
    }
}
#endif