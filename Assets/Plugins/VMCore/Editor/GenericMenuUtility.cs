using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace VMFramework.Core.Editor
{
    public static class GenericMenuUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddSeparator(this GenericMenu menu)
        {
            menu.AddSeparator("");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddItem(this GenericMenu menu, string label, GenericMenu.MenuFunction func)
        {
            menu.AddItem(new GUIContent(label), false, func);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddDisabledItem(this GenericMenu menu, string label)
        {
            menu.AddDisabledItem(new GUIContent(label));
        }
    }
}