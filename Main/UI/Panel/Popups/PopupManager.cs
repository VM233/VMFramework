using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.OdinExtensions;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public class PopupManager : ManagerBehaviour<PopupManager>
    {
        #region Popup Text

        [Button]
        public IPopupTextPanel PopupText([GamePrefabID(typeof(IPopupTextConfig))] string damagePopupID,
            TracingConfig config, SimpleText text, Color? textColor = null)
        {
            var popup = TracingUIManager.Instance.OpenOn<IPopupTextPanel>(damagePopupID, config, out _);

            popup.Text = text.text;

            if (textColor.HasValue)
            {
                popup.TextColor = textColor.Value;
            }

            return popup;
        }

        #endregion
    }
}
