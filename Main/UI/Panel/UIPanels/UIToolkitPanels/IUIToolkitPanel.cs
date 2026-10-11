using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace VMFramework.UI
{
    public interface IUIToolkitPanel : IUIPanel, IVisualElementGenerator, IBindVisualElementsManagerProvider
    {
        public UIDocument UIDocument { get; }
        
        public VisualElement RootVisualElement { get; }

        public Task LayoutCompletion { get; }

        public CancellationToken LayoutCancellationToken { get; }

        internal void CompleteLayoutRetirement(Task actualLayout);

        public event Action<IUIToolkitPanel, VisualElement> OnRootVisualElementReady;

        public event Action<IUIToolkitPanel, UIToolkitRootRetirement> OnRootVisualElementReleased;
        
        public event Action<IUIToolkitPanel> OnLayoutChangeEvent;
    }
}
