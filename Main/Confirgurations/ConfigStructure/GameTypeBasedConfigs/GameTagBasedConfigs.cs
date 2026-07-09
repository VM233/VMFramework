using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Configuration
{
    public partial class GameTagBasedConfigs<TConfig> : StructureConfigs<TConfig>, IGameTagBasedConfigs<TConfig>
        where TConfig : IConfig, IGameTagsOwner
    {
        [ShowInInspector]
        [HideInEditorMode]
        private Dictionary<string, TConfig> configsRuntime;

        public override void CheckSettings()
        {
            base.CheckSettings();

            configsRuntime = new();
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
    }
}