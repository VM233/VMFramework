using UnityEngine.UIElements;

namespace VMFramework.UI
{
    public interface ISlotProvider
    {
        public bool enableBackgroundImageOverride => false;

        public StyleBackground GetBackgroundImage() => null;

        public bool enableBorderImageOverride => false;

        public StyleBackground GetBorderImage() => null;

        public StyleBackground GetIconImage();

        public string GetDescriptionText();

        public void HandleRightMouseClickEvent(IUIPanel source)
        {

        }

        public void HandleMouseEnterEvent(IUIPanel source)
        {

        }

        public void HandleMouseLeaveEvent(IUIPanel source)
        {

        }
    }
}