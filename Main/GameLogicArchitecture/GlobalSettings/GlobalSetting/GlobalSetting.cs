using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Procedure;
using Object = UnityEngine.Object;

namespace VMFramework.GameLogicArchitecture
{
    public abstract partial class GlobalSetting<TGlobalSetting, TGlobalSettingFile>
        : ManagerBehaviour<TGlobalSetting>, IGlobalSetting
        where TGlobalSetting : GlobalSetting<TGlobalSetting, TGlobalSettingFile>
        where TGlobalSettingFile : Object, IGlobalSettingFile
    {
        [ShowInInspector]
        public static TGlobalSettingFile GlobalSettingFile { get; private set; }

        public async UniTask LoadGlobalSettingFile(IProgress<float> progress, CancellationToken cancellationToken)
        {
            var file = await GlobalSettingFileManager.LoadGlobalSettingFile<TGlobalSettingFile>(progress, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            GlobalSettingFile = file != null ? file :
                throw new InvalidOperationException($"Global setting loader returned no {typeof(TGlobalSettingFile)}.");
            progress.Report(1);
        }

        public void CheckSettings()
        {
            GlobalSettingFile.CheckSettings();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertGlobalSettingFileIsLoaded()
        {
            if (GlobalSettingFile == null)
            {
                throw new Exception($"{typeof(TGlobalSetting)} is not loaded!");
            }
        }

        IGlobalSettingFile IGlobalSetting.GlobalSettingFile => GlobalSettingFile;

        string INameOwner.Name => name;
    }
}
