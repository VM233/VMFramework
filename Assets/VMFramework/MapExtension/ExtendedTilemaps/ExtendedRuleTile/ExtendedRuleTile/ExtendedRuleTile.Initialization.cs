using System;
using System.Collections.Generic;
using VMFramework.Configuration;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;

namespace VMFramework.Maps
{
    public partial class ExtendedRuleTile : IInitializer
    {
        public override void CheckSettings()
        {
            base.CheckSettings();

            defaultSpriteConfig.CheckSettings();
            ruleSet.CheckSettings();
        }

        protected override void GetInitializationActions(ICollection<InitializationAction> actions)
        {
            base.GetInitializationActions(actions);
            
            actions.Add(new(InitializationOrder.PostInit, OnPostInit, this));
            actions.Add(new(InitializationOrder.InitComplete, OnInitComplete, this));
        }

        protected override void OnInit()
        {
            base.OnInit();

            if (hasParent)
            {
                parentRuleTile = GamePrefabManager.GetGamePrefabStrictly<ExtendedRuleTile>(parentRuleTileID);
            }
            
            defaultSpriteConfig.Init();
        }

        private void OnPostInit(Action onAction)
        {
            InitInheritance();
            onAction();
        }

        private void OnInitComplete(Action onAction)
        {
            runtimeRuleSet.Init();
            onAction();
        }
    }
}