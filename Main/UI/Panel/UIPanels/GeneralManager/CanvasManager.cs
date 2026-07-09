using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using VMFramework.Core;
using VMFramework.Procedure;
using VMFramework.Tools;

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public class CanvasManager : ManagerBehaviour<CanvasManager>
    {
        private static UIPanelGeneralSetting Setting => UISetting.UIPanelGeneralSetting;
        
        [ShowInInspector]
        public Transform CanvasContainer { get; private set; }

        [ShowInInspector]
        private readonly Dictionary<int, Canvas> canvasDict = new();

        protected override void Awake()
        {
            base.Awake();
            
            canvasDict.Clear();
        }

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();
            
            CanvasContainer = ContainerTransform.Get(UISetting.UIPanelGeneralSetting.containerName);
        }

        public virtual Canvas GetCanvas(int sortingOrder)
        {
            if (canvasDict.TryGetValue(sortingOrder, out var canvas))
            {
                return canvas;
            }
            
            var result = CanvasContainer.CreateCanvas($"Canvas:{sortingOrder}");
            canvas = result.canvas;

            canvas.sortingOrder = sortingOrder;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            result.canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            result.canvasScaler.referenceResolution = Setting.panelSettings.referenceResolution;
            result.canvasScaler.matchWidthOrHeight = Setting.panelSettings.match;

            canvasDict[sortingOrder] = canvas;

            return canvas;
        }
    }
}