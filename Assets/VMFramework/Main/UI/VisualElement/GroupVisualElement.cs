using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace VMFramework.UI
{
    [UxmlElement]
    public partial class GroupVisualElement : VisualElement
    {
        public GroupVisualElement() : base()
        {
            AddToClassList("group");
        }
    }
}
