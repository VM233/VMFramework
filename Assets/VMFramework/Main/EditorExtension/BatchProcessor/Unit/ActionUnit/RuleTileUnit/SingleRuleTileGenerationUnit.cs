#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core.Editor;

namespace VMFramework.Editor.BatchProcessor
{
    [UnitSettings(UnitPriority.Super)]
    public class SingleRuleTileGenerationUnit : SingleButtonBatchProcessorUnit
    {
        protected override string ProcessButtonName => "Generate Single Rule Tile";
        
        [SerializeField]
        private bool batchAllSprites;
        
        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            int spritesCount = selectedObjects.Count(obj => obj is Sprite);
            
            return spritesCount > 0;
        }

        protected override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            if (batchAllSprites)
            {
                var sprites = selectedObjects.Where(obj => obj is Sprite).Cast<Sprite>().ToList();

                if (sprites.Count == 0)
                {
                    return selectedObjects;
                }
                
                var firstSprite = sprites.First();

                if (RuleTileUnitUtility.TryGetRuleTilePath(firstSprite, out var assetPath))
                {
                    var ruleTile = assetPath.CreateScriptableObjectAsset<RuleTile>();
                
                    ruleTile.m_DefaultSprite = firstSprite;
                
                    ruleTile.m_TilingRules.Add(new RuleTile.TilingRule()
                    {
                        m_Sprites = sprites.ToArray(),
                        m_Output = RuleTile.TilingRuleOutput.OutputSprite.Random
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

                    if (RuleTileUnitUtility.TryGetRuleTilePath(sprite, out var assetPath) == false)
                    {
                        continue;
                    }

                    var ruleTile = assetPath.CreateScriptableObjectAsset<RuleTile>();
                
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