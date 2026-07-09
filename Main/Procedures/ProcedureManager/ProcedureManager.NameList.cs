#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;

namespace VMFramework.Procedure
{
    public partial class ProcedureManager
    {
        private static readonly TypeCollector<IProcedure> procedureCollector = new()
        {
            IncludingSelf = false,
            IncludingAbstract = false,
            IncludingInterface = false,
            IncludingGenericDefinition = false
        };

        public static IEnumerable<ValueDropdownItem> GetNameList()
        {
            if (procedureCollector.Count == 0)
            {
                procedureCollector.Collect();
            }

            foreach (var procedureType in procedureCollector.GetCollectedTypes())
            {
                var id = procedureType.GetStaticFieldValueByName<string>("ID");

                if (id.IsNullOrEmpty() == false)
                {
                    yield return new ValueDropdownItem(procedureType.Name, id);
                }
            }
        }
    }
}
#endif