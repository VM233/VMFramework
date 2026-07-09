#if UNITY_EDITOR
using VMFramework.Configuration;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GamePrefabBehaviour
    {
        #region On Inspector Init

        protected virtual void OnInspectorInit()
        {
            gameTags ??= new();
        }
        
        void IInspectorConfig.OnInspectorInit()
        {
            OnInspectorInit();
        }

        #endregion
        
        #region ID

        private const string PLACEHOLDER_TEXT = "Please enter an ID";

        private string GetIDPlaceholderText()
        {
            if (IDSuffix.IsNullOrWhiteSpace())
            {
                return PLACEHOLDER_TEXT;
            }

            return PLACEHOLDER_TEXT + $"and end with: _{IDSuffix}";
        }

        #endregion
    }
}
#endif