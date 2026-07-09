#if UNITY_EDITOR && ENABLE_TILEMAP
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.MapExtension;
using VMFramework.OdinExtensions;

namespace VMFramework.Editor.BatchProcessor
{
    [UnitSettings(UnitPriority.Super)]
    public class SingleRuleTileGenerationUnit : SingleButtonBatchProcessorUnit
    {
        public enum BatchMode
        {
            Random,
            Animation,
        }
        
        protected override string ProcessButtonName => "Generate Single Rule Tile";
        
        [DerivedType(typeof(RuleTile))]
        public Type ruleTileType = typeof(GameTagRuleTile);
        
        public bool batchSprites;
        
        [ShowIf(nameof(batchSprites))]
        public BatchMode batchMode;

        [ShowIf(nameof(batchSprites))]
        public bool enableBatchSize;
        
        [ShowIf(nameof(batchSprites))]
        [EnableIf(nameof(enableBatchSize))]
        [MinValue(2)]
        public int batchSize = 2;
        
        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            int spritesCount = selectedObjects.Count(obj => obj is Sprite);
            
            return spritesCount > 0;
        }

        protected override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            if (batchSprites)
            {
                var allSprites = selectedObjects.Where(obj => obj is Sprite).Cast<Sprite>().ToList();

                if (allSprites.Count == 0)
                {
                    return selectedObjects;
                }

                List<List<Sprite>> batchedSprites = new();

                if (enableBatchSize)
                {
                    foreach (var range in allSprites.GetSizeRange().SpiltToRanges(batchSize, StepSplitMode.Forwards))
                    {
                        batchedSprites.Add(new(allSprites.GetRange(range)));
                    }
                }
                else
                {
                    batchedSprites.Add(allSprites);
                }

                foreach (var sprites in batchedSprites)
                {
                    var firstSprite = sprites.First();
                    var assetPath = sprites.GetTargetObjectPath();
                
                    var ruleTile = (RuleTile)ruleTileType.CreateScriptableObjectAsset(assetPath);
                
                    ruleTile.m_DefaultSprite = firstSprite;
                
                    ruleTile.m_TilingRules.Add(new RuleTile.TilingRule()
                    {
                        m_Sprites = sprites.ToArray(),
                        m_Output = batchMode switch
                        {
                            BatchMode.Random => RuleTile.TilingRuleOutput.OutputSprite.Random,
                            BatchMode.Animation => RuleTile.TilingRuleOutput.OutputSprite.Animation,
                            _ => throw new ArgumentOutOfRangeException()
                        }
                    });
                
                    ruleTile.EnforceSave();
                }
            }
            else
            {
                var ruleTiles = new List<RuleTile>();
            
                foreach (var obj in selectedObjects)
                {
                    if (obj is not Sprite sprite)
                    {
                        continue;
                    }

                    var assetPath = RuleTileUnitUtility.GetRuleTilePath(sprite);

                    var ruleTile = (RuleTile)ruleTileType.CreateScriptableObjectAsset(assetPath);
                
                    ruleTile.m_DefaultSprite = sprite;
                
                    ruleTiles.Add(ruleTile);
                }
            
                ruleTiles.EnforceSave();
            }
            
            return selectedObjects;
        }
    }
}
#endif