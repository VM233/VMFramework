using UnityEngine.UIElements;

namespace VMFramework.UI
{
    public interface IVisualElementGenerator
    {
        public delegate void GenerateVisualElementHandler(IVisualElementGenerator generator, VisualElement root);
        
        public event GenerateVisualElementHandler OnGenerateVisualElement;
        
        public VisualElement GenerateVisualElement();
    }
}