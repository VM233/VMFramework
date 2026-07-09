#if UNITY_EDITOR
using Cysharp.Threading.Tasks;
using UnityEngine;
using VMFramework.Core.Editor;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GlobalSetting<TGlobalSetting, TGlobalSettingFile>
    {
        public UniTask LoadGlobalSettingFileInEditor()
        {
            if (GlobalSettingFileEditorManager.TryGetGlobalSettingPath(typeof(TGlobalSettingFile),
                    out string path, out string fileName) == false)
            {
                return new UniTask();
            }

            var file = path.GetAssetByPath<TGlobalSettingFile>();

            if (file == null)
            {
                Debug.LogError($"{typeof(TGlobalSettingFile)} Load Failed.");
                return new UniTask();
            }
            
            GlobalSettingFile = file;
            
            return new UniTask();
        }
    }
}
#endif