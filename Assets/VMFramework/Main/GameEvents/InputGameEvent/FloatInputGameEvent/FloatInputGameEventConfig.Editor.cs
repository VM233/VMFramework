#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.GameEvents
{
    public partial class FloatInputGameEventConfig
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            positiveActionGroups ??= new();
            negativeActionGroups ??= new();
        }

        [Button(ButtonSizes.Medium), TabGroup(TAB_GROUP_NAME, INPUT_MAPPING_CATEGORY)]
        private void QuickADSetup()
        {
            isFromAxis = false;

            positiveActionGroups.Clear();
            positiveActionGroups.Add(new(KeyCode.D, KeyBoardTriggerType.IsPressing));
            positiveActionGroups.Add(new(KeyCode.RightArrow, KeyBoardTriggerType.IsPressing));

            negativeActionGroups.Clear();
            negativeActionGroups.Add(new(KeyCode.A, KeyBoardTriggerType.IsPressing));
            negativeActionGroups.Add(new(KeyCode.LeftArrow, KeyBoardTriggerType.IsPressing));
        }
    }
}
#endif