using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.Profiling;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public static class GlobalSettingFileManager
    {
        public const string FileDiscoveryMarkerName = "VMFramework.GlobalSettings.DiscoverFiles";
        public const string LoaderDiscoveryMarkerName = "VMFramework.GlobalSettings.DiscoverLoaders";
        public const string LoaderSelectionMarkerName = "VMFramework.GlobalSettings.SelectLoader";
        public const string LoadBeginMarkerName = "VMFramework.GlobalSettings.Load.Begin";
        private static readonly ProfilerMarker FileDiscoveryMarker = new(FileDiscoveryMarkerName);
        private static readonly ProfilerMarker LoaderDiscoveryMarker = new(LoaderDiscoveryMarkerName);
        private static readonly ProfilerMarker LoaderSelectionMarker = new(LoaderSelectionMarkerName);
        private static readonly ProfilerMarker LoadBeginMarker = new(LoadBeginMarkerName);
        private static readonly Dictionary<Type, GlobalSettingFileInfo> typeInfos = new();
        
        public static IReadOnlyDictionary<Type, GlobalSettingFileConfigAttribute> TypeConfigs =>
            typeInfos.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.configAttribute);

        private static readonly Dictionary<Type, IGlobalSettingFileLoader> loaders = new();
        
        static GlobalSettingFileManager()
        {
            using var profile = FileDiscoveryMarker.Auto();
            foreach (var derivedType in typeof(IGlobalSettingFile).GetDerivedClasses(false, false)
                         .ExcludeAbstractAndInterface())
            {
                if (derivedType.TryGetAttribute(false, out GlobalSettingFileConfigAttribute config) == false)
                {
                    Debug.LogWarning(
                        $"{derivedType} does not have a {nameof(GlobalSettingFileConfigAttribute)}!");
                    continue;
                }

                var info = new GlobalSettingFileInfo()
                {
                    configAttribute = config,
                };

                typeInfos.Add(derivedType, info);
            }
        }

        private static void CollectLoaders()
        {
            using var profile = LoaderDiscoveryMarker.Auto();
            foreach (var loaderType in typeof(IGlobalSettingFileLoader).GetDerivedClasses(false, false)
                         .ExcludeAbstractAndInterface())
            {
                if (loaders.ContainsKey(loaderType))
                {
                    continue;
                }
                
                var loader = (IGlobalSettingFileLoader)Activator.CreateInstance(loaderType);
                loaders.Add(loaderType, loader);
            }
        }

        public static async UniTask<TGlobalSetting> LoadGlobalSettingFile<TGlobalSetting>()
        {
            if (typeInfos.TryGetValue(typeof(TGlobalSetting), out var info) == false)
            {
                Debug.LogError($"No global setting file found for type {typeof(TGlobalSetting)}!");
                return default;
            }
            
            CollectLoaders();

            var validLoader = SelectLoader(typeof(TGlobalSetting));
            UniTask<IGlobalSettingFile> loading;
            using (LoadBeginMarker.Auto())
                loading = validLoader.LoadGlobalSettingFile(typeof(TGlobalSetting), info.configAttribute);
            var globalSetting = await loading;

            return (TGlobalSetting)globalSetting;
        }
        private static IGlobalSettingFileLoader SelectLoader(Type globalSettingType)
        {
            using var profile = LoaderSelectionMarker.Auto();
            var validLoaders =
                new Dictionary<IGlobalSettingFileLoader, GlobalSettingFileLoaderTargetConfig>();

            foreach (var loader in loaders.Values)
            {
                var config = loader.CanLoad(globalSettingType);

                if (config.isTarget == false)
                {
                    continue;
                }
                
                validLoaders.Add(loader, config);
            }

            if (validLoaders.Count == 0)
            {
                throw new InvalidOperationException(
                    $"No {nameof(IGlobalSettingFileLoader)} found for " +
                    $"{nameof(GlobalSettingFile)} of Type : {globalSettingType}!");
            }

            return validLoaders.SelectMax(kvp => kvp.Value.priority).Key;
        }
    }
}
