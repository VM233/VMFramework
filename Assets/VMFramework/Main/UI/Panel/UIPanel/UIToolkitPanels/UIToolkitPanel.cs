using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;

namespace VMFramework.UI
{
    [DisallowMultipleComponent]
    public partial class UIToolkitPanel : UIPanel, IUIToolkitPanel
    {
        public UIDocument UIDocument { get; private set; }

        protected IUIToolkitPanelConfig UIToolkitPanelConfig => (IUIToolkitPanelConfig)GamePrefab;

        public VisualElement RootVisualElement { get; private set; }

        protected CancellationTokenSource OpenCTS { get; private set; }
        
        public VisualElement UIMainPart { get; private set; }

        #region Pool Events

        protected override void OnCreate()
        {
            base.OnCreate();
            
            var uiDocument = this.GetOrAddComponent<UIDocument>();

            uiDocument.panelSettings = UIToolkitPanelConfig.PanelSettings;
            uiDocument.visualTreeAsset = UIToolkitPanelConfig.VisualTree;

            uiDocument.enabled = false;
            
            UIDocument = uiDocument;
        }

        #endregion

        #region Open

        protected override async void OnOpen(IUIPanel source)
        {
            base.OnOpen(source);
            
            UIDocument.enabled = true;

            RootVisualElement = UIDocument.rootVisualElement;
            RootVisualElement.styleSheets.Add(UISetting.UIPanelGeneralSetting.defaultTheme);
            RootVisualElement.style.visibility = Visibility.Hidden;

            UIMainPart = RootVisualElement.QueryStrictly(UIToolkitPanelConfig.UIMainPartName,
                nameof(UIToolkitPanelConfig.UIMainPartName));

            OpenCTS = new();

            await UniTask.Yield(OpenCTS.Token);
            
            OnLayoutChange();

            OnPostLayoutChange();
        }

        #endregion

        protected override void OnClose()
        {
            base.OnClose();
            
            OpenCTS?.Cancel();
        }

        protected override void OnPostClose()
        {
            base.OnPostClose();
            
            UIDocument.enabled = false;
        }

        #region Layout Change
        
        protected virtual void OnLayoutChange()
        {

        }

        protected virtual void OnPostLayoutChange()
        {
            RootVisualElement.style.visibility = Visibility.Visible;

            if (UIToolkitPanelConfig.IgnoreMouseEvents)
            {
                foreach (var visualElement in RootVisualElement.GetAll<VisualElement>())
                {
                    visualElement.pickingMode = PickingMode.Ignore;
                }
            }
        }

        #endregion

        #region Add Visual Element

        public void AddVisualElement(VisualElement parent, VisualElement newChild)
        {
            parent.Add(newChild);

            OnNewVisualElementPostProcessing(newChild);
        }

        protected virtual void OnNewVisualElementPostProcessing(VisualElement newVisualElement)
        {
            if (UIToolkitPanelConfig.IgnoreMouseEvents)
            {
                foreach (var visualElement in newVisualElement.GetAll<VisualElement>())
                {
                    visualElement.pickingMode = PickingMode.Ignore;
                }
            }
        }

        #endregion
    }
}