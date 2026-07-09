using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    [CreateAssetMenu(menuName = FrameworkMeta.NAME + "/" + FILE_NAME, fileName = "New " + FILE_NAME)]
    public class GeneralObjectFilter : ObjectFilter
    {
        public const string FILE_NAME = "General Object Filter";
        
        [EnumToggleButtons]
        public GeneralObjectFilterType filterType;

        [EnabledIfHasFlag(nameof(filterType), GeneralObjectFilterType.Type)]
        public TypeFilter typeFilter;
        
        [EnabledIfHasFlag(nameof(filterType), GeneralObjectFilterType.GameTag)]
        public GameTagFilter gameTagFilter;
        
        [EnabledIfHasFlag(nameof(filterType), GeneralObjectFilterType.ComponentType)]
        public ComponentTypeFilter componentTypeFilter;

        public override bool IsMatch(object obj)
        {
            bool result = true;

            if (filterType.HasFlag(GeneralObjectFilterType.Type))
            {
                result &= typeFilter.IsMatch(obj);
            }
            if (filterType.HasFlag(GeneralObjectFilterType.GameTag))
            {
                result &= gameTagFilter.IsMatch(obj);
            }
            if (filterType.HasFlag(GeneralObjectFilterType.ComponentType))
            {
                result &= componentTypeFilter.IsMatch(obj);
            }

            return result;
        }
    }
}