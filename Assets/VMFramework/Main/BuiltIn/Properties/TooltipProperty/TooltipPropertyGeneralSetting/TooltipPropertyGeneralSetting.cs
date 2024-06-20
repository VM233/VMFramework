﻿using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Properties
{
    public sealed partial class TooltipPropertyGeneralSetting : GeneralSetting
    {
        #region Categories

        public const string PROPERTY_TOOLTIP_CATEGORY = "Tooltip Property";

        #endregion

        [TabGroup(TAB_GROUP_NAME, PROPERTY_TOOLTIP_CATEGORY)]
        [SerializeField, JsonProperty]
        public DictionaryConfigs<Type, TooltipPropertyConfig> tooltipPropertyConfigs = new();

        #region Init & Check

        protected override void OnInit()
        {
            base.OnInit();
            
            tooltipPropertyConfigs.Init();
        }

        public override void CheckSettings()
        {
            base.CheckSettings();
            
            tooltipPropertyConfigs.CheckSettings();
        }

        #endregion
    }
}