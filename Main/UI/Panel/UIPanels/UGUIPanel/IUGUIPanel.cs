using UnityEngine;
using UnityEngine.UI;

namespace VMFramework.UI
{
    public interface IUGUIPanel : IUIPanel
    {
        public GameObject UIMainGameObject { get; }
        
        public RectTransform UIMainRectTransform { get; }
        
        public Canvas Canvas { get; }
        
        public CanvasScaler CanvasScaler { get; }
    }
}