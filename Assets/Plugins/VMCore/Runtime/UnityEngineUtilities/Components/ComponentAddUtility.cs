using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class ComponentAddUtility
    {
        #region Get Or Add

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNull]
        public static T GetOrAddComponent<T>(this Component component)
            where T : Component
        {
            var result = component.GetComponent<T>();

            if (result == null)
            {
                result = component.gameObject.AddComponent<T>();
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNull]
        public static Component GetOrAddComponent(this Component component, Type componentType)
        {
            var result = component.GetComponent(componentType);

            if (result == null)
            {
                result = component.gameObject.AddComponent(componentType);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNull]
        public static T GetOrAddComponent<T>(this GameObject gameObject)
            where T : Component
        {
            var result = gameObject.GetComponent<T>();

            if (result == null)
            {
                result = gameObject.AddComponent<T>();
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNull]
        public static Component GetOrAddComponent(this GameObject gameObject, Type componentType)
        {
            var result = gameObject.GetComponent(componentType);

            if (result == null)
            {
                result = gameObject.AddComponent(componentType);
            }

            return result;
        }

        #endregion

        #region Add

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNull]
        public static T AddComponent<T>(this Component component)
            where T : Component
        {
            return component.gameObject.AddComponent<T>();
        }

        #endregion
    }
}
