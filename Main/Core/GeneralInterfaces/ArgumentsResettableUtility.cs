using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class ArgumentsResettableUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ResetArguments(this IEnumerable<IArgumentsResettable> resettableObjects)
        {
            foreach (var resettableObject in resettableObjects)
            {
                resettableObject.ResetArguments();
            }
        } 
    }
}