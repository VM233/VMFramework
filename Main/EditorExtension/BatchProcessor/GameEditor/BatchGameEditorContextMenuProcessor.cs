#if UNITY_EDITOR
using System.Collections.Generic;
using VMFramework.Configuration;
using VMFramework.Core.Linq;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.Editor.BatchProcessor
{
    public class BatchGameEditorContextMenuProcessor : IFuncTargetsProcessor<object, ContextMenuItemConfig>
    {
        public virtual void ProcessTargets(IReadOnlyCollection<object> targets,
            ICollection<ContextMenuItemConfig> results)
        {
            results.Add(
                new(BatchProcessorNames.BATCH_PROCESS_NAME, () => { BatchProcessorWindow.OpenWindow(targets); }));
            results.Add(new(BatchProcessorNames.ADD_TO_BATCH_PROCESSOR_NAME,
                () => { BatchProcessorWindow.AddToWindow(targets); }));

            if (targets.AnyIs<IGlobalSettingFile>())
            {
                results.Add(new(BatchProcessorNames.ADD_ALL_GLOBAL_SETTINGS_FILE_TO_BATCH_PROCESSOR_NAME,
                    () => { BatchProcessorWindow.AddToWindow(GlobalSettingFileEditorManager.GetGlobalSettings()); }));
            }

            if (targets.AnyIs<IGamePrefabsProvider>())
            {
                results.Add(new(BatchProcessorNames.ADD_ALL_PREFABS_TO_BATCH_PROCESSOR_NAME, () =>
                {
                    var prefabs = new List<IGamePrefab>();
                    foreach (var target in targets)
                    {
                        if (target is IGamePrefabsProvider prefabProvider)
                        {
                            prefabProvider.GetGamePrefabs(prefabs);
                        }
                    }
                    
                    BatchProcessorWindow.AddToWindow(prefabs);
                }));
            }
        }
    }
}
#endif