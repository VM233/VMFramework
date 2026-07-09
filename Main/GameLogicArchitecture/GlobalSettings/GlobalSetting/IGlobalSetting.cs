using Cysharp.Threading.Tasks;
using VMFramework.Configuration;

namespace VMFramework.GameLogicArchitecture
{
    public partial interface IGlobalSetting : INameOwner, ICheckableConfig
    {
        public IGlobalSettingFile GlobalSettingFile { get; }
        
        public UniTask LoadGlobalSettingFile();
    }
}