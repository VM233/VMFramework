#if UNITY_EDITOR
using UnityEngine;
using VMFramework.Configuration;

namespace VMFramework.Maps
{
    public partial class ExtendedRuleTile
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            ruleSet ??= new();

            defaultSpriteConfig ??= new();
        }

        private Color GetRuleModeColor()
        {
            return ruleMode switch
            {
                RuleMode.Inheritance => new(1, 1, 0.5f, 1),
                _ => Color.white
            };
        }

        private Rule AddRuleToRuleSetGUI()
        {
            return new Rule()
            {
                
            };
        }
    }
}
#endif