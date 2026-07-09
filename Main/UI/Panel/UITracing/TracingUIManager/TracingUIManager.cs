using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Cameras;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.Procedure;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public class TracingUIManager : ManagerBehaviour<TracingUIManager>
    {
        [ShowInInspector]
        private readonly Dictionary<ITracingPanelModifier, TracingInfo> allTracingInfos = new();

        private readonly List<ITracingPanelModifier> tracingUIPanelsToRemove = new();

        [ShowInInspector]
        private Camera mainCamera;

        #region Init

        protected override void Awake()
        {
            base.Awake();

            allTracingInfos.Clear();
        }

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            mainCamera = CameraManager.Instance.MainCamera;
        }

        #endregion

        #region Query

        public bool TryGetConfig(ITracingPanelModifier tracingUIPanel, out TracingConfig config)
        {
            if (allTracingInfos.TryGetValue(tracingUIPanel, out var info))
            {
                config = info.Config;
                return true;
            }

            config = default;
            return false;
        }

        #endregion

        #region Update

        protected virtual void Update()
        {
#if ENABLE_INPUT_SYSTEM
            var mousePosition = Mouse.current.position.ReadValue();
#else
            var mousePosition = Input.mousePosition.To2D();
#endif

            foreach (var (panel, info) in allTracingInfos)
            {
                Vector2 screenPos = info.Config.tracingType switch
                {
                    TracingType.MousePosition => mousePosition,
                    TracingType.Transform => mainCamera.WorldToScreenPoint(
                        position: info.Config.tracingTransform.position + info.Config.tracingOffset),
                    TracingType.WorldPosition => mainCamera.WorldToScreenPoint(info.Config.tracingWorldPosition),
                    _ => throw new ArgumentOutOfRangeException()
                };

                if (panel.TryUpdateScreenPosition(screenPos) && info.Config.hasMaxTracingCount)
                {
                    info.tracingCount++;
                }
            }

            foreach (var (panel, info) in allTracingInfos)
            {
                if (info.Config.hasMaxTracingCount && info.tracingCount >= info.Config.maxTracingCount)
                {
                    tracingUIPanelsToRemove.Add(panel);
                }
            }

            if (tracingUIPanelsToRemove.Count > 0)
            {
                foreach (var tracingUIPanel in tracingUIPanelsToRemove)
                {
                    StopTracing(tracingUIPanel);
                }

                tracingUIPanelsToRemove.Clear();
            }
        }

        #endregion

        #region Set Camera

        [Button]
        public void SetCamera(Camera camera)
        {
            mainCamera = camera;
        }

        #endregion

        #region Start Stop

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StartTracing(ITracingPanelModifier tracingUIPanel, TracingConfig tracingConfig)
        {
            StopTracing(tracingUIPanel);

            var info = SimplePool<TracingInfo>.Get();

            info.Initialize(tracingConfig);

            allTracingInfos.Add(tracingUIPanel, info);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool StopTracing(ITracingPanelModifier tracingUIPanel)
        {
            if (allTracingInfos.Remove(tracingUIPanel, out var info))
            {
                SimplePool<TracingInfo>.Return(info);
                return true;
            }

            return false;
        }

        #endregion

        #region Open

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IUIPanel OpenOn(string panelID, TracingConfig config, out ITracingPanelModifier tracingProcessor)
        {
            var panel = UIPanelManager.Instance.GetAndOpen(panelID);

            if (panel.Modifiers.TryGetTarget(out tracingProcessor) == false)
            {
                return null;
            }

            StartTracing(tracingProcessor, config);
            return panel;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TPanel OpenOn<TPanel>(string panelID, TracingConfig config, out ITracingPanelModifier tracingProcessor)
            where TPanel : IUIPanel
        {
            var panel = UIPanelManager.Instance.GetAndOpen<TPanel>(panelID);

            if (panel.Modifiers.TryGetTarget(out tracingProcessor) == false)
            {
                return default;
            }

            StartTracing(tracingProcessor, config);
            return panel;
        }

        #endregion
    }
}