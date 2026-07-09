using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.OdinExtensions;

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
        
#if UNITY_EDITOR
        [TabGroup(TAB_GROUP_NAME, GAME_TAG_CATEGORY)]
        [OnValueChanged(nameof(OnGameTagInfosChanged), true)]
        [OnCollectionChanged(nameof(OnGameTagInfosChanged))]
        [DisallowDuplicateElements]
#endif
        [SerializeField]
        private List<GameTagGroupBase> gameTagGroups = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnGameTagInfosChanged()
        {
            InitGameTags();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CheckGameTags()
        {
            if (EnumerateGameTagInfos().Any(gameTypeInfo => gameTypeInfo == null))
            {
                Debug.LogError($"There is a null {nameof(GameTagInfo)} in {nameof(gameTagInfos)}.");
            }

            foreach (var gameTagInfo in EnumerateGameTagInfos())
            {
                if (gameTagInfo.id == null)
                {
                    UnityEngine.Debug.LogWarning(
                        $"Existing initial {nameof(gameTagInfo)} has an empty {nameof(gameTagInfo.id)}.");
                    continue;
                }

                if (gameTagInfo.id.IsEmptyOrWhiteSpace())
                {
                    UnityEngine.Debug.LogWarning(
                        $"Existing initial {nameof(gameTagInfo)} has an empty {nameof(gameTagInfo.id)} after trimming.");
                    continue;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InitGameTags()
        {
            GameTag.Clear();

            foreach (var gameTagInfo in EnumerateGameTagInfos())
            {
                ProcessGameTagInfo(gameTagInfo);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ProcessGameTagInfo(GameTagInfo gameTagInfo)
        {
            if (gameTagInfo == null)
            {
                return;
            }

            if (gameTagInfo.id.IsNullOrEmpty())
            {
                return;
            }

            if (GameTag.HasTag(gameTagInfo.id))
            {
                UnityEngine.Debug.LogWarning($"Duplicate {nameof(GameTag)} with id {gameTagInfo.id} found.");
                return;
            }
                
            GameTag.AddTag(gameTagInfo);
        }

        public IEnumerable<GameTagInfo> EnumerateGameTagInfos()
        {
            foreach (var gameTagGroup in gameTagGroups)
            {
                if (gameTagGroup == null)
                {
                    continue;
                }
                
                foreach (var gameTagInfo in gameTagGroup.GetGameTagInfos())
                {
                    yield return gameTagInfo;
                }
            }
            
            foreach (var gameTagInfo in gameTagInfos)
            {
                yield return gameTagInfo;
            }
        }
    }
}