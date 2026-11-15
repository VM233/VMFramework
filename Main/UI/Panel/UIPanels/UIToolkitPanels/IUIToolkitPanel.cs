using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace VMFramework.UI
{
    public interface IUIToolkitPanel : IUIPanel, IVisualElementGenerator, IBindVisualElementsManagerProvider
    {
        public UIDocument UIDocument { get; }

        /// <summary>The actual native document tree, including a retained, logically closed view.</summary>
        public VisualElement DocumentRoot { get; }
        
        public VisualElement RootVisualElement { get; }

        public Task LayoutCompletion { get; }

        public CancellationToken LayoutCancellationToken { get; }

        internal void CompleteLayoutRetirement(Task actualLayout);

        public event Action<IUIToolkitPanel, VisualElement> OnRootVisualElementReady;

        public event Action<IUIToolkitPanel, UIToolkitRootRetirement> OnRootVisualElementReleased;
        
        public event Action<IUIToolkitPanel> OnLayoutChangeEvent;
    }
}
