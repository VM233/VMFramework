#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.Core.Pools;
using VMFramework.OdinExtensions;

namespace VMFramework.Editor.BatchProcessor
{
    [UnitSettings(UnitPriority.Super)]
    public sealed class RuleTile47GenerationUnit : SingleButtonBatchProcessorUnit
    {
        protected override string ProcessButtonName => "Generate RuleTile 47";

        [DerivedType(typeof(RuleTile))]
        [SerializeField]
        private Type ruleTileType = typeof(RuleTile);
        
        [SerializeField]
        private RuleTile existingRuleTile;
        
        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            int spritesCount = selectedObjects.Count(obj => obj is Sprite);
            
            return spritesCount == 47;
        }
        
        protected override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            var sprites = ListPool<Sprite>.Default.Get();
            sprites.Clear();
            
            foreach (var obj in selectedObjects)
            {
                if (obj is Sprite sprite)
                {
                    sprites.Add(sprite);
                }
            }
            
            RuleTile ruleTile;

            if (existingRuleTile == null)
            {
                var firstSprite = sprites[0];
                
                var assetPath = RuleTileUnitUtility.GetRuleTilePath(firstSprite);

                ruleTile = (RuleTile)ruleTileType.CreateScriptableObjectAsset(assetPath);
                
                ruleTile.m_DefaultSprite = sprites[15];
            }
            else
            {
                ruleTile = existingRuleTile;
                ruleTile.m_TilingRules.Clear();

                if (ruleTile.m_DefaultSprite == null)
                {
                    ruleTile.m_DefaultSprite = sprites[15];
                }
            }

            var corners = sprites.Get(0, 2, 10, 8);

            var cornersNeighbors = new List<RuleTileNeighborType>()
            {
                RuleTileNeighborType.None,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.None,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.None,
                RuleTileNeighborType.NotThis,
            };

            foreach (var corner in corners)
            {
                GenerateTilingRule(ruleTile, cornersNeighbors, corner);
                
                cornersNeighbors.Rotate(2);
            }

            var edges = sprites.Get(1, 6, 9, 4);

            var edgesNeighbors = new List<RuleTileNeighborType>()
            {
                RuleTileNeighborType.None,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.None,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
            };

            foreach (var edge in edges)
            {
                GenerateTilingRule(ruleTile, edgesNeighbors, edge);
                
                edgesNeighbors.Rotate(2);
            }

            var singleInnerCorners = sprites.Get(16, 17, 19, 18);

            var singleInnerCornersNeighbors = new List<RuleTileNeighborType>()
            {
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
            };

            foreach (var innerCorner in singleInnerCorners)
            {
                GenerateTilingRule(ruleTile, singleInnerCornersNeighbors, innerCorner);
                
                singleInnerCornersNeighbors.Rotate(2);
            }

            var doubleInnerCorners = sprites.Get(20, 23, 21, 22);

            var doubleInnerCornersNeighbors = new List<RuleTileNeighborType>()
            {
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This
            };
            
            foreach (var innerCorner in doubleInnerCorners)
            {
                GenerateTilingRule(ruleTile, doubleInnerCornersNeighbors, innerCorner);
                
                doubleInnerCornersNeighbors.Rotate(2);
            }

            var tripleInnerCorners = sprites.Get(24, 27, 26, 25);

            var tripleInnerCornersNeighbors = new List<RuleTileNeighborType>()
            {
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This
            };
            
            foreach (var innerCorner in tripleInnerCorners)
            {
                GenerateTilingRule(ruleTile, tripleInnerCornersNeighbors, innerCorner);
                
                tripleInnerCornersNeighbors.Rotate(2);
            }

            var singleInnerCornerDoubleEdges = sprites.Get(28, 29, 31, 30);

            var singleInnerCornerDoubleEdgesNeighbors = new List<RuleTileNeighborType>()
            {
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This,
                RuleTileNeighborType.None,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.None,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.None,
                RuleTileNeighborType.This
            };

            foreach (var corner in singleInnerCornerDoubleEdges)
            {
                GenerateTilingRule(ruleTile, singleInnerCornerDoubleEdgesNeighbors, corner);
                
                singleInnerCornerDoubleEdgesNeighbors.Rotate(2);
            }

            var doubleInnerCornerSingleEdges = sprites.Get(32, 35, 33, 34);

            var doubleInnerCornerSingleEdgesNeighbors = new List<RuleTileNeighborType>()
            {
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This,
                RuleTileNeighborType.None,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.None,
                RuleTileNeighborType.This,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This
            };
            
            foreach (var corner in doubleInnerCornerSingleEdges)
            {
                GenerateTilingRule(ruleTile, doubleInnerCornerSingleEdgesNeighbors, corner);
                
                doubleInnerCornerSingleEdgesNeighbors.Rotate(2);
            }

            var type1SingleInnerCornerSingleEdges = sprites.Get(36, 41, 39, 42);

            var type1SingleInnerCornerSingleEdgesNeighbors = new List<RuleTileNeighborType>()
            {
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This,
                RuleTileNeighborType.None,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.None,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This
            };
            
            foreach (var corner in type1SingleInnerCornerSingleEdges)
            {
                GenerateTilingRule(ruleTile, type1SingleInnerCornerSingleEdgesNeighbors, corner);
                
                type1SingleInnerCornerSingleEdgesNeighbors.Rotate(2);
            }

            var type2SingleInnerCornerSingleEdges = sprites.Get(40, 37, 43, 38);

            var type2SingleInnerCornerSingleEdgesNeighbors = new List<RuleTileNeighborType>()
            {
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.None,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.None,
                RuleTileNeighborType.This
            };
            
            foreach (var corner in type2SingleInnerCornerSingleEdges)
            {
                GenerateTilingRule(ruleTile, type2SingleInnerCornerSingleEdgesNeighbors, corner);
                
                type2SingleInnerCornerSingleEdgesNeighbors.Rotate(2);
            }

            var diagonalInnerCorners = sprites.Get(44, 45);

            var diagonalInnerCornersNeighbors = new List<RuleTileNeighborType>()
            {
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This
            };
            
            foreach (var corner in diagonalInnerCorners)
            {
                GenerateTilingRule(ruleTile, diagonalInnerCornersNeighbors, corner);
                
                diagonalInnerCornersNeighbors.Rotate(2);
            }

            var fourDiagonalInnerCorner = sprites[46];
            var fourDiagonalInnerCornerNeighbors = new List<RuleTileNeighborType>()
            {
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This
            };
            
            GenerateTilingRule(ruleTile, fourDiagonalInnerCornerNeighbors, fourDiagonalInnerCorner);
            
            var tips = sprites.Get(3, 14, 11, 12);

            var tipsNeighbors = new List<RuleTileNeighborType>()
            {
                RuleTileNeighborType.None,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.None,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.None,
                RuleTileNeighborType.This,
                RuleTileNeighborType.None,
                RuleTileNeighborType.NotThis,
            };
            
            foreach (var tip in tips)
            {
                GenerateTilingRule(ruleTile, tipsNeighbors, tip);
                
                tipsNeighbors.Rotate(2);
            }
            
            var bodies = sprites.Get(7, 13);

            var bodiesNeighbors = new List<RuleTileNeighborType>()
            {
                RuleTileNeighborType.None,
                RuleTileNeighborType.This,
                RuleTileNeighborType.None,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.None,
                RuleTileNeighborType.This,
                RuleTileNeighborType.None,
                RuleTileNeighborType.NotThis,
            };
            
            foreach (var body in bodies)
            {
                var rule = GenerateTilingRule(bodiesNeighbors, body);
                ruleTile.m_TilingRules.Add(rule);
                
                bodiesNeighbors.Rotate(2);
            }

            var center = sprites[5];
            
            var centerNeighbors = new List<RuleTileNeighborType>()
            {
                RuleTileNeighborType.None,
                RuleTileNeighborType.This,
                RuleTileNeighborType.None,
                RuleTileNeighborType.This,
                RuleTileNeighborType.None,
                RuleTileNeighborType.This,
                RuleTileNeighborType.None,
                RuleTileNeighborType.This
            };
            
            GenerateTilingRule(ruleTile, centerNeighbors, center);

            var single = sprites[15];
            
            var singleNeighbors = new List<RuleTileNeighborType>()
            {
                RuleTileNeighborType.None,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.None,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.None,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.None,
                RuleTileNeighborType.NotThis
            };
            
            GenerateTilingRule(ruleTile, singleNeighbors, single);
            
            sprites.ReturnToDefaultPool();
            
            ruleTile.EnforceSave();

            return selectedObjects;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GenerateTilingRule(RuleTile ruleTile, IList<RuleTileNeighborType> neighborTypes, Sprite sprite)
        {
            var tilingRule = GenerateTilingRule(neighborTypes, sprite);
            ruleTile.m_TilingRules.Add(tilingRule);
        }

        private readonly List<Vector3Int> allPositions = new()
        {
            new(-1, 1, 0),
            new(0, 1, 0),
            new(1, 1, 0),
            new(1, 0, 0),
            new(1, -1, 0),
            new(0, -1, 0),
            new(-1, -1, 0),
            new(-1, 0, 0),
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private RuleTile.TilingRule GenerateTilingRule(IList<RuleTileNeighborType> neighborTypes, Sprite sprite)
        {
            var neighborPositions = new List<Vector3Int>();
            var neighbors = new List<int>();

            for (int i = 0; i < neighborTypes.Count; i++)
            {
                if (neighborTypes[i] == RuleTileNeighborType.None)
                {
                    continue;
                }
                
                var position = allPositions[i];
                neighborPositions.Add(position);
                neighbors.Add((int)neighborTypes[i]);
            }
            
            var tilingRule = new RuleTile.TilingRule
            {
                m_NeighborPositions = neighborPositions,
                m_Neighbors = neighbors,
                m_Sprites = new[] { sprite }
            };
            
            return tilingRule;
        }
    }
}
#endif