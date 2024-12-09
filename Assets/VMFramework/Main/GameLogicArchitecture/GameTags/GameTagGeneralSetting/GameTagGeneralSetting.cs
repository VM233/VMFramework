using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture 
{
    public sealed partial class GameTagGeneralSetting : GeneralSetting
    {
        private const string GAME_TAG_CATEGORY = "Game Tag";

#if UNITY_EDITOR
        [TabGroup(TAB_GROUP_NAME, GAME_TAG_CATEGORY)]
        [OnValueChanged(nameof(OnGameTagInfosChanged), true)]
        [OnCollectionChanged(nameof(OnGameTagInfosChanged))]
        [Searchable]
        [ListDrawerSettings(NumberOfItemsPerPage = 8)]
#endif
        [SerializeField]
        private List<GameTagInfo> gameTagInfos = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnGameTagInfosChanged()
        {
            InitGameTags();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CheckGameTags()
        {
            if (gameTagInfos.Any(gameTypeInfo => gameTypeInfo == null))
            {
                Debug.LogError($"There is a null {nameof(GameTagInfo)} in {nameof(gameTagInfos)}.");
            }

            foreach (var gameTagInfo in gameTagInfos)
            {
                if (gameTagInfo.id == null)
                {
                    Debugger.LogWarning(
                        $"Existing initial {nameof(gameTagInfo)} has an empty {nameof(gameTagInfo.id)}.");
                    continue;
                }

                if (gameTagInfo.id.IsWhiteSpace())
                {
                    Debugger.LogWarning(
                        $"Existing initial {nameof(gameTagInfo)} has an empty {nameof(gameTagInfo.id)} after trimming.");
                    continue;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InitGameTags()
        {
            GameTag.Clear();

            foreach (var gameTagInfo in gameTagInfos)
            {
                if (gameTagInfo == null)
                {
                    continue;
                }

                if (gameTagInfo.id.IsNullOrEmpty())
                {
                    continue;
                }
                
                GameTag.AddTag(gameTagInfo);
            }
        }
    }
}