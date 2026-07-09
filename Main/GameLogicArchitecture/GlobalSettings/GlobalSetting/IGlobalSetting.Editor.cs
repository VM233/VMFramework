#if UNITY_EDITOR
using Cysharp.Threading.Tasks;

namespace VMFramework.GameLogicArchitecture
{
    public partial interface IGlobalSetting
    {
        public UniTask LoadGlobalSettingFileInEditor();
    }
}
#endif