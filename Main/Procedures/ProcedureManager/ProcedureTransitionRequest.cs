using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace VMFramework.Procedure
{
    internal sealed class ProcedureTransitionRequest
    {
        private readonly TaskCompletionSource<bool> completion = new();
        public string FromID { get; }
        public string ToID { get; }
        public Task Completion => completion.Task;
        public ProcedureTransitionRequest(string fromID, string toID)
        { FromID = fromID; ToID = toID; }
        public void CancelQueued(CancellationToken token) => completion.SetCanceled(token);
        public async Task RunAsync(Func<CancellationToken, UniTask> transition, Action retireSource, CancellationToken token)
        {
            Exception failure = null;
            try
            {
                token.ThrowIfCancellationRequested();
                await transition(token);
            }
            catch (Exception error) { failure = error; }
            retireSource();
            if (failure is OperationCanceledException cancellation && token.IsCancellationRequested &&
                cancellation.CancellationToken == token) completion.SetCanceled(token);
            else if (failure != null) completion.SetException(failure);
            else completion.SetResult(true);
        }
    }
}
