using UnityEngine;

namespace VMFramework.UI
{
    public interface ISlotProvider
    {
        public bool? IsEnabled()
        {
            return null;
        }
        
        public Color? GetColor()
        {
            return null;
        }

        public bool TryGetBackgroundImage(out Sprite backgroundImage)
        {
            backgroundImage = null;
            return false;
        }

        public bool TryGetBorderImage(out Sprite borderImage)
        {
            borderImage = null;
            return false;
        }

        public bool TryGetIconImage(out Sprite iconImage)
        {
            iconImage = null;
            return false;
        }

        public string GetDescriptionText()
        {
            return string.Empty;
        }
    }
}