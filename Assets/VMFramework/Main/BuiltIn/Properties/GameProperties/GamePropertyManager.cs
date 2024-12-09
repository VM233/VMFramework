using System;
using System.Collections.Generic;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Properties
{
    public static class GamePropertyManager
    {
        private static readonly Dictionary<Type, List<IGameProperty>> propertyConfigs = new();

        public static void Init()
        {
            propertyConfigs.Clear();

            foreach (var gameProperty in GamePrefabManager.GetAllGamePrefabs<IGameProperty>())
            {
                if (gameProperty.TargetType == null)
                {
                    Debugger.LogWarning($"{gameProperty} has no target type set.");
                    continue;
                }

                var gameProperties = propertyConfigs.GetValueOrAddNew(gameProperty.TargetType);
                
                gameProperties.Add(gameProperty);
            }
        }
        
        public static IReadOnlyList<IGameProperty> GetGameProperties(Type targetType)
        {
            var result = new List<IGameProperty>();

            if (propertyConfigs.Count == 0)
            {
                Debugger.LogWarning($"{nameof(propertyConfigs)} is not loaded");
                return result;
            }

            foreach (var (type, propertyConfig) in propertyConfigs)
            {
                if (targetType.IsDerivedFrom(type, true))
                {
                    result.AddRange(propertyConfig);
                }
            }

            return result;
        }
    }
}