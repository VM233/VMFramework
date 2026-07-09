#if UNITY_EDITOR && ENABLE_TILEMAP
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
    public sealed class RuleTile16GenerationUnit : SingleButtonBatchProcessorUnit
    {
        protected override string ProcessButtonName => "Generate RuleTile 16";
        
        [DerivedType(typeof(RuleTile))]
        [SerializeField]
        private Type ruleTileType = typeof(RuleTile);

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            int spritesCount = selectedObjects.Count(obj => obj is Sprite);
            
            return spritesCount == 16;
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
            
            var firstSprite = sprites[0];
            var assetPath = RuleTileUnitUtility.GetRuleTilePath(firstSprite);

            var ruleTile = (RuleTile)ruleTileType.CreateScriptableObjectAsset(assetPath);

            ruleTile.m_DefaultSprite = sprites[^1];

            var fourCorners = sprites.Get(0, 2, 10, 8);

            var fourCornersNeighbors = new List<RuleTileNeighborType>()
            {
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This
            };

            foreach (var corner in fourCorners)
            {
                GenerateTilingRule(ruleTile, fourCornersNeighbors, corner);
                
                fourCornersNeighbors.Rotate(1);
            }

            var fourEdges = sprites.Get(1, 6, 9, 4);

            var fourEdgesNeighbors = new List<RuleTileNeighborType>()
            {
                RuleTileNeighborType.This,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This
            };

            foreach (var edge in fourEdges)
            {
                GenerateTilingRule(ruleTile, fourEdgesNeighbors, edge);
                
                fourEdgesNeighbors.Rotate(1);
            }

            var fourTips = sprites.Get(3, 14, 11, 12);

            var fourTipsNeighbors = new List<RuleTileNeighborType>()
            {
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This
            };
            
            foreach (var tip in fourTips)
            {
                GenerateTilingRule(ruleTile, fourTipsNeighbors, tip);
                
                fourTipsNeighbors.Rotate(1);
            }
            
            var twoBodies = sprites.Get(7, 13);

            var twoBodiesNeighbors = new List<RuleTileNeighborType>()
            {
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.This
            };
            
            foreach (var body in twoBodies)
            {
                var rule = GenerateTilingRule(twoBodiesNeighbors, body);
                ruleTile.m_TilingRules.Add(rule);
                
                twoBodiesNeighbors.Rotate(1);
            }

            var center = sprites[5];
            
            var centerNeighbors = new List<RuleTileNeighborType>()
            {
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This,
                RuleTileNeighborType.This
            };
            
            GenerateTilingRule(ruleTile, centerNeighbors, center);

            var single = sprites[15];
            
            var singleNeighbors = new List<RuleTileNeighborType>()
            {
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.NotThis,
                RuleTileNeighborType.NotThis,
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private RuleTile.TilingRule GenerateTilingRule(IList<RuleTileNeighborType> neighborTypes, Sprite sprite)
        {
            var tilingRule = new RuleTile.TilingRule
            {
                m_NeighborPositions = new()
                {
                    new Vector3Int(-1, 0, 0),
                    new Vector3Int(0, 1, 0),
                    new Vector3Int(1, 0, 0),
                    new Vector3Int(0, -1, 0)
                },
                m_Neighbors = new()
                {
                    (int)neighborTypes[0],
                    (int)neighborTypes[1],
                    (int)neighborTypes[2],
                    (int)neighborTypes[3]
                },
                m_Sprites = new[] { sprite }
            };
            
            return tilingRule;
        }
    }
}
#endif