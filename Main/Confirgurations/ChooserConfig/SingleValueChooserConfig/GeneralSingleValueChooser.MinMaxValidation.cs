#if UNITY_EDITOR
using System.Runtime.CompilerServices;
using VMFramework.Core;
using VMFramework.Core.Generic;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    public partial class GeneralSingleValueChooser<TWrapper, TItem> : IMinimumValueProvider, IMaximumValueProvider
    {
        private bool? isWrapperVectorType;
        private bool? isWrapperNonMaximumProviderValueType;
        private bool? isWrapperNonMinimumProviderValueType;

        bool IMaximumValueProvider.CanClampByMaximum => CanClampByMaximumValue();
        bool IMinimumValueProvider.CanClampByMinimum => CanClampByMinimumValue();

        void IMaximumValueProvider.ClampByMaximum(double maximum)
        {
            if (IsVector())
            {
                value = value.ClampMax(maximum);
            }
            else if (value is IMaximumValueProvider maximumValueProvider)
            {
                maximumValueProvider.ClampByMaximum(maximum);
            }
        }

        void IMinimumValueProvider.ClampByMinimum(double minimum)
        {
            if (IsVector())
            {
                value = value.ClampMin(minimum);
            }
            else if (value is IMinimumValueProvider minimumValueProvider)
            {
                minimumValueProvider.ClampByMinimum(minimum);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsVector()
        {
            isWrapperVectorType ??= typeof(TWrapper).IsVector() || typeof(TWrapper).IsNumber();
            return isWrapperVectorType.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsNonMaximumProviderValueType()
        {
            if (isWrapperNonMaximumProviderValueType == null)
            {
                if (typeof(TWrapper).IsValueType == false)
                {
                    isWrapperNonMaximumProviderValueType = false;
                }
                else
                {
                    isWrapperNonMaximumProviderValueType =
                        typeof(TWrapper).IsDerivedFrom(typeof(IMaximumValueProvider), false) == false;
                }
            }
            
            return isWrapperNonMaximumProviderValueType.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsNonMinimumProviderValueType()
        {
            if (isWrapperNonMinimumProviderValueType == null)
            {
                if (typeof(TWrapper).IsValueType == false)
                {
                    isWrapperNonMinimumProviderValueType = false;
                }
                else
                {
                    isWrapperNonMinimumProviderValueType =
                        typeof(TWrapper).IsDerivedFrom(typeof(IMinimumValueProvider), false) == false;
                }
            }
            
            return isWrapperNonMinimumProviderValueType.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CanClampByMaximumValue()
        {
            if (IsVector())
            {
                return true;
            }

            if (IsNonMaximumProviderValueType())
            {
                return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CanClampByMinimumValue()
        {
            if (IsVector())
            {
                return true;
            }

            if (IsNonMinimumProviderValueType())
            {
                return false;
            }
            
            return true;
        }
    }
}
#endif