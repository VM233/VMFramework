using System;
using Cysharp.Threading.Tasks;
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
            GlobalSettingFileConfigAttribute config)
        {
            var handler = Addressables.LoadAssetAsync<IGlobalSettingFile>(config.FileName);
            await UniTask.WaitUntil(() => handler.IsDone);
            return handler.Result;
        }
    }
}