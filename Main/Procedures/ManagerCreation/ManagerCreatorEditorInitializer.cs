#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using VMFramework.Procedure.Editor;

namespace VMFramework.Procedure
{
    internal sealed class ManagerCreatorEditorInitializer : IEditorInitializer
    {
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.BeforeInitStart, OnBeforeInitStart, this));
        }
        
        private static UniTask OnBeforeInitStart(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ManagerCreator.CreateManagers();
            return UniTask.CompletedTask;
        }
    }
}
#endif
