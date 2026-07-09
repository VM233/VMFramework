using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture
{
    public static class GlobalSettingCollector
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IGlobalSetting> Collect()
        {
            foreach (var manager in ManagerBehaviourCollector.Collect())
            {
                if (manager is IGlobalSetting globalSetting)
                {
                    yield return globalSetting;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IGeneralSetting> GetAllGeneralSettings()
        {
            foreach (var globalSetting in Collect())
            {
                if (globalSetting.GlobalSettingFile == null)
                {
                    Debug.LogWarning($"GlobalSetting {globalSetting.Name} has no GlobalSettingFile assigned.");
                    continue;
                }

                foreach (var generalSetting in globalSetting.GlobalSettingFile.GetAllGeneralSettings())
                {
                    yield return generalSetting;
                }
            }
        }
    }
}