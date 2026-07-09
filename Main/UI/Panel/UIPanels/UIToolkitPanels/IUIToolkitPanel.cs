using System;
using UnityEngine.UIElements;

namespace VMFramework.UI
{
    public interface IUIToolkitPanel : IUIPanel, IVisualElementGenerator, IBindVisualElementsManagerProvider
    {
        public UIDocument UIDocument { get; }
        
        public VisualElement RootVisualElement { get; }
        
        public event Action<IUIToolkitPanel> OnLayoutChangeEvent;
    }
}