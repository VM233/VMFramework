using System.Runtime.CompilerServices;

namespace VMFramework.Properties
{
    public static class PropertyUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ToggleValue<TProperty>(this TProperty property, bool initial)
            where TProperty : IValueProperty<bool>
        {
            property.SetValue(!property.GetValue(), initial);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetValue<TValue>(this IReadOnlyProperty property)
        {
            var valuedProperty = (IReadOnlyProperty<TValue>)property;
            return valuedProperty.GetValue();
        }
    }
}