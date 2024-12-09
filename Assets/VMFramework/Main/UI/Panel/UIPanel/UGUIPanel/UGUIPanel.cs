using System;
using VMFramework.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace VMFramework.UI
{
    public class UGUIPanel : UIPanel, IUGUIPanel
    {
        protected IUGUIPanelConfig UGUIPanelConfig => (IUGUIPanelConfig)GamePrefab;
        
        [ShowInInspector]
        protected GameObject VisualObject { get; private set; }

        [ShowInInspector]
        public RectTransform VisualRectTransform { get; private set; }

        protected RectTransform RectTransform { get; private set; }

        [ShowInInspector]
        public Canvas Canvas { get; private set; }

        public CanvasScaler CanvasScaler { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            
            Canvas = CanvasManager.GetCanvas(Config.SortingOrder);

            CanvasScaler = Canvas.GetComponent<CanvasScaler>();

            transform.SetParent(Canvas.transform);

            transform.ResetLocalArguments();

            RectTransform = gameObject.GetOrAddComponent<RectTransform>();

            RectTransform.anchorMin = Vector2.zero;
            RectTransform.anchorMax = Vector2.one;
            RectTransform.offsetMin = Vector2.zero;
            RectTransform.offsetMax = Vector2.zero;

            VisualObject = Instantiate(UGUIPanelConfig.UGUIAsset, transform);

            VisualObject.AssertIsNotNull(nameof(VisualObject));

            VisualRectTransform = VisualObject.GetComponent<RectTransform>();

            VisualRectTransform.AssertIsNotNull(nameof(VisualRectTransform));
        }
    }
}
