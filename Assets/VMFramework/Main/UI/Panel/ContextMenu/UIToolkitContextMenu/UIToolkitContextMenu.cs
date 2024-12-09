using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.GameEvents;

namespace VMFramework.UI
{
    public class UIToolkitContextMenu : UIToolkitPanel, IContextMenu
    {
        private struct ContextMenuEntryInfo
        {
            public IconLabelVisualElement iconLabel;
        }

        protected UIToolkitContextMenuConfig ContextMenuConfig => (UIToolkitContextMenuConfig)GamePrefab;

        [ShowInInspector]
        private VisualElement contextMenuEntryContainer;

        [ShowInInspector]
        private List<ContextMenuEntryInfo> entryInfos = new();

        [ShowInInspector]
        public IContextMenuProvider Provider { get;private set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            
            UIPanelPointerEventManager.OnPanelOnMouseClickChanged += (oldPanel, currentPanel) =>
            {
                if (currentPanel != this)
                {
                    this.Close();
                }
            };

            if (ContextMenuConfig.gameEventIDsToClose != null)
            {
                foreach (var gameEventID in ContextMenuConfig.gameEventIDsToClose)
                {
                    GameEventManager.AddCallback(gameEventID, this.Close, GameEventPriority.TINY);
                }
            }
        }

        protected override void OnClear()
        {
            base.OnClear();
            
            if (ContextMenuConfig.gameEventIDsToClose != null)
            {
                foreach (var gameEventID in ContextMenuConfig.gameEventIDsToClose)
                {
                    GameEventManager.RemoveCallback(gameEventID, this.Close);
                }
            }
        }

        protected override void OnOpen(IUIPanel source)
        {
            base.OnOpen(source);
            
            contextMenuEntryContainer = RootVisualElement.Q(
                ContextMenuConfig.contextMenuEntryContainerName);

            contextMenuEntryContainer.AssertIsNotNull(nameof(contextMenuEntryContainer));
        }

        protected override void OnClose()
        {
            base.OnClose();
            
            entryInfos.Clear();

            Provider = null;
        }

        public void Open(IContextMenuProvider provider, IUIPanel source)
        {
            this.Close();

            var entryContents = provider.GetContextMenuContent().ToArray();

            if (entryContents.Length == 0)
            {
                return;
            }

            if (entryContents.Length == 1 && ContextMenuConfig.autoExecuteIfOnlyOneEntry)
            {
                entryContents[0].action.Invoke();
                return;
            }

            this.Provider = provider;

            this.Open(source);

            contextMenuEntryContainer.Clear();

            foreach (var entryConfig in entryContents)
            {
                var iconLabel = new IconLabelVisualElement
                {
                    IconAlwaysDisplay = true
                };

                iconLabel.SetIcon(entryConfig.icon);
                iconLabel.SetContent(entryConfig.title);

                iconLabel.RegisterCallback<MouseUpEvent>(evt =>
                {
                    if (ContextMenuConfig.clickMouseButtonType.HasMouseButton(evt.button))
                    {
                        entryConfig.action.Invoke();
                        Close(provider);
                    }
                });

                iconLabel.RegisterCallback<MouseEnterEvent>(evt =>
                {
                    iconLabel.SetIcon(ContextMenuConfig.entrySelectedIcon);
                });
                iconLabel.RegisterCallback<MouseLeaveEvent>(evt => { iconLabel.SetIcon(entryConfig.icon); });

                AddVisualElement(contextMenuEntryContainer, iconLabel);

                entryInfos.Add(new ContextMenuEntryInfo
                {
                    iconLabel = iconLabel
                });
            }
        }

        public void Close(IContextMenuProvider provider)
        {
            if (this.Provider == provider)
            {
                this.Close();
            }
        }
    }
}