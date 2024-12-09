using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using VMFramework.Core;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture
{
    [GameInitializerRegister(VMFrameworkInitializationDoneProcedure.ID, ProcedureLoadingType.OnEnter)]
    [Preserve]
    public sealed class GamePrefabRegisterInitializer : IGameInitializer
    {
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.PostInit, OnPostInit, this));
            actions.Add(new(InitializationOrder.InitComplete, OnInitComplete, this));
        }

        private static async void OnPostInit(Action onDone)
        {
            GamePrefabManager.Clear();

            var gamePrefabs = await GamePrefabCollectorManager.Collect();

            foreach (var gamePrefab in gamePrefabs)
            {
                GamePrefabManager.RegisterGamePrefab(gamePrefab);
            }

            onDone();
        }
        
        private static void OnInitComplete(Action onDone)
        {
            foreach (var gamePrefab in GamePrefabManager.GetAllGamePrefabs())
            {
                Debugger.Log($"Checking {gamePrefab}");
                
                gamePrefab.CheckSettings();
            }
            
            onDone();
        }
    }
}