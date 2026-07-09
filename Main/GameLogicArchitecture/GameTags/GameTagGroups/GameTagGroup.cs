using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    [CreateAssetMenu(menuName = FrameworkMeta.NAME + "/" + FILE_NAME, fileName = "New " + FILE_NAME)]
    public partial class GameTagGroup : GameTagGroupBase
    {
        public const string FILE_NAME = "Game Tag Group";
        
        [ListDrawerSettings(ShowFoldout = false)]
        [OnValueChanged(nameof(OnGameTagInfosChanged), true)]
        [OnCollectionChanged(nameof(OnGameTagInfosChanged))]
        public List<GameTagInfo> gameTagInfos = new();

        public override IEnumerable<GameTagInfo> GetGameTagInfos()
        {
            return gameTagInfos;
        }
        
        private void OnGameTagInfosChanged()
        {
            CoreSetting.GameTagGeneralSetting.InitGameTags();
        }
    }
}