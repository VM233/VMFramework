using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.OdinExtensions;

namespace VMFramework.GameLogicArchitecture
{
    public abstract partial class SubGamePrefab : ISubGamePrefab
    {
        #region Constants

        protected const string TAB_GROUP_NAME = "TabGroup";

        protected const string BASIC_CATEGORY = "Basic";

        protected const string ACTIVE_STATE_AND_DEBUGGING_MODE_GROUP =
            TAB_GROUP_NAME + "/" + BASIC_CATEGORY + "/" + "ActiveStateAndDebuggingModeGroup";

        #endregion

        #region Configs

        [field: LabelText(SdfIconType.Activity)]
        [field: PropertyOrder(-9000)]
        [field: HorizontalGroup(ACTIVE_STATE_AND_DEBUGGING_MODE_GROUP)]
        [field: JsonProperty(Order = -9000), SerializeField]
        public bool IsActive { get; set; } = true;

        [field: LabelText(SdfIconType.BugFill)]
        [field: PropertyOrder(-8000)]
        [field: HorizontalGroup(ACTIVE_STATE_AND_DEBUGGING_MODE_GROUP)]
        [field: JsonProperty(Order = -8000), SerializeField]
        public bool IsDebugging { get; set; } = false;
        
        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [GameTagID]
        public List<string> gameTags = new();

        #endregion

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [ShowInInspector, HideInEditorMode]
        [ReadOnly]
        [PropertyOrder(-10000)]
        private string _id;

        public string id
        {
            get => _id;
            set
            {
                if (_id == value) return;
                var oldId = _id;
                _id = value;
                OnIDChangedEvent?.Invoke(this, oldId, value);
            }
        }

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [ShowInInspector]
        [HideIfNull]
        public virtual Type GameItemType => null;

        public event Action<IGamePrefab, string, string> OnIDChangedEvent;

        #region Interface Implementation

        int IGamePrefab.GameItemPrewarmCount => 0;

        string INameOwner.Name => id;

        ICollection<string> IGameTagsOwner.GameTags => gameTags;

        string IGamePrefab.IDPrefix => null;
        string IGamePrefab.IDSuffix => null;

        #endregion
    }
}