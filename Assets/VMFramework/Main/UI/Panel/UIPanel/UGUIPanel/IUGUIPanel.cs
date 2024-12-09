using UnityEngine;
using UnityEngine.UI;

namespace VMFramework.UI
{
    public interface IUGUIPanel : IUIPanel
    {
        public RectTransform VisualRectTransform { get; }
        
        public Canvas Canvas { get; }
        
        public CanvasScaler CanvasScaler { get; }
    }
}