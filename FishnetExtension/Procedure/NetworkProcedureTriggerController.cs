#if FISHNET

using VMFramework.Core;

namespace VMFramework.Procedure
{
    [ManagerCreationProvider(ManagerType.NetworkCore)]
    public sealed class NetworkProcedureTriggerController : NetworkManagerBehaviour<NetworkProcedureTriggerController>
    {
        public override void OnStartServer()
        {
            base.OnStartServer();
            
            UnityEngine.Debug.Log("The server has started");
            ProcedureManager.Instance.EnterProcedure(MainMenuProcedure.ID, ServerRunningProcedure.ID);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            UnityEngine.Debug.Log("The client has started");

            if (ProcedureManager.Instance.HasCurrentProcedure(MainMenuProcedure.ID))
            {
                ProcedureManager.Instance.EnterProcedure(MainMenuProcedure.ID, ClientRunningProcedure.ID);
            }
            else
            {
                ProcedureManager.Instance.EnterProcedure(ClientRunningProcedure.ID);
            }
        }

        public override void OnStopClient()
        {
            base.OnStopClient();

            if (IsServerStarted == false)
            {
                ProcedureManager.Instance.EnterProcedure(ClientRunningProcedure.ID, MainMenuProcedure.ID);
            }
            else
            {
                ProcedureManager.Instance.ExitProcedureImmediately(ClientRunningProcedure.ID);
            }
        }

        public override void OnStopServer()
        {
            base.OnStopServer();

            if (IsClientStarted == false)
            {
                ProcedureManager.Instance.EnterProcedure(ServerRunningProcedure.ID, MainMenuProcedure.ID);
            }
            else
            {
                ProcedureManager.Instance.ExitProcedureImmediately(ServerRunningProcedure.ID);
            }
        }
    }
}

#endif
