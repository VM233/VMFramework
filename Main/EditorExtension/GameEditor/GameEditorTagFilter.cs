#if UNITY_EDITOR
using System.Collections.Generic;
using VMFramework.Configuration;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Editor.GameEditor
{
    internal static class GameEditorTagFilter
    {
        internal static HashSet<TreeNodePreBuildInfo> SelectVisibleNodes(
            IEnumerable<TreeNodePreBuildInfo> nodes, string[] tags, bool matchAll)
        {
            var visible = new HashSet<TreeNodePreBuildInfo>();
            if (tags.Length == 0)
            {
                visible.UnionWith(nodes);
                return visible;
            }

            var filter = new GameTagFilter
            {
                isMultiple = true,
                gameTags = tags,
                isAll = matchAll
            };
            var gamePrefabs = new List<IGamePrefab>();
            foreach (var info in nodes)
            {
                if (MatchesNode(info.node, filter, gamePrefabs) == false)
                {
                    continue;
                }

                // Ancestors provide navigation, not a match that admits their other children.
                for (var ancestor = info; ancestor != null && visible.Add(ancestor);
                     ancestor = ancestor.parentInfo)
                {
                }
            }

            return visible;
        }

        private static bool MatchesNode(object node, GameTagFilter filter, List<IGamePrefab> gamePrefabs)
        {
            if (node is IGameTagsOwner owner && filter.IsMatch(owner))
            {
                return true;
            }

            if (node is not IGamePrefabsProvider provider)
            {
                return false;
            }

            gamePrefabs.Clear();
            provider.GetGamePrefabs(gamePrefabs);
            foreach (var gamePrefab in gamePrefabs)
            {
                // All selected tags must match one config, not a union across a multiple wrapper.
                if (gamePrefab != null && filter.IsMatch(gamePrefab))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
#endif
