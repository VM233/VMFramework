using System.Collections.Generic;

namespace VMFramework.GameLogicArchitecture
{
    public static class GameTagNameUtility
    {
        public static IEnumerable<string> GetTagNames(IEnumerable<string> gameTagsID)
        {
            foreach (var gameTagID in gameTagsID)
            {
                if (GameTag.TryGetName(gameTagID, out string tagName))
                {
                    yield return tagName;
                }
            }
        }
    }
}