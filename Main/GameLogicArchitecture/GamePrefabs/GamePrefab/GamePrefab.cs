using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.GameLogicArchitecture
{
    public abstract partial class GamePrefab : IGamePrefab
    {
        #region Constants

        protected const string ACTIVE_STATE_AND_DEBUGGING_MODE_GROUP = "ActiveStateAndDebuggingModeGroup";

        protected const string TAB_GROUP_NAME = "TabGroup";

        protected const string BASIC_CATEGORY = "Basic";

        protected const string TOOLS_CATEGORY = "Tools";

        protected const string RUNTIME_DATA_CATEGORY = "Runtime Data";

        #endregion

        #region Configs

        [HideInInspector]
        [SerializeField, JsonProperty(Order = -10000)]
        private string _id;

        [LabelText(SdfIconType.Activity)]
        [JsonProperty(Order = -9000), PropertyOrder(-9000)]
        [HorizontalGroup(ACTIVE_STATE_AND_DEBUGGING_MODE_GROUP)]
        public bool isActive = true;

        [LabelText(SdfIconType.BugFill)]
        [JsonProperty(Order = -8000), PropertyOrder(-8000)]
        [HorizontalGroup(ACTIVE_STATE_AND_DEBUGGING_MODE_GROUP)]
        public bool isDebugging = false;

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [MinValue(0), MaxValue(50000)]
        public int gameItemPrewarmCount = 0;

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [GameTagID]
        public HashSet<string> gameTags = new();

        #endregion

        #region Properties

        [LabelText("ID", SdfIconType.Globe)]
        [ShowInInspector]
        [IsNotNullOrEmpty(DrawCurrentRect = true)]
        [IsGamePrefabID]
        [ValidateIsNot(contents: IGamePrefab.NULL_ID, DrawCurrentRect = true)]
#if UNITY_EDITOR
        [Placeholder("@" + nameof(GetIDPlaceholderText) + "()")]
#endif
        [PropertyOrder(-10000)]
        public string id
        {
            get => _id;
            set
            {
                var oldID = _id;
                _id = value;
                OnIDChangedEvent?.Invoke(this, oldID, _id);
            }
        }

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [ShowInInspector]
        [HideIfNull]
        public virtual Type GameItemType => null;

        #endregion

        #region Events

        public event Action<IGamePrefab, string, string> OnIDChangedEvent;

        #endregion

        #region ID Prefix and Suffix

        /// <summary>
        /// The prefix of the ID.
        /// This id should start with this prefix.
        /// </summary>
        public virtual string IDPrefix => null;

        /// <summary>
        /// The suffix of the ID.
        /// This id should end with this suffix.
        /// </summary>
        public virtual string IDSuffix => null;

        public bool IsIDStartsWithPrefix
        {
            get
            {
                if (id.IsNullOrWhiteSpace() || IDPrefix.IsNullOrWhiteSpace())
                {
                    return true;
                }

                var prefix = IDPrefix.TrimEnd(' ', '_').TrimStart(' ', '_');

                return id.StartsWith(prefix);
            }
        }

        public bool IsIDEndsWithSuffix
        {
            get
            {
                if (id.IsNullOrWhiteSpace() || IDSuffix.IsNullOrWhiteSpace())
                {
                    return true;
                }

                var suffix = IDSuffix.TrimEnd(' ', '_').TrimStart(' ', '_');

                return id.EndsWith(suffix);
            }
        }

        #endregion

        #region Interface Implementations

        string IIDOwner<string>.id => id;

        string INameOwner.Name => id;

        bool IGamePrefab.IsActive
        {
            get => isActive;
            set => isActive = value;
        }

        bool IGamePrefab.IsDebugging
        {
            get => isDebugging;
            set => isDebugging = value;
        }

        int IGamePrefab.GameItemPrewarmCount => gameItemPrewarmCount;

        ICollection<string> IGameTagsOwner.GameTags => gameTags;

        #endregion

        #region To String

        public override string ToString()
        {
            return $"({GetType()}, id:{id})";
        }

        #endregion
    }
}