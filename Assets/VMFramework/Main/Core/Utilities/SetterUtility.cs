using System;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class SetterUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue SetClassValue<TValue, TOwner>(this TOwner owner, TValue oldValue, TValue newValue,
            out bool result, string valueName)
            where TValue : class
        {
            if (oldValue == null)
            {
                if (newValue == null)
                {
                    Debugger.LogWarning($"The {valueName} of {owner} has already been set to null." +
                                        "Cannot set it to null again.");
                    result = false;
                    return null;
                }

                result = true;
                return newValue;
            }

            if (newValue == null)
            {
                result = true;
                return null;
            }
                
            Debugger.LogWarning($"The {valueName} of {owner} has already been set to {oldValue} and cannot be changed." +
                                $"If you want to change the {valueName}, please set the {valueName} to null first.");
            result = false;
            return oldValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue SetStructValue<TValue, TOwner>(this TOwner owner, TValue oldValue, TValue newValue,
            TValue emptyValue, out bool result, string valueName)
            where TValue : struct, IEquatable<TValue>
        {
            if (oldValue.Equals(emptyValue))
            {
                if (newValue.Equals(emptyValue))
                {
                    Debugger.LogWarning(
                        $"The {valueName} of {owner} has already been set to the empty value({emptyValue})." +
                        "Cannot set it to null again.");
                    result = false;
                    return emptyValue;
                }

                result = true;
                return newValue;
            }

            if (newValue.Equals(emptyValue))
            {
                result = true;
                return emptyValue;
            }
                
            Debugger.LogWarning($"The {valueName} of {owner} has already been set to {oldValue} " +
                                $"and cannot be changed to {newValue}." +
                                $"If you want to change the {valueName}, " +
                                $"please set the {valueName} to the empty value({emptyValue}) first.");
            result = false;
            return oldValue;
        }
    }
}