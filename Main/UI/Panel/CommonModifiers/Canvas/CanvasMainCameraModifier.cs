using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Cameras;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class CanvasMainCameraModifier : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        [Required]
        public List<Canvas> canvasList = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            foreach (var canvas in canvasList)
            {
                canvas.worldCamera = CameraManager.Instance.MainCamera;
            }
        }
    }
}