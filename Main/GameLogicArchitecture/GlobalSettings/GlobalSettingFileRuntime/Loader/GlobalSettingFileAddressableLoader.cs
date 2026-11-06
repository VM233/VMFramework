using System;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.AddressableAssets;

namespace VMFramework.GameLogicArchitecture
{
    public sealed class GlobalSettingFileAddressableLoader : IGlobalSettingFileLoader
    {
        GlobalSettingFileLoaderTargetConfig IGlobalSettingFileLoader.CanLoad(Type globalSettingType)
        {
            return new GlobalSettingFileLoaderTargetConfig(true, 0);
        }

        async UniTask<IGlobalSettingFile> IGlobalSettingFileLoader.LoadGlobalSettingFile(Type globalSettingType,
            GlobalSettingFileConfigAttribute config, IProgress<float> progress, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var handler = Addressables.LoadAssetAsync<IGlobalSettingFile>(config.FileName);
            try
            {
                return await handler.ToUniTask(progress, cancellationToken: cancellationToken);
            }
            catch
            {
                Addressables.Release(handler);
                throw;
            }
        }
    }
}
