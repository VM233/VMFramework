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
        private static bool playingFrame;
#if UNITY_EDITOR
        private static int editorFrame;

        static InitializationWorkSlice()
        {
            UnityEditor.EditorApplication.update += AdvanceEditorFrame;
            UnityEditor.AssemblyReloadEvents.beforeAssemblyReload += RetireEditorClock;
            UnityEditor.EditorApplication.quitting += RetireEditorClock;
        }

        private static void AdvanceEditorFrame() => editorFrame++;

        private static void RetireEditorClock()
        {
            UnityEditor.EditorApplication.update -= AdvanceEditorFrame;
            UnityEditor.AssemblyReloadEvents.beforeAssemblyReload -= RetireEditorClock;
            UnityEditor.EditorApplication.quitting -= RetireEditorClock;
        }
#endif

        public InitializationWorkSlice() => AdoptCurrentFrame();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetFrame() => frame = -1;

        private static void AdoptCurrentFrame()
        {
            bool playing = Application.isPlaying;
            int actualFrame = Time.frameCount;
#if UNITY_EDITOR
            if (!playing) actualFrame = editorFrame;
#endif
            if (frame == actualFrame && playingFrame == playing) return;
            frame = actualFrame;
            playingFrame = playing;
            started = Stopwatch.GetTimestamp();
        }

        public async UniTask YieldIfBudgetExpired(CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                AdoptCurrentFrame();
                if ((Stopwatch.GetTimestamp() - started) / (double)Stopwatch.Frequency < BudgetSeconds) return;
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken, cancelImmediately: true);
                    continue;
                }
#endif
                await UniTask.NextFrame(PlayerLoopTiming.Update, cancellationToken, cancelImmediately: true);
            }
        }
    }
}
