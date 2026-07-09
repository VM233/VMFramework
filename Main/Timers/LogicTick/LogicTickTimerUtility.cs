using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.Timers
{
    public static class LogicTickTimerUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetTimerEnabled([DisallowNull] this ITimer<ulong> timer, bool enabled, int minInterval,
            int maxInterval)
        {
            LogicTickTimerManager.Instance.TryStop(timer);
            if (enabled)
            {
                timer.RandomAddTimer(minInterval, maxInterval);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetTimerEnabled([DisallowNull] this ITimer<ulong> timer, bool enabled, uint delay)
        {
            if (enabled)
            {
                LogicTickTimerManager.Instance.Add(timer, delay);
            }
            else
            {
                LogicTickTimerManager.Instance.TryStop(timer);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RandomAddTimer([DisallowNull] this ITimer<ulong> timer, int minInterval, int maxInterval)
        {
            var delay = GlobalRandom.Default.Range(minInterval, maxInterval).ClampMin(0);
            LogicTickTimerManager.Instance.Add(timer, (uint)delay);
        }
    }
}