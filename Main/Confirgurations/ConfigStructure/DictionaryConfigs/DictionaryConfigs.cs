using System.Collections.Generic;
using VMFramework.Core;
using Sirenix.OdinInspector;
using VMFramework.OdinExtensions;
using System.Collections;
using System.Linq;
#if UNITY_EDITOR
using UnityEngine;
using VMFramework.Core.Linq;
#endif

namespace VMFramework.Configuration
{
    [System.Serializable]
    [PreviewComposite]
    public sealed class DictionaryConfigs<TID, TConfig> : StructureConfigs<TConfig>, IDictionaryConfigs<TID, TConfig>, IReadOnlyCollection<KeyValuePair<TID, TConfig>>
        where TConfig : IConfig, IIDOwner<TID>
    {
        [ShowInInspector]
        [HideInEditorMode]
        private Dictionary<TID, TConfig> configsRuntime;

        #region Initialization

        protected override void OnInit()
        {
            configsRuntime = new();
            base.OnInit();
        }

        #endregion

        #region Add Config

        public override bool TryAddConfigRuntime(TConfig config)
        {
            return configsRuntime.TryAdd(config.id, config);
        }

        #endregion

        #region Remove Config

        public bool RemoveConfigEditor(TID id)
        {
            foreach (var config in configs)
            {
                if (config.id.Equals(id))
                {
                    configs.Remove(config);
                    return true;
                }
            }

            return false;
        }

        public bool RemoveConfigRuntime(TID id)
        {
            return configsRuntime.Remove(id);
        }

        #endregion

        #region Get Config

        public override IEnumerable<TConfig> GetAllConfigsRuntime()
        {
            return configsRuntime.Values;
        }

        public TConfig GetConfigEditor(TID id)
        {
            foreach (var config in configs)
            {
                if (config.id.Equals(id))
                {
                    return config;
                }
            }

            return default;
        }

        public TConfig GetConfigRuntime(TID id)
        {
            return configsRuntime.GetValueOrDefault(id);
        }

        #endregion

        #region Has Config

        public override bool HasConfigEditor(TConfig config)
        {
            if (config == null)
            {
                return false;
            }

            return GetConfigEditor(config.id) != null;
        }

        public override bool HasConfigRuntime(TConfig config)
        {
            if (config == null)
            {
                return false;
            }

            return GetConfigRuntime(config.id) != null;
        }

        #endregion

        public IReadOnlyDictionary<TID, TConfig> GetRuntimeDictionary()
        {
            return configsRuntime;
        }
        public int Count
        {
            get
            {
                if (InitDone)
                {
                    return configsRuntime.Count;
                }

                return configs.Count;
            }
        }

        public IEnumerator<KeyValuePair<TID, TConfig>> GetEnumerator()
        {
            if (InitDone)
            {
                return configsRuntime.GetEnumerator();
            }

            return configs.Select(config => new KeyValuePair<TID, TConfig>(config.id, config))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
#if UNITY_EDITOR
        protected override IEnumerable<ValidationResult> GetValidationResults(GUIContent label)
        {
            foreach (var result in base.GetValidationResults(label))
            {
                yield return result;
            }

            if (configs.IsAnyNull())
            {
                yield return new("All Configs must be non-null.", ValidateType.Error);
                yield break;
            }

            if (configs.Any(config => config.id == null || config.id is ""))
            {
                yield return new("All Configs must have a non-empty ID.", ValidateType.Error);
            }

            if (configs.Select(config => config.id).ContainsSame())
            {
                yield return new("All Configs must have unique IDs.", ValidateType.Error);
            }
        }
#endif
    }
}
