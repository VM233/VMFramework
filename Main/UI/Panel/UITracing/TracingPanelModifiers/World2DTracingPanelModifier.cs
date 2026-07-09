using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Cameras;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class World2DTracingPanelModifier : TracingPanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        public List<Transform> tracingContainers = new();

        protected override void OnOpen(IUIPanel panel)
        {
            base.OnOpen(panel);

            foreach (var container in tracingContainers)
            {
                container.SetActive(true);
            }
        }

        protected override void OnPostClose(IUIPanel panel)
        {
            base.OnPostClose(panel);

            foreach (var container in tracingContainers)
            {
                container.SetActive(false);
            }
        }

        public override bool TryUpdateScreenPosition(Vector2 screenPosition)
        {
            var camera = CameraManager.Instance.MainCamera;

            if (camera == null)
            {
                return false;
            }

            if (enableScreenOverflow == false)
            {
                Vector2 screenSize = new(Screen.width, Screen.height);
                screenPosition = screenPosition.Clamp(screenSize);
            }

            var worldPosition = camera.ScreenToWorldPoint(screenPosition.As3DXY());

            foreach (var tracingContainer in tracingContainers)
            {
                tracingContainer.position = worldPosition;
            }
            
            return true;
        }

        public override void SetPivot(Vector2 pivot)
        {
            
        }
    }
}