#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using VMFramework.Procedure;
using VMFramework.Procedure.Editor;

namespace VMFramework.GameLogicArchitecture.Editor
{
    internal sealed class GlobalSettingEditorInitializer : IEditorInitializer
    {
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.BeforeInitStart, OnBeforeInitStart, this));
            actions.Add(new(InitializationOrder.InitStart, OnInitStart, this));
        }

        private static UniTask OnBeforeInitStart(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            GlobalSettingFileEditorManager.CheckGlobalSettingsFile();
            return UniTask.CompletedTask;
        }

        private static async UniTask OnInitStart(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var tasks = new List<UniTask>();
            
            foreach (var manager in ManagerCreator.Managers)
            {
                if (manager is IGlobalSetting globalSetting)
                {
                    tasks.Add(globalSetting.LoadGlobalSettingFileInEditor());
                }
            }

            await UniTask.WhenAll(tasks).AttachExternalCancellation(cancellationToken);

            foreach (var globalSettingFile in GlobalSettingFileEditorManager.GetGlobalSettings())
            {
                cancellationToken.ThrowIfCancellationRequested();
                globalSettingFile.AutoFindSettings();
            }
        }
    }
}
#endif
