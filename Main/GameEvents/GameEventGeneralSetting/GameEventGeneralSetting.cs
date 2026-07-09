using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.GameEvents
{
    public sealed partial class GameEventGeneralSetting : GamePrefabGeneralSetting
    {
        #region Meta Data

        public const string DEPENDENCY_CATEGORY = "Dependency";

        public override Type BaseGamePrefabType => typeof(GameEventConfig);

        #endregion

        [TabGroup(TAB_GROUP_NAME, DEPENDENCY_CATEGORY)]
        public List<GameEventDependencyNode> dependencyNodes = new();

        [TabGroup(TAB_GROUP_NAME, DEPENDENCY_CATEGORY)]
        [NonSerialized]
        [ShowInInspector]
        public Dictionary<string, HashSet<string>> directDependencies;

        public bool autoEnableActions = true;

        #region Check & Init

        public override void CheckSettings()
        {
            base.CheckSettings();

            dependencyNodes.CheckSettings();
        }

        protected override void OnInit()
        {
            base.OnInit();

            dependencyNodes.Init();

            directDependencies = new();

            foreach (var node in dependencyNodes.LevelOrderTraverse(true, node => node.children))
            {
                if (node.children.IsNullOrEmpty())
                {
                    continue;
                }

                var dependencies = directDependencies.GetOrCreate(node.gameEventID);

                foreach (var child in node.children)
                {
                    dependencies.Add(child.gameEventID);
                }
            }

            if (autoEnableActions)
            {
                InputSystem.actions.Enable();
            }
        }

        #endregion
    }
}