#if UNITY_EDITOR
using System;
using System.Collections.Generic;
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

        private static void OnBeforeInitStart(Action onDone)
        {
            GlobalSettingFileEditorManager.CheckGlobalSettingsFile();
            
            onDone();
        }

        private static async void OnInitStart(Action onDone)
        {
            var tasks = new List<UniTask>();
            
            foreach (var manager in ManagerCreator.Managers)
            {
                if (manager is IGlobalSetting globalSetting)
                {
                    tasks.Add(globalSetting.LoadGlobalSettingFileInEditor());
                }
            }

            await UniTask.WhenAll(tasks);

            foreach (var globalSettingFile in GlobalSettingFileEditorManager.GetGlobalSettings())
            {
                globalSettingFile.AutoFindSettings();
            }
            
            onDone();
        }
    }
}
#endif