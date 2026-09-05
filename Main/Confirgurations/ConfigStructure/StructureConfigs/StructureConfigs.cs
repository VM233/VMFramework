using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.GameLogicArchitecture;
#if UNITY_EDITOR
using VMFramework.OdinExtensions;
#endif

namespace VMFramework.Configuration
{
    [System.Serializable]
#if UNITY_EDITOR
    [TypeValidation]
#endif
    public abstract class StructureConfigs<TConfig> : BaseConfig, IStructureConfigs<TConfig>
#if UNITY_EDITOR
        , ITypeValidationProvider
#endif
        where TConfig : IConfig
    {
        [ListDrawerSettings(ShowFoldout = false)]
        [SerializeReference]
        protected List<TConfig> configs = new();

        public override void CheckSettings()
        {
            base.CheckSettings();

            configs.CheckSettings();
        }

        protected override void OnInit()
        {
            base.OnInit();

            configs.Init();

            foreach (var config in configs)
            {
                if (TryAddConfigRuntime(config) == false)
                {
                    UnityEngine.Debug.LogWarning($"Could not add config {config}");
                }
            }
        }

        public abstract bool HasConfigEditor(TConfig config);

        public abstract bool HasConfigRuntime(TConfig config);

        public IEnumerable<TConfig> GetAllConfigsEditor()
        {
            return configs;
        }

        public abstract IEnumerable<TConfig> GetAllConfigsRuntime();

        public bool TryAddConfigEditor(TConfig config)
        {
            if (HasConfigEditor(config) == false)
            {
                configs.Add(config);
                return true;
            }

            return false;
        }

        public abstract bool TryAddConfigRuntime(TConfig config);

        public override string ToString()
        {
            return configs.Select<TConfig, INameOwner>().Select(nameOwner => nameOwner.Name).Join(",");
        }
#if UNITY_EDITOR
        protected virtual IEnumerable<ValidationResult> GetValidationResults(GUIContent label)
        {
            var labelName = label?.text;

            if (configs.Count == 0)
            {
                yield return new($"{labelName} is lacking any configuration", ValidateType.Info);
            }

            foreach (var config in configs)
            {
                if (config == null)
                {
                    yield return new($"{labelName} configs contain null", ValidateType.Error);
                }
            }
        }

        IEnumerable<ValidationResult> ITypeValidationProvider.GetValidationResults(GUIContent label)
        {
            return GetValidationResults(label);
        }
#endif
    }
}
