#if UNITY_EDITOR
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector.Editor;
using VMFramework.Core;

namespace VMFramework.OdinExtensions
{
    public static class OdinMenuTreeUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryFindMenuItem<TValue>(this OdinMenuTree tree, out OdinMenuItem menuItem)
        {
            foreach (var treeMenuItem in tree.EnumerateTree())
            {
                if (treeMenuItem.Value is TValue)
                {
                    menuItem = treeMenuItem;
                    return true;
                }
            }
            
            menuItem = null;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyList<OdinMenuItem> FindMenuItems<TValue>(this OdinMenuTree tree)
        {
            var values = new List<OdinMenuItem>();
            
            foreach (var menuItem in tree.EnumerateTree())
            {
                if (menuItem.Value is TValue)
                {
                    values.Add(menuItem);
                }
            }

            return values;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static OdinMenuItem FindMenuItem(this OdinMenuTree tree, object value)
        {
            foreach (var menuItem in tree.EnumerateTree())
            {
                if (menuItem.Value == value)
                {
                    return menuItem;
                }
            }

            return null;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryFindMenuItem(this OdinMenuTree tree, object value, out OdinMenuItem menuItem)
        {
            foreach (var item in tree.EnumerateTree())
            {
                if (item.Value == value)
                {
                    menuItem = item;
                    return true;
                }
            }

            menuItem = null;
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddIfNotContains(this OdinMenuTreeSelection selection, OdinMenuItem menuItem)
        {
            if (selection.Contains(menuItem))
            {
                selection.Remove(menuItem);
            }
            else
            {
                selection.Add(menuItem);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Select(this OdinMenuTreeSelection selection, OdinMenuItem menuItem, bool additive = false)
        {
            if (additive)
            {
                selection.AddIfNotContains(menuItem);
            }
            else
            {
                selection.Clear();
                selection.Add(menuItem);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Select(this OdinMenuTreeSelection selection, IEnumerable<OdinMenuItem> menuItems, bool additive = false)
        {
            if (additive)
            {
                foreach (var menuItem in menuItems)
                {
                    selection.AddIfNotContains(menuItem);
                }
            }
            else
            {
                selection.Clear();
                selection.AddRange(menuItems);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Select(this OdinMenuTree tree, object value)
        {
            if (tree.TryFindMenuItem(value, out var menuItem))
            {
                tree.Selection.Select(menuItem);
            }
        }
    }
}
#endif