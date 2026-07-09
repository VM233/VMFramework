using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace VMFramework.Parameters
{
    public static class ParameterSourceUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetObjectValue([DisallowNull] this IParameterSource source, out object value)
        {
            value = null;
            return source.TryGetObjectValue(ref value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetObjectValueOrDefault([DisallowNull] this IParameterSource source)
        {
            object value = null;
            source.TryGetObjectValue(ref value);
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetValue<TValue>([DisallowNull] this IParameterSource<TValue> source, out TValue value)
        {
            value = default;
            return source.TryGetValue(ref value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetValueOrDefault<TValue>([DisallowNull] this IParameterSource<TValue> source,
            TValue defaultValue = default)
        {
            TValue value = defaultValue;
            source.TryGetValue(ref value);
            return value;
        }
    }
}