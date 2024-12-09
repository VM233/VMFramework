using UnityEngine;

namespace VMFramework.UI
{
    public interface IPopupTextPanel : IPopupPanel
    {
        public string Text { get; set; }
        
        public Color TextColor { get; set; }
    }
}