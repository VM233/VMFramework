using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;

namespace VMFramework.UI
{
    [RequireComponent(typeof(UIDocument))]
    [DisallowMultipleComponent]
    public partial class UIToolkitPanel : UIPanel, IUIToolkitPanel
    {
        [TitleGroup(ComponentNames.CONFIG)]
        public bool autoPanelSettings = true;

        public UIDocument UIDocument { get; private set; }

        protected IUIToolkitPanelConfig UIToolkitPanelConfig => (IUIToolkitPanelConfig)GamePrefab;

        public VisualElement RootVisualElement { get; private set; }

        public event Action<IUIToolkitPanel> OnLayoutChangeEvent;

        public event IUIToolkitPanel.GenerateVisualElementHandler OnGenerateVisualElement;
        
        protected CancellationTokenSource OpenCTS { get; private set; }

        public BindVisualElementsManager BindVisualElementsManager { get; protected set; }

        #region Pool Events

        protected override void OnCreate()
        {
            BindVisualElementsManager = GetComponentInChildren<BindVisualElementsManager>();
            
            base.OnCreate();

            var uiDocument = GetComponent<UIDocument>();

            if (autoPanelSettings)
            {
                uiDocument.panelSettings = UIToolkitPanelConfig.PanelSettings;
            }

            uiDocument.visualTreeAsset = UIToolkitPanelConfig.VisualTree;

            uiDocument.enabled = false;

            UIDocument = uiDocument;
        }

        #endregion

        #region Open

        protected override async void OnOpenInternal(IUIPanel source)
        {
            base.OnOpenInternal(source);

            if (UIDocument.enabled == false)
            {
                UIDocument.enabled = true;
            }

            RootVisualElement = UIDocument.rootVisualElement;

            RootVisualElement.DisplayFlex();

            RootVisualElement.style.visibility = Visibility.Hidden;
            
            OnGenerateVisualElement?.Invoke(this, RootVisualElement);

            OpenCTS = new();

            try
            {
                await UniTask.Yield(OpenCTS.Token);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            OnLayoutChange();

            OnLayoutChangeEvent?.Invoke(this);

            OnPostLayoutChange();
        }

        #endregion

        #region Close

        protected override void OnPreCloseInternal()
        {
            base.OnPreCloseInternal();

            OpenCTS?.Cancel();
        }

        protected override void OnPostCloseInternal()
        {
            base.OnPostCloseInternal();

            if (UIToolkitPanelConfig.CloseMode == UIToolkitPanelCloseMode.DisableDocument)
            {
                UIDocument.enabled = false;
            }
            else
            {
                RootVisualElement.DisplayNone();
            }
        }

        #endregion

        #region Layout Change

        protected virtual void OnLayoutChange()
        {

        }

        protected virtual void OnPostLayoutChange()
        {
            RootVisualElement.style.visibility = Visibility.Visible;

            if (UIToolkitPanelConfig.IgnoreMouseEvents)
            {
                RootVisualElement.SetPickingMode(PickingMode.Ignore);
            }
        }

        #endregion

        public virtual VisualElement GenerateVisualElement()
        {
            var uiDocument = GetComponent<UIDocument>();
            var root = uiDocument.visualTreeAsset.CloneTree();
            OnGenerateVisualElement?.Invoke(this, root);
            return root;
        }
    }
}