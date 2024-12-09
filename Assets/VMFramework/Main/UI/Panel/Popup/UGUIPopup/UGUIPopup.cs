using System.Threading;
using VMFramework.Core;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.UI
{
    public class UGUIPopup : UGUIPanel
    {
        protected UGUIPopupConfig UGUIPopupConfig => (UGUIPopupConfig)GamePrefab;

        [ShowInInspector]
        protected Transform PopupContainer { get; private set; }

        private CancellationTokenSource openingCTS;

        protected override void OnCreate()
        {
            base.OnCreate();
            
            PopupContainer = VisualObject.transform.QueryFirstComponentInChildren<RectTransform>(
                UGUIPopupConfig.popupContainerName, true);

            PopupContainer.AssertIsNotNull(nameof(PopupContainer));
        }

        protected override async void OnOpen(IUIPanel source)
        {
            base.OnOpen(source);
            
            PopupContainer.ResetLocalArguments();

            openingCTS = new();
            
            await AwaitToOpen(openingCTS.Token);
            
            if (UGUIPopupConfig.enableContainerAnimation)
            {
                if (UGUIPopupConfig.splitContainerAnimation == false)
                {
                    if (UGUIPopupConfig.autoCloseAfterContainerAnimation)
                    {
                        this.Close();
                    }
                }
            }
        }

        protected virtual async UniTask AwaitToOpen(CancellationToken token)
        {
            if (UGUIPopupConfig.enableContainerAnimation)
            {
                if (UGUIPopupConfig.splitContainerAnimation)
                {
                    await UGUIPopupConfig.startContainerAnimation.RunAndAwait(PopupContainer, token);
                }
                else
                {
                    await UGUIPopupConfig.containerAnimation.RunAndAwait(PopupContainer, token);
                }
            }
        }

        protected override async void OnClose()
        {
            base.OnClose();
            
            openingCTS.Cancel();

            IsOpened = true;
            
            await AwaitToClose();
            
            IsOpened = false;
        }

        protected virtual async UniTask AwaitToClose()
        {
            if (UGUIPopupConfig.enableContainerAnimation)
            {
                if (UGUIPopupConfig.splitContainerAnimation)
                {
                    UGUIPopupConfig.startContainerAnimation.Kill(PopupContainer);

                    await UGUIPopupConfig.endContainerAnimation.RunAndAwait(PopupContainer);
                }
            }
        }
    }
}
