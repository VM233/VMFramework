using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace VMFramework.Core
{
    public sealed class TypeCollector<TType>
    {
        public bool IncludingSelf { get; init; }

        public bool IncludingAbstract { get; init; }

        public bool IncludingInterface { get; init; }

        public bool IncludingGenericDefinition { get; init; }

        private readonly List<Type> types = new();

        public int Count => types.Count;

        public void Collect()
        {
            types.Clear();

            foreach (var type in typeof(TType).GetDerivedClasses(IncludingSelf, IncludingGenericDefinition))
            {
                if (IncludingAbstract == false && type.IsAbstract)
                {
                    continue;
                }

                if (IncludingInterface == false && type.IsInterface)
                {
                    continue;
                }

                types.Add(type);
            }
        }

        public IReadOnlyList<Type> GetCollectedTypes()
        {
            return types;
        }

        public IEnumerable<ValueDropdownItem<Type>> GetNameList()
        {
            if (types.Count == 0)
            {
                Collect();
            }

            foreach (var type in types)
            {
                yield return new ValueDropdownItem<Type>(type.Name, type);
            }
        }
    }
}