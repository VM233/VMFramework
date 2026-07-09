using System.Collections.Generic;
using UnityEngine.Scripting;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture
{
    [GameInitializerRegister(GameInitializationDoneProcedure.ID, ProcedureLoadingType.OnEnter)]
    [Preserve]
    public sealed class GeneralSettingsInitializer : IGameInitializer
    {
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            foreach (var globalSetting in GlobalSettingCollector.Collect())
            {
                globalSetting.GlobalSettingFile.GetInitializationActions(actions);
            }
        }
    }
}