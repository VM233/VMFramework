using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Profiling;
using UnityEngine.Scripting;
using VMFramework.Configuration;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture
{
    [GameInitializerRegister(VMFrameworkInitializationDoneProcedure.ID, ProcedureLoadingType.OnEnter)]
    [Preserve]
    public sealed class GlobalSettingsFileLoadingInitializer : IGameInitializer, IInitializationProgressSource
    {
        public const string LoadDispatchMarkerName = "VMFramework.GlobalSettings.Load.Dispatch";
        private static readonly ProfilerMarker LoadDispatchMarker = new(LoadDispatchMarkerName);
        private readonly List<IGlobalSetting> globalSettings = new();
        private FileProgress[] files = Array.Empty<FileProgress>();

        private sealed class FileProgress : IProgress<float>
        {
            public float Fraction { get; private set; }
            public void Report(float value) => Fraction = value;
        }

        public InitializationProgress Progress
        {
            get
            {
                if (files.Length == 0) return default;
                float sum = 0;
                foreach (var file in files) sum += file.Fraction;
                return new InitializationProgress(sum / files.Length);
            }
        }

        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.InitStart, OnInitStart, this, this));
            actions.Add(new(InitializationOrder.InitComplete, OnInitComplete, this));
        }

        private async UniTask OnInitStart(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var tasks = new List<UniTask>();
            
            globalSettings.Clear();
            files = Array.Empty<FileProgress>();
            await GlobalSettingFileManager.PrepareLoadersAsync(cancellationToken);
            globalSettings.AddRange(GlobalSettingCollector.Collect());
            files = new FileProgress[globalSettings.Count];
            for (int i = 0; i < files.Length; i++) files[i] = new FileProgress();

            for (int i = 0; i < globalSettings.Count; i++)
            {
                using (LoadDispatchMarker.Auto())
                    tasks.Add(globalSettings[i].LoadGlobalSettingFile(files[i], cancellationToken));
            }
            
            await UniTask.WhenAll(tasks);
            cancellationToken.ThrowIfCancellationRequested();
        }

        private async UniTask OnInitComplete(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var slice = new InitializationWorkSlice();
            foreach (var setting in globalSettings)
            {
                await slice.YieldIfBudgetExpired(cancellationToken);
                await ConfigurationValidation.CheckAsync(setting.GlobalSettingFile, cancellationToken);
            }
        }
    }
}
