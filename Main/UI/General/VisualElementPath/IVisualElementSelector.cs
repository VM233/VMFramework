using UnityEngine.UIElements;

namespace VMFramework.UI
{
    public interface IVisualElementSelector
    {
        public VisualElement Query(VisualElement root);

        public T Query<T>(VisualElement root)
            where T : VisualElement;
    }
}