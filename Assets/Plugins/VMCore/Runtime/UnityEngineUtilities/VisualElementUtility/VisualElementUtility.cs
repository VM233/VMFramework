using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace VMFramework.Core
{
    public static partial class VisualElementUtility
    {
        #region Clear All

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClearAll<T>(this VisualElement root) where T : VisualElement
        {
            foreach (var child in root.GetAll<T>().ToList())
            {
                child.RemoveFromHierarchy();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClearAll<T>(this VisualElement root, Func<T, bool> predicate)
            where T : VisualElement
        {
            foreach (var child in root.GetAll<T>().ToList())
            {
                if (predicate(child))
                {
                    child.RemoveFromHierarchy();
                }
            }
        }

        #endregion

        #region Button

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void QueryAndInitButton(this VisualElement root, string name, Action onClick)
        {
            var button = root.Q<Button>(name);

            if (button != null)
            {
                button.clicked += onClick;
            }
        }

        #endregion
    }
}