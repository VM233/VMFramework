using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;
using VMFramework.Core;

namespace VMFramework.UI
{
    public static class VisualElementSelectorUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VisualElement MandatoryQuery(this IVisualElementSelector selector, VisualElement root,
            string pathSourceName)
        {
            var element = selector.Query(root);

            element.AssertIsNotNull(pathSourceName);

            return element;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MandatoryQuery<T>(this IVisualElementSelector selector, VisualElement root,
            string pathSourceName)
            where T : VisualElement
        {
            var element = selector.Query<T>(root);

            element.AssertIsNotNull($"{pathSourceName} of {typeof(T)}");

            return element;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<VisualElement> Query(this IEnumerable<IVisualElementSelector> selectors,
            VisualElement root)
        {
            return selectors.Select(selector => selector.Query(root));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> Query<T>(this IEnumerable<IVisualElementSelector> selectors, VisualElement root)
            where T : VisualElement
        {
            return selectors.Select(selector => selector.Query<T>(root));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<VisualElement> MandatoryQuery(this IEnumerable<IVisualElementSelector> selectors,
            VisualElement root, string pathSourceName)
        {
            return selectors.Select(selector => selector.MandatoryQuery(root, pathSourceName));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> MandatoryQuery<T>(this IEnumerable<IVisualElementSelector> selectors,
            VisualElement root, string pathSourceName)
            where T : VisualElement
        {
            return selectors.Select(selector => selector.MandatoryQuery<T>(root, pathSourceName));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<VisualElement> Query(this IEnumerable<VisualElementSelection> selections,
            VisualElement root)
        {
            foreach (var selection in selections)
            {
                foreach (var element in selection.Query(root))
                {
                    yield return element;
                }
            }
        }
    }
}