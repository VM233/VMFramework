using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;
#if UNITY_EDITOR
using VMFramework.Editor.GameEditor;
#endif

namespace VMFramework.UI
{
    public sealed class UIPanelProcedureGeneralSetting : GeneralSetting
#if UNITY_EDITOR
        , IGameEditorMenuTreeNode
#endif
    {
        private const string PROCEDURE_CATEGORY = "Procedures";

#if UNITY_EDITOR
        string INameOwner.Name => "UI Procedure";
#endif

        [TabGroup(TAB_GROUP_NAME, PROCEDURE_CATEGORY)]
        [SerializeReference]
        public List<UIPanelProcedureConfig> procedureConfigs = new();

        public override void CheckSettings()
        {
            base.CheckSettings();

            procedureConfigs.CheckUniqueIDs(nameof(procedureConfigs));
            procedureConfigs.CheckSettings();
        }

        protected override void OnInit()
        {
            base.OnInit();

            procedureConfigs.CheckUniqueIDs(nameof(procedureConfigs));
            procedureConfigs.Init();
            ProcedureManager.Instance.OnEnterProcedureEvent += OnEnterProcedure;
            ProcedureManager.Instance.OnExitProcedureEvent += OnExitProcedure;
        }

        public bool TryGetProcedureConfig(string procedureID, out UIPanelProcedureConfig config)
        {
            foreach (var candidate in procedureConfigs)
            {
                if (candidate.procedureID == procedureID)
                {
                    config = candidate;
                    return true;
                }
            }

            config = null;
            return false;
        }

        private void OnEnterProcedure(string procedureID)
        {
            if (TryGetProcedureConfig(procedureID, out var config) == false)
            {
                return;
            }

            if (config.uiPanelAutoCloseOnEnter != null)
            {
                foreach (var uiPanelID in config.uiPanelAutoCloseOnEnter)
                {
                    if (UIPanelManager.Instance.TryGetOpenedPanels(uiPanelID, out var uiPanels))
                    {
                        var openedUIPanels = ListPool<IUIPanel>.Default.Get();
                        openedUIPanels.Clear();
                        openedUIPanels.AddRange(uiPanels);

                        foreach (var uiPanelController in openedUIPanels)
                        {
                            uiPanelController.Close();
                        }

                        openedUIPanels.ReturnToDefaultPool();
                    }
                }
            }

            if (config.uniqueUIPanelAutoOpenOnEnter != null)
            {
                foreach (var uiPanelID in config.uniqueUIPanelAutoOpenOnEnter)
                {
                    if (UIPanelManager.Instance.TryGetUniquePanelWithWarning(uiPanelID, out var panel))
                    {
                        panel.Open(null);
                    }
                }
            }
        }

        private void OnExitProcedure(string procedureID)
        {
            if (TryGetProcedureConfig(procedureID, out var config) == false)
            {
                return;
            }

            if (config.uiPanelAutoCloseOnExit != null)
            {
                foreach (var uiPanelID in config.uiPanelAutoCloseOnExit)
                {
                    if (UIPanelManager.Instance.TryGetOpenedPanels(uiPanelID, out var uiPanels))
                    {
                        var openedUIPanels = ListPool<IUIPanel>.Default.Get();
                        openedUIPanels.Clear();
                        openedUIPanels.AddRange(uiPanels);

                        foreach (var uiPanelController in openedUIPanels)
                        {
                            uiPanelController.Close();
                        }

                        openedUIPanels.ReturnToDefaultPool();
                    }
                }
            }

            if (config.uniqueUIPanelAutoOpenOnExit != null)
            {
                foreach (var uiPanelID in config.uniqueUIPanelAutoOpenOnExit)
                {
                    if (UIPanelManager.Instance.TryGetUniquePanelWithWarning(uiPanelID, out var panel))
                    {
                        panel.Open(null);
                    }
                }
            }
        }
    }
}
