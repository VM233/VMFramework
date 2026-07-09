using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace VMFramework.UI
{
    public class UGUIPopupText : UGUIPanel, IPopupTextPanel
    {
        protected UGUIPopupTextConfig UGUIPopupTextConfig => (UGUIPopupTextConfig)GamePrefab;
        
        [Required]
        public TextMeshProUGUI textUGUI;

        public string Text
        {
            get => textUGUI.text;
            set => textUGUI.text = value;
        }

        public Color TextColor
        {
            get => textUGUI.color;
            set => textUGUI.color = value;
        }
    }
}
