using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using VMFramework.Core.Pools;

namespace VMFramework.Core
{
    public static class AStar
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FindPath<TNode>(TNode start, TNode end,
            [NotNull] Func<TNode, TNode, float> adjacencyCostGetter,
            [NotNull] Func<TNode, TNode, float> heuristicCostGetter, [NotNull] ICollection<TNode> path)
            where TNode : class, IAStarNode<TNode>
        {
            var openList = CollectionPool<GenericMinMap<TNode>>.Shared.Get();
            openList.Clear();
            var closedList = HashSetPool<TNode>.Shared.Get();
            closedList.Clear();
            var neighbors = ListPool<TNode>.Shared.Get();
            var success = false;
            
            openList.Add(start);
            
            while (openList.Count > 0)
            {
                var current = openList.ExtractMin();

                if (current == end)
                {
                    success = true;
                    break;
                }
                
                closedList.Add(current);
                
                neighbors.Clear();
                current.GetNeighbors(neighbors);

                foreach (var neighbor in neighbors)
                {
                    if (closedList.Contains(neighbor))
                    {
                        continue;
                    }

                    if (openList.Contains(neighbor))
                    {
                        continue;
                    }
                    
                    var costToNeighbor = adjacencyCostGetter(current, neighbor);
                    neighbor.GCost = current.GCost + costToNeighbor;
                    neighbor.HCost = heuristicCostGetter(neighbor, end);
                    neighbor.Parent = current;
                    
                    openList.Add(neighbor);
                }
            }

            if (success)
            {
                var tempPath = ListPool<TNode>.Shared.Get();
                tempPath.Clear();
                var current = end;
                while (current != start)
                {
                    tempPath.Add(current);
                    current = current.Parent;
                }
                tempPath.Add(start);
                for (var i = tempPath.Count - 1; i >= 0; i--)
                {
                    path.Add(tempPath[i]);
                }
                tempPath.ReturnToSharedPool();
            }
            
            CollectionPool<GenericMinMap<TNode>>.Shared.Return(openList);
            closedList.ReturnToSharedPool();
            neighbors.ReturnToSharedPool();
            
            return success;
        }
    }
}