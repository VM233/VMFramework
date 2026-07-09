#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VMFramework.Core.Editor;
using VMFramework.GameLogicArchitecture;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class GlobalSettingFilesAutoGroupUnit : SingleButtonBatchProcessorUnit
    {
        protected override string ProcessButtonName => "Auto Group Global Setting Files";
        
        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.Any(obj =>
                obj is IGlobalSettingFile && ((Object)obj).IsAddressableAsset() == false);
        }

        protected override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            foreach (var obj in selectedObjects)
            {
                if (obj is not IGlobalSettingFile globalSettingFile)
                {
                    continue;
                }
                
                globalSettingFile.AutoAddressableGroup();
            }
            
            return selectedObjects;
        }
    }
}
#endif