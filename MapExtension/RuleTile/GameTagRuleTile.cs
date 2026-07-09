using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.MapExtension
{
    [CreateAssetMenu]
    public class GameTagRuleTile : RuleTile, IGameTagsOwner
    {
        public List<string> gameTags = new();

        protected HashSet<string> gameTagsSet;

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject instantiatedGameObject)
        {
            gameTagsSet = new HashSet<string>(gameTags);
            return base.StartUp(position, tilemap, instantiatedGameObject);
        }

        protected virtual bool OnRuleMatch(int neighbor, TileBase other)
        {
            switch (neighbor)
            {
                case TilingRuleOutput.Neighbor.This:
                {
                    if (other == this)
                    {
                        return true;
                    }

                    if (gameTagsSet != null && other is IGameTagsOwner { GameTags: not null } gameTagsOwner)
                    {
                        if (gameTagsSet.Overlaps(gameTagsOwner.GameTags))
                        {
                            return true;
                        }
                    }

                    return false;
                }
                case TilingRuleOutput.Neighbor.NotThis:
                {
                    if (other == this)
                    {
                        return false;
                    }

                    if (gameTagsSet != null && other is IGameTagsOwner { GameTags: not null } gameTagsOwner)
                    {
                        if (gameTagsSet.Overlaps(gameTagsOwner.GameTags))
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        public override bool RuleMatch(int neighbor, TileBase other)
        {
            if (other is RuleOverrideTile ot)
                other = ot.m_InstanceTile;

            return OnRuleMatch(neighbor, other);
        }

        ICollection<string> IGameTagsOwner.GameTags => gameTagsSet;
    }
}