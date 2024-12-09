#if FISHNET
using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Managing.Scened;
using UnityEngine;
using UnityEngine.Scripting;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.Network;

namespace VMFramework.Procedure
{
    [GameInitializerRegister(ServerRunningProcedure.ID, ProcedureLoadingType.OnEnter)]
    [Preserve]
    public sealed class DefaultGlobalScenesInitializer : IGameInitializer
    {
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            if (NetworkSetting.DefaultGlobalScenesGeneralSetting.enableDefaultGlobalScenesLoader)
            {
                actions.Add(new(InitializationOrder.AfterInitComplete, OnAfterInitComplete, this));
            }
        }

        private static void OnAfterInitComplete(Action onDone)
        {
            var sceneNames = NetworkSetting.DefaultGlobalScenesGeneralSetting.sceneNames;

            if (sceneNames.IsNullOrEmpty())
            {
                Debugger.LogError($"No Default Global Scenes found");
                onDone();
                return;
            }

            Debugger.Log("Loading Default Global Scenes : " + sceneNames.Join(", "));

            InstanceFinder.SceneManager.LoadGlobalScenes(new SceneLoadData(sceneNames));

            onDone();
        }
    }
}

#endif