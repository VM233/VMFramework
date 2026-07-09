using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.GameEvents
{
    public partial class KeyCodeTranslation : BaseConfig, IIDOwner<KeyCode>, INameOwner
    {
        [HideLabel]
        public KeyCode keyCode;

        public LocalizedString translation = new();

        #region Constructor

        public KeyCodeTranslation()
        {

        }

        public KeyCodeTranslation(KeyCode keyCode, LocalizedString translation)
        {
            this.keyCode = keyCode;
            this.translation = translation;
        }

        #endregion

        #region IIDOwner

        KeyCode IIDOwner<KeyCode>.id => keyCode;

        string INameOwner.Name => keyCode.ToString();

        #endregion
    }
}