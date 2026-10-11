using System;
using UnityEngine;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Tests
{
    [ExecuteAlways]
    public sealed class ControllerCleanupTestItem : ControllerGameItem
    {
        public Action CreateAction, GetAction, ReturnAction, ClearAction, NativeDestroyedAction;
        public int Created, Returned, Cleared, NativeDestroyed;

        protected override void OnCreate()
        {
            base.OnCreate();
            Created++;
            CreateAction?.Invoke();
        }

        protected override void OnGet()
        {
            base.OnGet();
            GetAction?.Invoke();
        }

        protected override void OnReturn()
        {
            Returned++;
            base.OnReturn();
            ReturnAction?.Invoke();
        }

        protected override void OnClear()
        {
            Cleared++;
            var action = ClearAction;
            UnityEngine.Object.DestroyImmediate(gameObject);
            action?.Invoke();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            NativeDestroyed++;
            NativeDestroyedAction?.Invoke();
        }
    }
}
