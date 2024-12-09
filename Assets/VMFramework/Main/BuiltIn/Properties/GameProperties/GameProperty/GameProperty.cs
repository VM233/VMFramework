using System;
using VMFramework.GameLogicArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Properties
{
    public abstract partial class GameProperty : LocalizedGamePrefab, IGameProperty
    {
        public override string IDSuffix => "property";

        public sealed override Type GameItemType => null;

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [ShowInInspector]
        public abstract Type TargetType { get; }

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [PreviewField(50, ObjectFieldAlignment.Center)]
        [Required]
        public Sprite icon;

        Sprite IGameProperty.icon => icon;

        public abstract string GetValueString(object target);

        public override void CheckSettings()
        {
            base.CheckSettings();

            if (icon == null)
            {
                Debugger.LogWarning($"{this} icon is not set.");
            }
        }
    }
}
