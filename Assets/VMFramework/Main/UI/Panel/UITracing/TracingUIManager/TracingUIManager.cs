using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Cameras;
using VMFramework.Core;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public sealed partial class TracingUIManager : ManagerBehaviour<TracingUIManager>
    {
        [ShowInInspector]
        private static readonly Dictionary<ITracingPanelModifier, TracingInfo> allTracingInfos = new();

        private static readonly List<ITracingPanelModifier> tracingUIPanelsToRemove = new();

        [ShowInInspector]
        private new static Camera camera;
        
        #region Init

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();
            
            camera = CameraManager.MainCamera;
        }

        #endregion

        #region Update

        private void Update()
        {
            var mousePosition = Input.mousePosition.To2D();

            foreach (var (panel, info) in allTracingInfos)
            {
                Vector2 screenPos = info.config.tracingType switch
                {
                    TracingType.MousePosition => mousePosition,
                    TracingType.Transform => camera.WorldToScreenPoint(info.config.tracingTransform.position),
                    TracingType.WorldPosition => camera.WorldToScreenPoint(info.config.tracingWorldPosition),
                    _ => throw new ArgumentOutOfRangeException()
                };
                
                if (panel.TryUpdatePosition(screenPos) && info.config.hasMaxTracingCount)
                {
                    info.tracingCount++;
                }
            }

            foreach (var (panel, info) in allTracingInfos)
            {
                if (info.config.hasMaxTracingCount && info.tracingCount > info.config.maxTracingCount)
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
        public static void SetCamera(Camera camera)
        {
            TracingUIManager.camera = camera;
        }

        #endregion
    }
}