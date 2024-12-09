using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using VMFramework.Core;

namespace VMFramework.GameEvents
{
    public class FloatInputGameEvent : InputGameEvent<float>, IFloatInputGameEvent, IUpdateableGameEvent
    {
        protected FloatInputGameEventConfig FloatInputGameEventConfig => (FloatInputGameEventConfig)GamePrefab;

        [ShowInInspector]
        public bool isReversed { get; private set; }
        
        [ShowInInspector]
        public bool isFromAxis { get; private set; }

        [ShowInInspector]
        public InputAxisType inputAxisType { get; private set; }

        [ShowInInspector]
        private List<InputActionGroupRuntime> positiveActionGroups;

        [ShowInInspector]
        private List<InputActionGroupRuntime> negativeActionGroups;

        [ShowInInspector]
        public float value { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();

            isFromAxis = FloatInputGameEventConfig.isFromAxis;
            inputAxisType = FloatInputGameEventConfig.inputAxisType;

            positiveActionGroups = FloatInputGameEventConfig.positiveActionGroups.ToRuntime().ToList();
            negativeActionGroups = FloatInputGameEventConfig.negativeActionGroups.ToRuntime().ToList();
        }

        public override IEnumerable<string> GetInputMappingContent(KeyCodeToStringMode mode)
        {
            yield return "";
        }

        void IUpdateableGameEvent.Update()
        {
            if (isFromAxis)
            {
                value = inputAxisType.GetAxisValue();
            }
            else
            {
                value = 0;

                if (positiveActionGroups.Check())
                {
                    value += 1;
                }

                if (negativeActionGroups.Check())
                {
                    value -= 1;
                }
            }

            if (value != 0)
            {
                if (isReversed)
                {
                    value = -value;
                }
                
                Propagate(value);
            }
        }
    }
}