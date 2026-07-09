using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using VMFramework.Core.Linq;
using VMFramework.Core.Pools;

namespace VMFramework.Containers
{
    public static class ContainerEnumerableUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IContainerItem> ToItems([DisallowNull] this IEnumerable<IContainer> containers)
        {
            return containers.WhereNotNull().SelectMany(container => container);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<IContainerItem> ToItemsListDefaultPooled(
            [DisallowNull] this IEnumerable<IContainer> containers)
        {
            var items = ListPool<IContainerItem>.Default.Get();
            items.Clear();

            foreach (var container in containers)
            {
                if (container == null)
                {
                    continue;
                }

                items.AddRange(container.ValidItems);
            }

            return items;
        }
    }
}