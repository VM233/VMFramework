using System;
using System.Collections.Generic;
using UnityEngine.Scripting;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture 
{
    [GameInitializerRegister(GameInitializationDoneProcedure.ID, ProcedureLoadingType.OnEnter)]
    [Preserve]
    public sealed class GameItemPrewarmInitializer : IGameInitializer
    {
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.InitComplete, OnInitComplete, this));
        }

        private static void OnInitComplete(Action onDone)
        {
            foreach (var gamePrefab in GamePrefabManager.GetAllGamePrefabs())
            {
                if (gamePrefab.GameItemPrewarmCount <= 0)
                {
                    continue;
                }
                
                GameItemManager.Instance.PrewarmUntil(gamePrefab.id, gamePrefab.GameItemPrewarmCount);
            }
            
            onDone();
        }
    }
}