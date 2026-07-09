using Sirenix.OdinInspector;
using VMFramework.Configuration;

namespace VMFramework.GameLogicArchitecture
{
    public abstract partial class GameSettingBase : SerializedScriptableObject, ICheckableConfig
    {
        #region Categories

        protected const string TAB_GROUP_NAME = "TabGroup";

        protected const string DEBUGGING_CATEGORY = "Debug";

        protected const string MISCELLANEOUS_CATEGORY = "Misc";

        protected const string METADATA_CATEGORY = "Metadata";

        #endregion

        public virtual void CheckSettings()
        {

        }
    }
}