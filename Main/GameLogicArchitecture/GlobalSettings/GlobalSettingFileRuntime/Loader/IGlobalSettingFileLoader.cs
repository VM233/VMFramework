using System;
using Cysharp.Threading.Tasks;

namespace VMFramework.GameLogicArchitecture
{
    public interface IGlobalSettingFileLoader
    {
        public GlobalSettingFileLoaderTargetConfig CanLoad(Type globalSettingType);
        
        public UniTask<IGlobalSettingFile> LoadGlobalSettingFile(Type globalSettingType,
            GlobalSettingFileConfigAttribute config);
    }
}