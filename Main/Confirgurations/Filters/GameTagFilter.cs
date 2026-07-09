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
                    var gameTags = gameTagsOwner.GameTags;
                    if (isAll)
                    {
                        bool isTargetGameTag = true;
                        foreach (var gameTag in gameTags)
                        {
                            if (gameTag.Contains(gameTag) == false)
                            {
                                isTargetGameTag = false;
                                break;
                            }
                        }
                        return isTargetGameTag ^ inversed;
                    }
                    else
                    {
                        bool isTargetGameTag = false;
                        foreach (var gameTag in gameTags)
                        {
                            if (gameTag.Contains(gameTag))
                            {
                                isTargetGameTag = true;
                                break;
                            }
                        }
                        return isTargetGameTag ^ inversed;
                    }
                }
            }
            else
            {
                if (gameTag.IsNullOrEmpty() == false)
                {
                    var isTargetGameTag = gameTagsOwner.GameTags.Contains(gameTag);
                    return isTargetGameTag ^ inversed;
                }
            }

            return true;
        }
    }
}