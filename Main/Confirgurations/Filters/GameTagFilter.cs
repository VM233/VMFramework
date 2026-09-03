using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    public struct GameTagFilter : IFilter
    {
        public bool isMultiple;
        
        [GameTagID]
        [HideIf(nameof(isMultiple))]
        public string gameTag;
        
        [GameTagID]
        [ShowIf(nameof(isMultiple))]
        [IsNotNullOrEmpty]
        public string[] gameTags;

        [ShowIf(nameof(isMultiple))]
        public bool isAll;

        public bool inversed;

        public bool IsMatch(object obj)
        {
            var gameTagsOwner = (IGameTagsOwner)obj;
            
            if (isMultiple)
            {
                if (gameTags is { Length: > 0 })
                {
                    bool matches = isAll
                        ? gameTagsOwner.HasAllTags(gameTags)
                        : gameTagsOwner.HasAnyTags(gameTags);
                    return matches ^ inversed;
                }
            }
            else
            {
                if (gameTag.IsNullOrEmpty() == false)
                {
                    var isTargetGameTag = gameTagsOwner.HasTag(gameTag);
                    return isTargetGameTag ^ inversed;
                }
            }

            return true;
        }
    }
}
