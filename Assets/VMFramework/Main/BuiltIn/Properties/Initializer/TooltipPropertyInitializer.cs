using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;

namespace VMFramework.Properties
{
    [GameInitializerRegister(GameInitializationDoneProcedure.ID, ProcedureLoadingType.OnEnter)]
    [Preserve]
    public sealed class TooltipPropertyInitializer : IGameInitializer
    {
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.Init, OnInit, this));
        }

        private static void OnInit(Action onDone)
        {
            Debugger.Log("Initializing Tooltip Property");
            
            foreach (var gamePrefab in GamePrefabManager.GetAllGamePrefabs())
            {
                if (gamePrefab.GameItemType == null)
                {
                    continue;
                }

                if (gamePrefab.GameItemType.IsAbstract)
                {
                    continue;
                }

                foreach (var configRuntime in TooltipPropertyManager.GetTooltipPropertyConfigsRuntime(
                             gamePrefab.GameItemType))
                {
                    TooltipPropertyManager.AddTooltipPropertyConfigRuntime(gamePrefab.id, configRuntime);
                }
            }

            onDone();
        }
    }
}