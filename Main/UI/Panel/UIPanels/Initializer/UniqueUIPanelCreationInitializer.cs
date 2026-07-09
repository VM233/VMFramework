using System;
using System.Collections.Generic;
using UnityEngine.Scripting;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;

namespace VMFramework.UI 
{
    [GameInitializerRegister(GameInitializationDoneProcedure.ID, ProcedureLoadingType.OnEnter)]
    [Preserve]
    public sealed class UniqueUIPanelCreationInitializer : IGameInitializer
    {
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.InitComplete, OnInitComplete, this));
        }

        private static void OnInitComplete(Action onDone)
        {
            foreach (var config in GamePrefabManager.GetAllActiveGamePrefabs<IUIPanelConfig>())
            {
                if (config.IsUnique)
                {
                    GameItemManager.Instance.Get(config.id);
                }
            }
            
            onDone();
        }
    }
}
