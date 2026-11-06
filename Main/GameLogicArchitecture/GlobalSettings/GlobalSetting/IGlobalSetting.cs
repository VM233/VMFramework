using Cysharp.Threading.Tasks;
using VMFramework.Configuration;
using System;
using System.Threading;

namespace VMFramework.GameLogicArchitecture
{
    public partial interface IGlobalSetting : INameOwner, ICheckableConfig
    {
        public IGlobalSettingFile GlobalSettingFile { get; }
        
        public UniTask LoadGlobalSettingFile(IProgress<float> progress, CancellationToken cancellationToken);
    }
}
