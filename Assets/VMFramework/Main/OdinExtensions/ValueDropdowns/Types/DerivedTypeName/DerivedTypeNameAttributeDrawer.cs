﻿#if UNITY_EDITOR && ODIN_INSPECTOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace VMFramework.OdinExtensions
{
    [DrawerPriority(0, 0, 2002)]
    internal sealed class DerivedTypeNameAttributeDrawer : GeneralValueDropdownAttributeDrawer<DerivedTypeNameAttribute>
    {
        protected override IEnumerable<ValueDropdownItem> GetValues()
        {
            foreach (var parentType in Attribute.ParentTypes)
            {
                foreach (var valueDropdownItem in parentType.GetDerivedTypeNamesNameList(Attribute.IncludingSelf,
                             Attribute.IncludingInterfaces, Attribute.IncludingGeneric, Attribute.IncludingAbstract,
                             Attribute.IncludingSealed))
                {
                    yield return valueDropdownItem;
                }
            }
        }
    }
}
#endif