using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.GameEvents;
using VMFramework.OdinExtensions;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    public sealed class UIToggleOnEventTriggeredModifier : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [ListDrawerSettings(ShowFoldout = false)]
        [GamePrefabID(typeof(IGameEventConfig))]
        [DisallowDuplicateElements]
        public List<string> uiToggleGameEventIDs = new();

        [TitleGroup(ComponentNames.CONFIG)]
        [ListDrawerSettings(ShowFoldout = false)]
        [ProcedureID]
        [DisallowDuplicateElements]
        public List<string> activeProceduresID = new();

        private readonly List<IReadOnlyParameterlessGameEvent> subscribedEvents = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            foreach (var gameEventID in uiToggleGameEventIDs)
            {
                var gameEvent = GameEventManager.Instance.GetGameEventStrictly<IReadOnlyParameterlessGameEvent>(gameEventID);
                gameEvent.AddCallback(ToggleConditional, PriorityDefines.TINY);
                subscribedEvents.Add(gameEvent);
            }
        }

        protected override void OnDeinitialize()
        {
            base.OnDeinitialize();
            
            foreach (var gameEvent in subscribedEvents)
            {
                gameEvent.RemoveCallback(ToggleConditional);
            }
            subscribedEvents.Clear();
        }

        private void ToggleConditional()
        {
            if (activeProceduresID.Count > 0)
            {
                if (activeProceduresID.All(procedureID =>
                        ProcedureManager.Instance.CurrentProcedureIDs.Contains(procedureID) == false))
                {
                    return;
                }
            }

            Panel.Toggle();
        }
    }
}
