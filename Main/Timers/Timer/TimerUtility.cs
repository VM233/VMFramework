using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.Timers
{
    public static class TimerUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RandomAddTimer([DisallowNull] this ITimer<double> timer, float minInterval,
            float maxInterval)
        {
            var delay = GlobalRandom.Default.Range(minInterval, maxInterval).ClampMin(0);
            TimerManager.Instance.Add(timer, delay);
        }
    }
}