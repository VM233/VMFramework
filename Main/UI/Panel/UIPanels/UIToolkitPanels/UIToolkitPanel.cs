using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;
using VMFramework.Core;

namespace VMFramework.UI
{
    [RequireComponent(typeof(UIDocument))]
    [DisallowMultipleComponent]
    public class UIToolkitPanel : UIPanel, IUIToolkitPanel, IUIPanelPointerEventProvider, ILocalizedPanelModifier
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

        private Action<IUIPanel> OnPointerEnterEvent;
        private Action<IUIPanel> OnPointerLeaveEvent;

        void IUIPanelPointerEventProvider.AddPointerEvent(Action<IUIPanel> onPointerEnter,
            Action<IUIPanel> onPointerLeave)
        {
            OnPointerEnterEvent = onPointerEnter;
            OnPointerLeaveEvent = onPointerLeave;

            foreach (var visualElement in RootVisualElement.Children())
            {
                visualElement.RegisterCallback<MouseEnterEvent>(OnPointerEnter);
                visualElement.RegisterCallback<MouseLeaveEvent>(OnPointerLeave);
            }
        }

        void IUIPanelPointerEventProvider.RemovePointerEvent()
        {
            OnPointerEnterEvent = null;
            OnPointerLeaveEvent = null;

            foreach (var visualElement in RootVisualElement.Children())
            {
                visualElement.UnregisterCallback<MouseEnterEvent>(OnPointerEnter);
                visualElement.UnregisterCallback<MouseLeaveEvent>(OnPointerLeave);
            }
        }

        private void OnPointerEnter(MouseEnterEvent e)
        {
            OnPointerEnterEvent?.Invoke(this);
        }

        private void OnPointerLeave(MouseLeaveEvent e)
        {
            OnPointerLeaveEvent?.Invoke(this);
        }

        protected Locale lastLocale { get; private set; }

        void ILocalizedPanelModifier.OnCurrentLanguageChanged(Locale currentLocale)
        {
            OnCurrentLanguageChanged(currentLocale);
        }

        protected virtual void OnCurrentLanguageChanged(Locale currentLocale)
        {
            if (UISetting.UIPanelGeneralSetting.enableLanguageConfigs == false)
            {
                return;
            }

            if (lastLocale != null)
            {
                var previousLanguageConfig =
                    UISetting.UIPanelGeneralSetting.GetLanguageConfig(lastLocale.Identifier.Code);

                if (previousLanguageConfig != null)
                {
                    RootVisualElement.styleSheets.Remove(previousLanguageConfig.styleSheet);
                }
            }

            lastLocale = currentLocale;

            var currentLanguageConfig =
                UISetting.UIPanelGeneralSetting.GetLanguageConfig(currentLocale.Identifier.Code);

            if (currentLanguageConfig != null)
            {
                RootVisualElement.styleSheets.Add(currentLanguageConfig.styleSheet);
            }
        }
    }
}
