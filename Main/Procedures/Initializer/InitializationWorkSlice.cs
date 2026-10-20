using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace VMFramework.Procedure
{
    public sealed class InitializationWorkSlice
    {
        public const double BudgetSeconds = .010;
        private static int frame = -1;
        private static long started;

        public InitializationWorkSlice() => AdoptCurrentFrame();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetFrame() => frame = -1;

        private static void AdoptCurrentFrame()
        {
            if (frame == Time.frameCount) return;
            frame = Time.frameCount;
            started = Stopwatch.GetTimestamp();
        }

        public async UniTask YieldIfBudgetExpired(CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                AdoptCurrentFrame();
                if ((Stopwatch.GetTimestamp() - started) / (double)Stopwatch.Frequency < BudgetSeconds) return;
                await UniTask.NextFrame(PlayerLoopTiming.Update, cancellationToken, cancelImmediately: true);
            }
        }
    }
}
