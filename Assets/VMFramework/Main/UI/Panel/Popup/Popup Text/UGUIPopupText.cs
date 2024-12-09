using VMFramework.Core;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace VMFramework.UI
{
    public class UGUIPopupText : UGUIPopup, IPopupTextPanel
    {
        protected UGUIPopupTextConfig UGUIPopupTextConfig => (UGUIPopupTextConfig)GamePrefab;

        [ShowInInspector]
        protected TextMeshProUGUI textUGUI;

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

        protected override void OnCreate()
        {
            base.OnCreate();
            
            textUGUI = VisualObject.transform.QueryFirstComponentInChildren<TextMeshProUGUI>(
                UGUIPopupTextConfig.textName, true);

            textUGUI.AssertIsNotNull(nameof(textUGUI));
        }
    }
}
