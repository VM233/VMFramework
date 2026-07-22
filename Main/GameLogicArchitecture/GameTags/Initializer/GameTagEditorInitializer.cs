#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VMFramework.Procedure;
using VMFramework.Procedure.Editor;

namespace VMFramework.GameLogicArchitecture.Editor
{
    internal sealed class GameTagEditorInitializer : IEditorInitializer
    {
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.PreInit, OnPreInit, this));
        }

        private static UniTask OnPreInit(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (CoreSetting.GameTagGeneralSetting == null)
            {
                Debug.LogError($"{nameof(GameTagGeneralSetting)} is not set. Please set it in the {nameof(CoreSetting)}.");
                return UniTask.CompletedTask;
            }
            
            CoreSetting.GameTagGeneralSetting.InitGameTags();
            return UniTask.CompletedTask;
        }
    }
}
#endif
