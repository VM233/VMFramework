using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using VMFramework.Core;
using UnityEngine;
using VMFramework.Core.Linq;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;
using System.Collections;

namespace VMFramework.Configuration
{
    [Serializable]
    [PreviewComposite]
#if UNITY_EDITOR
    [TypeValidation]
#endif
    public class ListConfigs<TConfig> : BaseConfig, IEnumerable<TConfig>
#if UNITY_EDITOR
        , ITypeValidationProvider
#endif
        where TConfig : BaseConfig, INameOwner, IListConfig<TConfig>
    {
        [LabelText("Configurations")]
#if UNITY_EDITOR
        [ListDrawerSettings(DefaultExpandedState = true, ShowFoldout = false)]
        [OnCollectionChanged(nameof(OnConfigsCollectionChanged))]
#endif
        [SerializeReference]
        [IsNotNullOrEmpty]
        private List<TConfig> configs = new();

        public int count => configs.Count;

        public int maxIndex => configs.Count - 1;

        #region Init & CheckSettings

        public override void CheckSettings()
        {
            base.CheckSettings();

            foreach (var config in configs)
            {
                config.CheckSettings();
            }
        }

        protected override void OnInit()
        {
            base.OnInit();

            foreach (var (index, config) in configs.Enumerate())
            {
                config.index = index;
                config.listConfigs = this;
            }

            foreach (var config in configs)
            {
                config.Init();
            }
        }

        #endregion

        #region Get Configs

        public TConfig GetConfig(int index)
        {
            if (index < 0 || index >= configs.Count)
            {
                throw new ArgumentOutOfRangeException(
                    $"Index {index} is outside the configuration range [0, {configs.Count - 1}].");
            }

            return configs[index];
        }

        public bool TryGetConfig(int index, out TConfig config)
        {
            if (index < 0 || index >= configs.Count)
            {
                config = null;
                return false;
            }

            config = configs[index];
            return true;
        }

        public IEnumerable<TConfig> GetAllConfigs()
        {
            return configs;
        }

        public IEnumerable<TConfig> GetRangeConfigs(int start, int end)
        {
            return configs.GetRange(new RangeInteger(start, end));
        }

        #endregion

        #region To String

        public override string ToString()
        {
            return configs.Select(config => config.Name).Join(",");
        }

        #endregion

        #region Indexer

        public TConfig this[int index] => GetConfig(index);

        #endregion
#if UNITY_EDITOR
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();


            OnConfigsCollectionChanged();
        }

        private void OnConfigsCollectionChanged()
        {
            foreach (var (index, config) in configs.Enumerate())
            {
                config.index = index;
                config.listConfigs = this;
            }
        }

        public IEnumerable<ValueDropdownItem<int>> GetNameList()
        {
            return configs.Select((config, index) =>
                new ValueDropdownItem<int>(config.Name, index));
        }
#endif
        public IEnumerator<TConfig> GetEnumerator()
        {
            return configs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
#if UNITY_EDITOR
        public IEnumerable<ValidationResult> GetValidationResults(GUIContent label)
        {
            if (configs.Count == 0)
            {
                var labelName = label?.text;
                yield return new ($"{labelName} has no configurations.", ValidateType.Warning);
            }
        }
#endif
    }
}
