using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.GameEvents
{
    public class Vector2InputGameEvent : InputGameEvent<Vector2>, IVector2InputGameEvent, IUpdateableGameEvent
    {
        protected Vector2InputGameEventConfig Vector2InputGameEventConfig => (Vector2InputGameEventConfig)GamePrefab;

        [ShowInInspector]
        public bool isXFromAxis { get; private set; }

        [ShowInInspector]
        public InputAxisType xInputAxisType { get; private set; }

        [ShowInInspector]
        private List<InputActionGroupRuntime> xPositiveActionGroups;

        [ShowInInspector]
        private List<InputActionGroupRuntime> xNegativeActionGroups;

        [ShowInInspector]
        public bool isYFromAxis { get; private set; }

        [ShowInInspector]
        public InputAxisType yInputAxisType { get; private set; }

        [ShowInInspector]
        private List<InputActionGroupRuntime> yPositiveActionGroups;

        [ShowInInspector]
        private List<InputActionGroupRuntime> yNegativeActionGroups;

        [ShowInInspector]
        public Vector2 value { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();

            isXFromAxis = Vector2InputGameEventConfig.isXFromAxis;
            xInputAxisType = Vector2InputGameEventConfig.xInputAxisType;
            xPositiveActionGroups = Vector2InputGameEventConfig.xPositiveActionGroups.ToRuntime().ToList();
            xNegativeActionGroups = Vector2InputGameEventConfig.xNegativeActionGroups.ToRuntime().ToList();

            isYFromAxis = Vector2InputGameEventConfig.isYFromAxis;
            yInputAxisType = Vector2InputGameEventConfig.yInputAxisType;
            yPositiveActionGroups = Vector2InputGameEventConfig.yPositiveActionGroups.ToRuntime().ToList();
            yNegativeActionGroups = Vector2InputGameEventConfig.yNegativeActionGroups.ToRuntime().ToList();
        }

        public override IEnumerable<string> GetInputMappingContent(KeyCodeToStringMode mode)
        {
            yield return "";
        }

        void IUpdateableGameEvent.Update()
        {
            var vector = Vector2.zero;

            if (isXFromAxis)
            {
                vector.x = xInputAxisType.GetAxisValue();
            }
            else
            {
                if (xPositiveActionGroups.Check())
                {
                    vector.x += 1;
                }

                if (xNegativeActionGroups.Check())
                {
                    vector.x -= 1;
                }
            }

            if (isYFromAxis)
            {
                vector.y = yInputAxisType.GetAxisValue();
            }
            else
            {
                if (yPositiveActionGroups.Check())
                {
                    vector.y += 1;
                }

                if (yNegativeActionGroups.Check())
                {
                    vector.y -= 1;
                }
            }

            if (Vector2InputGameEventConfig.requireMouseInScreen)
            {
                vector = vector.ClampMaxMagnitude(1);
            }

            value = vector;

            if (value != Vector2.zero)
            {
                Propagate(value);
            }
        }
    }
}