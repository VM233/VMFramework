using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.OdinExtensions;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public sealed class PopupManager : ManagerBehaviour<PopupManager>
    {
        #region Popup Text

        [Button]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IPopupTextPanel PopupText(
            [UIPresetID(typeof(IPopupTextConfig))]
            string damagePopupID, TracingConfig config, SimpleText text, Color? textColor = null)
        {
            var popup = TracingUIManager.OpenOn<IPopupTextPanel>(damagePopupID, config, out _);

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
