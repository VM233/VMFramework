using System;
using System.Diagnostics;

namespace VMFramework.Configuration
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    [Conditional("UNITY_EDITOR")]
    public class FixedCommonPresetRegisterAttribute : Attribute
    {
        public enum RegisterMode
        {
            CreatePreset,
            RegisterEntry,
            RegisterConstValues
        }

        public RegisterMode Mode { get; set; }

        public string Key { get; set; }

        public Type ValueType { get; set; }
        
        public bool SimplifiedValueType { get; set; }

        public string PresetKey { get; set; }

        public object Value { get; set; }

        public string[] ExcludedFields { get; set; }
        
        /// <summary>
        /// 仅作为记录，无需初始化
        /// </summary>
        public Type ClassType { get; set; }

        public FixedCommonPresetRegisterAttribute(string key, Type valueType, bool simplifiedValueType = true)
        {
            Mode = RegisterMode.CreatePreset;
            Key = key;
            ValueType = valueType;
            SimplifiedValueType = simplifiedValueType;
        }

        public FixedCommonPresetRegisterAttribute(string key, string presetKey, object value)
        {
            Mode = RegisterMode.RegisterEntry;
            Key = key;
            PresetKey = presetKey;
            Value = value;
        }

        public FixedCommonPresetRegisterAttribute(string key, string[] excludedFields = null)
        {
            Mode = RegisterMode.RegisterConstValues;
            Key = key;
            ExcludedFields = excludedFields;
        }
    }
}