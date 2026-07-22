#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using VMFramework.Procedure;
using VMFramework.Procedure.Editor;

namespace VMFramework.Editor.GameEditor
{
    internal sealed class GameEditorInitializer : IEditorInitializer
    {
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.InitComplete, OnInitComplete, this));
        }

        private static async UniTask OnInitComplete(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (Application.isPlaying)
            {
                return;
            }

            if (EditorWindow.HasOpenInstances<GameEditor>() == false)
            {
                return;
            }

            var gameEditor = EditorWindow.GetWindow<GameEditor>();

            if (gameEditor == null)
            {
                return;
            }

            await Refresh(gameEditor, cancellationToken);
        }

        private static async UniTask Refresh(GameEditor gameEditor, CancellationToken cancellationToken)
        {
            await UniTask.Delay(500, cancellationToken: cancellationToken);

            gameEditor.Repaint();
            gameEditor.ForceMenuTreeRebuild();

            await UniTask.Delay(1000, cancellationToken: cancellationToken);

            gameEditor.Repaint();
            gameEditor.ForceMenuTreeRebuild();
        }
    }
}

#endif
