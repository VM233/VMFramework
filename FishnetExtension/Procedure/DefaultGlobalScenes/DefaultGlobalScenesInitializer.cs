#if FISHNET
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using FishNet;
using FishNet.Managing.Scened;
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

        private static UniTask OnAfterInitComplete(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var sceneNames = NetworkSetting.DefaultGlobalScenesGeneralSetting.sceneNames;

            if (sceneNames.IsNullOrEmpty())
            {
                UnityEngine.Debug.LogError($"No Default Global Scenes found");
                return UniTask.CompletedTask;
            }

            UnityEngine.Debug.Log("Loading Default Global Scenes : " + sceneNames.Join(", "));

            InstanceFinder.SceneManager.LoadGlobalScenes(new SceneLoadData(sceneNames));

            return UniTask.CompletedTask;
        }
    }
}

#endif
