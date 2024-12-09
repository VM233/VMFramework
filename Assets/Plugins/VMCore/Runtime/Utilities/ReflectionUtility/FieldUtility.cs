using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static partial class ReflectionUtility
    {
        #region Static

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<FieldInfo> GetStaticFields(this Type type)
        {
            return type.GetFields(ALL_STATIC_FIELDS_FLAGS);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FieldInfo GetStaticFieldByName(this Type type, string fieldName)
        {
            return type.GetFieldByName(fieldName, ALL_STATIC_FIELDS_FLAGS);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetStaticFieldValueByName<T>(this Type type, string fieldName)
        {
            T result = default;
            var field = type.GetStaticFieldByName(fieldName);
            if (field != null)
            {
                result = (T)field.GetValue(null);
            }

            return result;
        }

        #endregion

        #region Get By Return Type

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TReturnType> GetFieldsValueByReturnType<TReturnType>(this object obj,
            BindingFlags bindingFlags = ALL_FIELDS_FLAGS) where TReturnType : class
        {
            foreach (var fieldInfo in obj.GetType().GetFieldsByReturnType(typeof(TReturnType), bindingFlags))
            {
                yield return fieldInfo.GetValue(obj) as TReturnType;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<FieldInfo> GetFieldsByReturnType(this Type type,
            Type returnType,
            BindingFlags bindingFlags = ALL_FIELDS_FLAGS)
        {
            return type.GetFields(bindingFlags).Where(fieldInfo =>
                fieldInfo.FieldType.IsDerivedFrom(returnType, true));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<FieldInfo> GetFieldsByReturnType<TReturnType>(this Type type,
            BindingFlags bindingFlags = ALL_FIELDS_FLAGS)
        {
            return type.GetFields(bindingFlags).Where(fieldInfo =>
                fieldInfo.FieldType.IsDerivedFrom(typeof(TReturnType), true));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FieldInfo GetFieldByReturnType(this Type type, Type returnType,
            BindingFlags bindingFlags = ALL_FIELDS_FLAGS)
        {
            return type.GetFieldsByReturnType(returnType, bindingFlags)
                .FirstOrDefault();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasFieldByReturnType(this Type type, Type returnType,
            BindingFlags bindingFlags = ALL_FIELDS_FLAGS)
        {
            return type.GetFieldByReturnType(returnType, bindingFlags) != null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetFieldValueByReturnType(this Type type,
            Type returnType, object targetObject,
            BindingFlags bindingFlags = ALL_FIELDS_FLAGS)
        {
            var field = type.GetFieldByReturnType(returnType, bindingFlags);

            return field?.GetValue(targetObject);
        }

        #endregion

        #region GetByName

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetFieldByName(this Type type, string name,
            out FieldInfo fieldInfo,
            BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance |
                                        BindingFlags.Public | BindingFlags.Static)
        {
            fieldInfo = type.GetFieldByName(name, bindingFlags);
            return fieldInfo != null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FieldInfo GetFieldByName(this Type type, string fieldName,
            BindingFlags bindingFlags = ALL_FIELDS_FLAGS)
        {
            return type.GetField(fieldName, bindingFlags);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetFieldValueByName<T>(this object obj, string fieldName,
            BindingFlags bindingFlags = ALL_FIELDS_FLAGS)
        {
            T result = default;
            var field = obj.GetType().GetFieldByName(fieldName, bindingFlags);
            if (field != null)
            {
                result = (T)field.GetValue(obj);
            }

            return result;
        }

        public static bool HasFieldByName(this Type type, string fieldName,
            BindingFlags bindingFlags = ALL_FIELDS_FLAGS)
        {
            return type.GetField(fieldName, bindingFlags) != null;
        }

        #endregion

        #region SetByName

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SetFieldValueByName(this object obj, string fieldName,
            object value)
        {
            var fieldInfo = obj.GetType().GetFieldByName(fieldName);

            if (fieldInfo != null)
            {
                fieldInfo.SetValue(obj, value);
                return true;
            }

            throw new ArgumentException($"没有找到名为{fieldName}的字段");
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<(FieldInfo, object)> GetAllFieldValues(
            this object obj,
            BindingFlags bindingFlags = ALL_FIELDS_FLAGS)
        {
            foreach (var fieldInfo in obj.GetType().GetFields(bindingFlags))
            {
                var value = fieldInfo.GetValue(obj);
                yield return (fieldInfo, value);
            }
        }
    }
}