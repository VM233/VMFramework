using System;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace VMFramework.GameLogicArchitecture
{
    [UnityEngine.Scripting.RequireImplementors]
    public interface IGlobalSettingFileLoader
    {
        public GlobalSettingFileLoaderTargetConfig CanLoad(Type globalSettingType);
        
        public UniTask<IGlobalSettingFile> LoadGlobalSettingFile(Type globalSettingType,
            GlobalSettingFileConfigAttribute config, IProgress<float> progress, CancellationToken cancellationToken);
    }
}
