using Sirenix.OdinInspector;
using UnityEngine.UIElements;

namespace VMFramework.UI
{
    [UxmlElement]
    public partial class CarouselGroupVisualElement : VisualElement
    {
        public override VisualElement contentContainer => container ?? this;

        [ShowInInspector]
        public VisualElement container { get; }

        public CarouselGroupVisualElement() : base()
        {
            var container = new VisualElement()
            {
                name = "Container"
            };
            container.AddToClassList("carouselGroupContainer");

            Add(container);

            this.container = container;
        }
    }
}