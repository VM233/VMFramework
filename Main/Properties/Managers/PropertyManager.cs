using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.OdinExtensions;

namespace VMFramework.Properties
{
    [DisallowMultipleComponent]
    public class PropertyManager : MonoBehaviour
    {
        [ComponentRequired(typeof(IPropertyProvider))]
        public List<GameObject> excludeProviderObjects = new();

        public IReadOnlyDictionary<string, IReadOnlyProperty> Properties => properties;

        [ShowInInspector]
        protected readonly Dictionary<string, IReadOnlyProperty> properties = new();

        protected virtual void Awake()
        {
            Collect();
        }

        public virtual void Collect()
        {
            properties.Clear();

            var excludePropertyProviders = HashSetPool<IPropertyProvider>.Default.Get();
            excludePropertyProviders.Clear();

            var propertyInfos = ListPool<IReadOnlyProperty>.Default.Get();

            foreach (var propertyProviderObject in excludeProviderObjects)
            {
                foreach (var propertyProvider in propertyProviderObject.GetComponents<IPropertyProvider>())
                {
                    excludePropertyProviders.Add(propertyProvider);
                }
            }

            foreach (var propertyProvider in GetComponentsInChildren<IPropertyProvider>())
            {
                propertyInfos.Clear();
                propertyProvider.GetProperties(propertyInfos);

                foreach (var property in propertyInfos)
                {
                    if (property.Name.IsNullOrEmpty())
                    {
                        UnityEngine.Debug.LogError($"property from {propertyProvider} has no name", context: this);
                    }

                    RegisterProperty(property);
                }
            }

            excludePropertyProviders.ReturnToDefaultPool();
            propertyInfos.ReturnToDefaultPool();
        }

        public virtual void RegisterProperty(IReadOnlyProperty property)
        {
            if (properties.TryAdd(property.Name, property) == false)
            {
                UnityEngine.Debug.LogError($"Property with name {property.Name} already exists in the property manager.", this);
            }
        }

        public virtual bool TryGetProperty<TProperty>(string propertyName,
            [NotNullWhen(returnValue: true)] [MaybeNullWhen(returnValue: false)] out TProperty property)
            where TProperty : IReadOnlyProperty
        {
            if (propertyName == null)
            {
                property = default;
                return false;
            }

            if (properties.TryGetValue(propertyName, out var readOnlyProperty) == false)
            {
                property = default;
                return false;
            }

            property = (TProperty)readOnlyProperty;
            return true;
        }

        public virtual void GetPropertyStrictly<TProperty>(string propertyName, out TProperty property)
            where TProperty : IReadOnlyProperty
        {
            if (properties.TryGetValue(propertyName, out var readOnlyProperty) == false)
            {
                UnityEngine.Debug.LogError($"Property with name {propertyName} does not exist in the property manager.",
                    context: this);
                property = default;
            }
            else
            {
                property = (TProperty)readOnlyProperty;
            }
        }

        public virtual void GetPropertiesStrictly<TProperty>(IEnumerable<string> propertiesName,
            ICollection<TProperty> targetProperties, bool clearFirst = false)
            where TProperty : IReadOnlyProperty
        {
            if (clearFirst)
            {
                targetProperties.Clear();
            }

            foreach (var propertyName in propertiesName)
            {
                if (properties.TryGetValue(propertyName, out var readOnlyProperty) == false)
                {
                    UnityEngine.Debug.LogError($"Property with name {propertyName} does not exist in the property manager.",
                        context: this);
                    continue;
                }

                var property = (TProperty)readOnlyProperty;
                targetProperties.Add(property);
            }
        }

        public virtual void GetPropertiesStrictly<TProperty>(IEnumerable<string> propertiesName,
            IDictionary<string, TProperty> targetProperties)
            where TProperty : IReadOnlyProperty
        {
            foreach (var propertyName in propertiesName)
            {
                if (properties.TryGetValue(propertyName, out var readOnlyProperty) == false)
                {
                    UnityEngine.Debug.LogError($"Property with name {propertyName} does not exist in the property manager.",
                        context: this);
                    continue;
                }

                var property = (TProperty)readOnlyProperty;
                targetProperties.Add(propertyName, property);
            }
        }

        /// <returns>是否成功获取到属性</returns>
        public virtual bool GetPropertyIfNull<TProperty>(string propertyName, ref TProperty property)
            where TProperty : IReadOnlyProperty
        {
            if (property != null)
            {
                return false;
            }

            if (properties.TryGetValue(propertyName, out var readOnlyProperty) == false)
            {
                UnityEngine.Debug.LogError($"Property with name {propertyName} does not exist in the property manager.");
                return false;
            }

            property = (TProperty)readOnlyProperty;
            return true;
        }

        public virtual TValue GetValue<TValue>(string propertyName)
        {
            GetPropertyStrictly(propertyName, out IProperty<TValue> property);
            return property.GetValue();
        }

        public virtual bool TryGetValue<TValue>(string propertyName, out TValue value)
        {
            if (properties.TryGetValue(propertyName, out var readOnlyProperty) == false)
            {
                value = default;
                return false;
            }

            var valueProperty = (IReadOnlyProperty<TValue>)readOnlyProperty;
            value = valueProperty.GetValue();
            return true;
        }

        public virtual TValue GetValueOrDefault<TValue>(string propertyName, TValue defaultValue)
        {
            if (properties.TryGetValue(propertyName, out var readOnlyProperty) == false)
            {
                return defaultValue;
            }

            var valueProperty = (IReadOnlyProperty<TValue>)readOnlyProperty;
            return valueProperty.GetValue();
        }

        public virtual void SetValue<TValue>(string propertyName, TValue value)
        {
            GetPropertyStrictly(propertyName, out IProperty<TValue> property);
            property.SetValue(value, initial: false);
        }
    }
}