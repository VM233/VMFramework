using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using VMFramework.Procedure;
using VMFramework.Procedure.Editor;

namespace VMFramework.Tests
{
    public sealed class BuildBoundaryPendingInitializer : IEditorInitializer
    {
        internal static bool Enabled;
        internal static bool Entered;
        internal static UniTaskCompletionSource Pending;

        public bool EnableInitializationDebugLog => false;

        public void GetInitializationActions(ICollection<InitializationAction> actions)
        {
            if (Enabled) actions.Add(new InitializationAction(int.MinValue, WaitForBuild, this));
        }

        private static async UniTask WaitForBuild(CancellationToken cancellationToken)
        {
            Entered = true;
            await Pending.Task.AttachExternalCancellation(cancellationToken);
        }
    }
}
