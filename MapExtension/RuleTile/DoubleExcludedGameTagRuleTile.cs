using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.MapExtension
{
    [CreateAssetMenu]
    public class DoubleExcludedGameTagRuleTile : GameTagRuleTile
    {
        public List<string> excludedGameTagsA = new();

        public List<string> excludedGameTagsB = new();

        public override Type m_NeighborType => typeof(Neighbor);

        protected HashSet<string> excludedGameTagsSetA;

        protected HashSet<string> excludedGameTagsSetB;

        public class Neighbor : TilingRuleOutput.Neighbor
        {
            public const int ThisButExcludedA = 3;
            public const int ThisButExcludedB = 4;
        }

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject instantiatedGameObject)
        {
            excludedGameTagsSetA = new HashSet<string>(excludedGameTagsA);
            excludedGameTagsSetB = new HashSet<string>(excludedGameTagsB);

            return base.StartUp(position, tilemap, instantiatedGameObject);
        }

        protected override bool OnRuleMatch(int neighbor, TileBase other)
        {
            switch (neighbor)
            {
                case Neighbor.ThisButExcludedA:
                {
                    if (other == this)
                    {
                        return true;
                    }
                    
                    if (gameTagsSet != null && other is IGameTagsOwner { GameTags: not null } gameTagsOwner)
                    {
                        if (gameTagsSet.Overlaps(gameTagsOwner.GameTags))
                        {
                            return excludedGameTagsSetA.Overlaps(gameTagsOwner.GameTags) == false;
                        }
                    }

                    return false;
                }
                case Neighbor.ThisButExcludedB:
                {
                    if (other == this)
                    {
                        return true;
                    }
                    
                    if (gameTagsSet != null && other is IGameTagsOwner { GameTags: not null } gameTagsOwner)
                    {
                        if (gameTagsSet.Overlaps(gameTagsOwner.GameTags))
                        {
                            return excludedGameTagsSetB.Overlaps(gameTagsOwner.GameTags) == false;
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
                            if (excludedGameTagsSetA.Overlaps(gameTagsOwner.GameTags))
                            {
                                return true;
                            }
                            
                            if (excludedGameTagsSetB.Overlaps(gameTagsOwner.GameTags))
                            {
                                return true;
                            }

                            return false;
                        }
                    }

                    return true;
                }
            }

            return base.OnRuleMatch(neighbor, other);
        }
    }
}