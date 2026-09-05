using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using VMFramework.GameLogicArchitecture;
#if UNITY_EDITOR
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.OdinExtensions;
#endif

namespace VMFramework.Configuration
{
    [System.Serializable]
    public class GameTagBasedConfigs<TConfig> : StructureConfigs<TConfig>, IGameTagBasedConfigs<TConfig>
        where TConfig : IConfig, IGameTagsOwner
    {
        [ShowInInspector]
        [HideInEditorMode]
        private Dictionary<string, TConfig> configsRuntime;

        protected override void OnInit()
        {
            configsRuntime = new();
            base.OnInit();
        }

        #region Add Config

        public override bool TryAddConfigRuntime(TConfig config)
        {
            bool success = true;

            foreach (var gameTag in config.GameTags)
            {
                if (GameTag.HasTag(gameTag) == false)
                {
                    continue;
                }

                if (configsRuntime.TryAdd(gameTag, config) == false)
                {
                    success = false;
                }
            }

            return success;
        }

        #endregion

        #region Get Config

        public TConfig GetConfigEditor(string id)
        {
            if (GameTag.HasTagWithWarning(id) == false)
            {
                return default;
            }

            foreach (var config in configs)
            {
                if (config.GameTags.Any(gameTagID => gameTagID == id))
                {
                    return config;
                }
            }

            return default;
        }

        public TConfig GetConfigRuntime(string id)
        {
            if (GameTag.HasTagWithWarning(id) == false)
            {
                return default;
            }

            return configsRuntime.GetValueOrDefault(id);
        }

        #endregion

        #region Remove Config

        public bool RemoveConfigEditor(string id)
        {
            if (GameTag.HasTagWithWarning(id) == false)
            {
                return false;
            }

            foreach (var config in configs.ToArray())
            {
                if (config.GameTags.Any(gameTagID => gameTagID == id))
                {
                    configs.Remove(config);
                }
            }

            return true;
        }

        public bool RemoveConfigRuntime(string id)
        {
            if (GameTag.HasTagWithWarning(id) == false)
            {
                return false;
            }

            configsRuntime.Remove(id);

            return true;
        }

        #endregion

        #region Has Config

        public override bool HasConfigEditor(TConfig config)
        {
            if (config == null)
            {
                return false;
            }

            return config.GameTags.All(gameTagID => GetConfigEditor(gameTagID) != null);
        }

        public override bool HasConfigRuntime(TConfig config)
        {
            if (config == null)
            {
                return false;
            }

            return config.GameTags.All(gameTagID => GetConfigRuntime(gameTagID) != null);
        }

        #endregion

        public override IEnumerable<TConfig> GetAllConfigsRuntime()
        {
            return configsRuntime.Values;
        }
#if UNITY_EDITOR
        protected override IEnumerable<ValidationResult> GetValidationResults(GUIContent label)
        {
            foreach (var result in base.GetValidationResults(label))
            {
                yield return result;
            }

            if (configs.Count == 0)
            {
                yield break;
            }

            if (configs.Any(config => config.GameTags == null))
            {
                yield return new($"There is null GameTags Collections in the configurations.", ValidateType.Error);
                yield break;
            }

            if (configs.Any(config => config.GameTags.Any() == false))
            {
                yield return new($"Each configuration must have at least one {nameof(GameTag)} ID assigned.",
                    ValidateType.Error);
            }

            if (configs.Any(config => config.GameTags.Any(id => id.IsNullOrEmpty())))
            {
                yield return new($"Each {nameof(GameTag)} ID must be a valid string.", ValidateType.Error);
            }

            if (configs.Select(config => config.GameTags.AsEnumerable()).Aggregate((one, two) => one.Concat(two))
                    .IsUnique() == false)
            {
                yield return new("Each game type ID must be unique across all configurations.",
                    ValidateType.Error);
            }
        }
#endif
    }
}
