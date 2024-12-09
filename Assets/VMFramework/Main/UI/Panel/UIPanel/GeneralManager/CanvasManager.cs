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
    public sealed class CanvasManager : ManagerBehaviour<CanvasManager>
    {
        private static UIPanelGeneralSetting Setting => UISetting.UIPanelGeneralSetting;
        
        [ShowInInspector]
        private static Transform canvasContainer;

        [ShowInInspector]
        private static readonly Dictionary<int, Canvas> canvasDict = new();

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();
            
            canvasContainer = ContainerTransform.Get(UISetting.UIPanelGeneralSetting.containerName);
        }

        public static Canvas GetCanvas(int sortingOrder)
        {
            if (canvasDict.TryGetValue(sortingOrder, out var canvas) == false)
            {
                var result = canvasContainer.CreateCanvas($"Canvas:{sortingOrder}");
                canvas = result.canvas;

                canvas.sortingOrder = sortingOrder;
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;

                result.canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                result.canvasScaler.referenceResolution = Setting.defaultReferenceResolution;
                result.canvasScaler.matchWidthOrHeight = Setting.defaultMatch;

                canvasDict[sortingOrder] = canvas;
            }

            return canvas;
        }
    }
}