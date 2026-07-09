using VMFramework.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    [RequireComponent(typeof(RectTransform))]
    public partial class UGUIPanel : UIPanel, IUGUIPanel
    {
        protected IUGUIPanelConfig UGUIPanelConfig => (IUGUIPanelConfig)GamePrefab;
        
        [field: Required]
        [field: ComponentRequired(typeof(RectTransform))]
        [field: SerializeField]
        public GameObject UIMainGameObject { get; private set; }

        [ShowInInspector]
        public RectTransform UIMainRectTransform { get; private set; }

        public RectTransform RectTransform { get; private set; }

        [ShowInInspector]
        public Canvas Canvas { get; private set; }

        public CanvasScaler CanvasScaler { get; private set; }

        protected override void OnCreate()
        {
            Canvas = CanvasManager.Instance.GetCanvas(Config.SortingOrder);

            CanvasScaler = Canvas.GetComponent<CanvasScaler>();

            transform.SetParent(Canvas.transform);

            transform.ResetLocalArguments();

            RectTransform = gameObject.GetComponent<RectTransform>();

            RectTransform.anchorMin = Vector2.zero;
            RectTransform.anchorMax = Vector2.one;
            RectTransform.offsetMin = Vector2.zero;
            RectTransform.offsetMax = Vector2.zero;

            UIMainRectTransform = UIMainGameObject.GetComponent<RectTransform>();
            
            base.OnCreate();
        }
    }
}
