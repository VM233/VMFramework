#if UNITY_EDITOR
using System.Collections.Generic;
using VMFramework.Configuration;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.Editor.BatchProcessor
{
    public class BatchGameEditorToolbarButtonProcessor : IFuncProcessor<object, ToolbarButtonConfig>
    {
        public virtual void ProcessTarget(object target, ICollection<ToolbarButtonConfig> results)
        {
            results.Add(new(BatchProcessorNames.BATCH_PROCESS_PATH, () =>
            {
                BatchProcessorWindow.OpenWindow(target);
            }));
            results.Add(new(BatchProcessorNames.ADD_TO_BATCH_PROCESSOR_PATH, () =>
            {
                BatchProcessorWindow.AddToWindow(target);
            }));

            if (target is IGlobalSettingFile)
            {
                results.Add(new(BatchProcessorNames.ADD_ALL_GLOBAL_SETTINGS_FILE_TO_BATCH_PROCESSOR_PATH, () =>
                {
                    BatchProcessorWindow.AddToWindow(GlobalSettingFileEditorManager.GetGlobalSettings());
                }));
            }

            if (target is IGamePrefabsProvider gamePrefabsProvider)
            {
                results.Add(new(BatchProcessorNames.ADD_ALL_PREFABS_TO_BATCH_PROCESSOR_PATH, () =>
                {
                    var prefabs = new List<IGamePrefab>();
                    gamePrefabsProvider.GetGamePrefabs(prefabs);
                    BatchProcessorWindow.AddToWindow(prefabs);
                }));
            }
        }
    }
}
#endif