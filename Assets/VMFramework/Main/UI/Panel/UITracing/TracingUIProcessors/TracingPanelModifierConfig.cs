using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public abstract class TracingPanelModifierConfig : PanelModifierConfig
    {
        protected const string TRACING_UI_SETTING_CATEGORY = "Tracing UI";
        
        [SuffixLabel("Left bottom corner is (0, 0)"),
         TabGroup(TAB_GROUP_NAME, TRACING_UI_SETTING_CATEGORY, SdfIconType.Mouse, TextColor = "purple")]
        [MinValue(0), MaxValue(1)]
        [JsonProperty]
        public Vector2 defaultPivot = new(0, 1);

        [TabGroup(TAB_GROUP_NAME, TRACING_UI_SETTING_CATEGORY)]
        [JsonProperty]
        public bool enableScreenOverflow;

        [TabGroup(TAB_GROUP_NAME, TRACING_UI_SETTING_CATEGORY)]
        [HideIf(nameof(enableScreenOverflow))]
        [JsonProperty]
        public bool autoPivotCorrection = true;

        [TabGroup(TAB_GROUP_NAME, TRACING_UI_SETTING_CATEGORY)]
        [JsonProperty]
        public bool enableAutoMouseTracing = false;

        [TabGroup(TAB_GROUP_NAME, TRACING_UI_SETTING_CATEGORY)]
        [ToggleButtons("Persistent Tracing", "Single-Shot Tracing")]
        [JsonProperty]
        public bool persistentTracing = true;
    }
}