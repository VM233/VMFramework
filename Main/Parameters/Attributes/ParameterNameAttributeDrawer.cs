#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Parameters
{
    public class ParameterNameAttributeDrawer : GeneralValueDropdownAttributeDrawer<ParameterNameAttribute>
    {
        private ValueResolver<Type> componentTypeGetter;

        protected override void Initialize()
        {
            componentTypeGetter = ValueResolver.Get<Type>(Property, Attribute.TypeGetter);

            base.Initialize();
        }

        protected override IEnumerable<ValueDropdownItem> GetValues()
        {
            if (Attribute.IsCollection)
            {
                foreach (var parent in Property.TraverseToRoot(true, property => property.Parent))
                {
                    ICollectionParameterProvider parameterProvider = null;

                    foreach (var parentValue in parent.ParentValues)
                    {
                        if (parentValue is ICollectionParameterProviderOwner parameterProviderOwner)
                        {
                            parameterProvider = parameterProviderOwner.CollectionParameterProvider;
                        }
                    }

                    if (parameterProvider == null)
                    {
                        continue;
                    }

                    var targetType = componentTypeGetter.GetValue();

                    if (parameterProvider is ICollectionParametersInfoProvider parametersInfoProvider)
                    {
                        foreach (var (name, type) in parametersInfoProvider.GetCollectionParametersInfo())
                        {
                            if (TryGetDropdownItem(name, type, targetType, out var dropdownItem))
                            {
                                yield return dropdownItem;
                            }
                        }
                    }
                    else
                    {
                        foreach (var attribute in parameterProvider.GetType().GetAttributes(includingInherit: true))
                        {
                            if (attribute is ParameterDefineAttribute parameterDefineAttribute)
                            {
                                if (parameterDefineAttribute.IsCollection == false)
                                {
                                    continue;
                                }

                                if (TryGetDropdownItem(parameterDefineAttribute.Name, parameterDefineAttribute.Type,
                                        targetType, out var dropdownItem))
                                {
                                    yield return dropdownItem;
                                }
                            }
                        }
                    }

                    yield break;
                }
            }
            else
            {
                foreach (var parent in Property.TraverseToRoot(true, property => property.Parent))
                {
                    IParameterProvider parameterProvider = null;

                    foreach (var parentValue in parent.ParentValues)
                    {
                        if (parentValue is IParameterProviderOwner parameterProviderOwner)
                        {
                            parameterProvider = parameterProviderOwner.ParameterProvider;
                        }
                    }

                    if (parameterProvider == null)
                    {
                        continue;
                    }

                    var targetType = componentTypeGetter.GetValue();

                    if (parameterProvider is IParametersInfoProvider parametersInfoProvider)
                    {
                        foreach (var (name, type) in parametersInfoProvider.GetParametersInfo())
                        {
                            if (TryGetDropdownItem(name, type, targetType, out var dropdownItem))
                            {
                                yield return dropdownItem;
                            }
                        }
                    }
                    else
                    {
                        foreach (var attribute in parameterProvider.GetType().GetAttributes(includingInherit: true))
                        {
                            if (attribute is ParameterDefineAttribute parameterDefineAttribute)
                            {
                                if (parameterDefineAttribute.IsCollection)
                                {
                                    continue;
                                }

                                if (TryGetDropdownItem(parameterDefineAttribute.Name, parameterDefineAttribute.Type,
                                        targetType, out var dropdownItem))
                                {
                                    yield return dropdownItem;
                                }
                            }
                        }
                    }

                    yield break;
                }
            }
        }

        protected virtual bool TryGetDropdownItem(string parameterName, Type parameterType, Type targetType,
            out ValueDropdownItem dropdownItem)
        {
            if (targetType.IsValueType)
            {
                if (targetType == parameterType)
                {
                    dropdownItem = new ValueDropdownItem(parameterName, parameterName);
                    return true;
                }
            }
            else
            {
                if (parameterType.IsDerivedFrom(targetType, includingSelf: true))
                {
                    dropdownItem = new ValueDropdownItem(parameterName, parameterName);
                    return true;
                }
            }

            dropdownItem = default;
            return false;
        }
    }
}
#endif