using System;
using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace VMFramework.Procedure
{
    internal sealed class InitializationDeadline : IDisposable
    {
        private readonly IInitializationActivity activity;
        private readonly CancellationTokenSource cancellation = new();
        private readonly PlayerLoopTimer timer;
        private double remainingSeconds;
        private long started;
        private bool active;
        public CancellationToken Token => cancellation.Token;
        public bool IsCancellationRequested => cancellation.IsCancellationRequested;

        public InitializationDeadline(TimeSpan limit, IInitializationActivity activity)
        {
            this.activity = activity;
            remainingSeconds = limit.TotalSeconds;
            timer = PlayerLoopTimer.Create(limit, false, DelayType.Realtime, PlayerLoopTiming.Update,
                CancellationToken.None, state => ((InitializationDeadline)state).Expire(), this);
            activity.Changed += ActivityChanged;
            ActivityChanged(activity.Active);
        }

        private void ActivityChanged(bool value)
        {
            if (active == value || cancellation.IsCancellationRequested) return;
            if (active)
            {
                remainingSeconds -= (Stopwatch.GetTimestamp() - started) / (double)Stopwatch.Frequency;
                timer.Stop();
            }
            active = value;
            if (remainingSeconds <= 0) { Expire(); return; }
            if (active)
            {
                started = Stopwatch.GetTimestamp();
                timer.Restart(TimeSpan.FromSeconds(remainingSeconds));
            }
        }

        private void Expire() => cancellation.Cancel();

        public void Dispose()
        {
            activity.Changed -= ActivityChanged;
            timer.Dispose();
            activity.Dispose();
            cancellation.Dispose();
        }
    }
}
