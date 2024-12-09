using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.Maps
{
    public static class RuleUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rule GetRule(this ExtendedRuleTile tile, EightDirectionsNeighbors<ExtendedRuleTile> neighbors,
            IReadOnlyCollection<Rule> rules)
        {
            foreach (var rule in rules)
            {
                if (rule.enable == false)
                {
                    continue;
                }

                var neighborLimits = rule.neighborLimits;

                if (neighbors.Where((t, i) => tile.SatisfyLimit(t, neighborLimits[i]) == false).Any())
                {
                    continue;
                }

                return rule;
            }

            return tile.hasParent ? tile.parentRuleTile.GetRule(neighbors, rules) : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SatisfyLimit(this ExtendedRuleTile tile, ExtendedRuleTile neighborTile, Limit limit)
        {
            switch (limit.limitType)
            {
                case LimitType.None:
                    return true;
                case LimitType.This:
                    if (neighborTile == null)
                    {
                        return false;
                    }

                    if (neighborTile.id == tile.id)
                    {
                        return true;
                    }

                    if (neighborTile.ruleMode == RuleMode.Inheritance)
                    {
                        return tile.SatisfyLimit(neighborTile.parentRuleTile, limit);
                    }

                    return false;

                case LimitType.NotThis:
                    if (neighborTile == null)
                    {
                        return true;
                    }

                    if (neighborTile.id == tile.id)
                    {
                        return false;
                    }

                    if (neighborTile.ruleMode == RuleMode.Inheritance)
                    {
                        return tile.SatisfyLimit(neighborTile.parentRuleTile, limit);
                    }

                    return true;

                case LimitType.SpecificTiles:
                    if (neighborTile == null)
                    {
                        return false;
                    }

                    if (limit.specificTiles.Contains(neighborTile.id))
                    {
                        return true;
                    }

                    if (neighborTile.ruleMode == RuleMode.Inheritance)
                    {
                        return tile.SatisfyLimit(neighborTile.parentRuleTile, limit);
                    }

                    return false;

                case LimitType.NotSpecificTiles:
                    if (neighborTile == null)
                    {
                        return true;
                    }

                    if (limit.specificTiles.Contains(neighborTile.id))
                    {
                        return false;
                    }

                    if (neighborTile.ruleMode == RuleMode.Inheritance)
                    {
                        return tile.SatisfyLimit(neighborTile.parentRuleTile, limit);
                    }

                    return true;

                case LimitType.IsEmpty:
                    return neighborTile == null || neighborTile.id == ExtendedRuleTile.EMPTY_TILE_ID;
                case LimitType.NotEmpty:
                    return neighborTile != null && neighborTile.id != ExtendedRuleTile.EMPTY_TILE_ID;
                case LimitType.ThisOrParent:

                    if (neighborTile == null)
                    {
                        return false;
                    }

                    return tile.GetRoot().SatisfyLimit(neighborTile, new()
                    {
                        limitType = LimitType.This
                    });

                case LimitType.NotThisAndParent:

                    if (neighborTile == null)
                    {
                        return true;
                    }

                    return tile.GetRoot().SatisfyLimit(neighborTile, new()
                    {
                        limitType = LimitType.NotThis
                    });
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}