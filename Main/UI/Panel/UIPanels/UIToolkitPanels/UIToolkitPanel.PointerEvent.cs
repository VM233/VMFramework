using System;
using UnityEngine.UIElements;

namespace VMFramework.UI
{
    public partial class UIToolkitPanel : IUIPanelPointerEventProvider
    {
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
    }
}