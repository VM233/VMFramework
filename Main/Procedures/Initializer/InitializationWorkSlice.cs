using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace VMFramework.Procedure
{
    public sealed class InitializationWorkSlice
    {
        public const double BudgetSeconds = .004;
        private long started = Stopwatch.GetTimestamp();

        public async UniTask YieldIfBudgetExpired(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((Stopwatch.GetTimestamp() - started) / (double)Stopwatch.Frequency < BudgetSeconds) return;
            await UniTask.NextFrame(PlayerLoopTiming.Update, cancellationToken, cancelImmediately: true);
            started = Stopwatch.GetTimestamp();
        }
    }
}
