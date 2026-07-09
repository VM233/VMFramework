using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public static class GlobalSettingFileManager
    {
        private static readonly Dictionary<Type, GlobalSettingFileInfo> typeInfos = new();
        
        public static IReadOnlyDictionary<Type, GlobalSettingFileConfigAttribute> TypeConfigs =>
            typeInfos.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.configAttribute);

        private static readonly Dictionary<Type, IGlobalSettingFileLoader> loaders = new();
        
        static GlobalSettingFileManager()
        {
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

            var validLoaders =
                new Dictionary<IGlobalSettingFileLoader, GlobalSettingFileLoaderTargetConfig>();

            foreach (var loader in loaders.Values)
            {
                var config = loader.CanLoad(typeof(TGlobalSetting));

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
                    $"{nameof(GlobalSettingFile)} of Type : {typeof(TGlobalSetting)}!");
            }

            var validLoader = validLoaders.SelectMax(kvp => kvp.Value.priority).Key;

            var globalSetting =
                await validLoader.LoadGlobalSettingFile(typeof(TGlobalSetting), info.configAttribute);
            
            return (TGlobalSetting)globalSetting;
        }
    }
}