using System;

namespace VMFramework.UI
{
    public interface IUIPanelPointerEventProvider : IUIPanel
    {
        public void AddPointerEvent(Action<IUIPanel> onPointerEnter,
            Action<IUIPanel> onPointerLeave);
        
        public void RemovePointerEvent()
        {
            
        }
    }
}