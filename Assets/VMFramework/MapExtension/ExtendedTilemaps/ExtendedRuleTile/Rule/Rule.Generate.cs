using System.Collections.Generic;

namespace VMFramework.Maps
{
    public partial class Rule
    {
        public IEnumerable<Rule> GenerateRules()
        {
            yield return GetClone(false, false);
            
            if (flipX)
            {
                yield return GetClone(true, false);
            }
            
            if (flipY)
            {
                yield return GetClone(false, true);
            }
            
            if (flipX && flipY)
            {
                yield return GetClone(true, true);
            }
        }
        
        public Rule GetClone(bool flipX, bool flipY)
        {
            var result = new Rule()
            {
                enable = enable,
                enableAnimation = enableAnimation,
                // animationSprites = animationSprites,
                // gap = gap,
                // autoPlayOnStart = autoPlayOnStart,
                upperLeft = upperLeft,
                upper = upper,
                upperRight = upperRight,
                left = left,
                right = right,
                lowerLeft = lowerLeft,
                lower = lower,
                lowerRight = lowerRight,
            };

            if (flipX)
            {
                (result.upperLeft, result.upperRight) = (result.upperRight, result.upperLeft);
                (result.left, result.right) = (result.right, result.left);
                (result.lowerLeft, result.lowerRight) = (result.lowerRight, result.lowerLeft);
            }

            if (flipY)
            {
                (result.upperLeft, result.lowerLeft) = (result.lowerLeft, result.upperLeft);
                (result.upper, result.lower) = (result.lower, result.upper);
                (result.upperRight, result.lowerRight) = (result.lowerRight, result.upperRight);
            }

            result.spriteConfig = new()
            {
                sprite = spriteConfig.sprite.GetFlipChooserConfig(flipX, flipY),
            };

            return result;
        }
    }
}