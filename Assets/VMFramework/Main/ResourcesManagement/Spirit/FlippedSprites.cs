using System;
using System.Runtime.CompilerServices;
using EnumsNET;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.ResourcesManagement
{
    public readonly struct FlippedSprites
    {
        public readonly FlipType2D existedFlipType;
        public readonly Sprite nonFlipped;
        public readonly Sprite xFlipped;
        public readonly Sprite yFlipped;
        public readonly Sprite xyFlipped;

        public FlippedSprites(Sprite nonFlipped, Sprite xFlipped, Sprite yFlipped, Sprite xyFlipped,
            FlipType2D existedFlipType)
        {
            this.existedFlipType = existedFlipType;
            this.nonFlipped = nonFlipped;
            this.xFlipped = xFlipped;
            this.yFlipped = yFlipped;
            this.xyFlipped = xyFlipped;
        }
        
        public FlippedSprites(Sprite sprite, FlipType2D flipType)
        {
            switch (flipType)
            {
                case FlipType2D.NonFlipped:
                    nonFlipped = sprite;
                    xFlipped = null;
                    yFlipped = null;
                    xyFlipped = null;
                    existedFlipType = FlipType2D.NonFlipped;
                    break;
                case FlipType2D.X:
                    nonFlipped = null;
                    xFlipped = sprite;
                    yFlipped = null;
                    xyFlipped = null;
                    existedFlipType = FlipType2D.X;
                    break;
                case FlipType2D.Y:
                    nonFlipped = null;
                    xFlipped = null;
                    yFlipped = sprite;
                    xyFlipped = null;
                    existedFlipType = FlipType2D.Y;
                    break;
                case FlipType2D.XY:
                    nonFlipped = null;
                    xFlipped = null;
                    yFlipped = null;
                    xyFlipped = sprite;
                    existedFlipType = FlipType2D.XY;
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(FlipType2D)} value. ({flipType})");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetSprite(FlipType2D flipType, out Sprite sprite)
        {
            sprite = flipType switch
            {
                FlipType2D.NonFlipped => nonFlipped,
                FlipType2D.X => xFlipped,
                FlipType2D.Y => yFlipped,
                FlipType2D.XY => xyFlipped,
                _ => throw new ArgumentException($"Invalid {nameof(FlipType2D)} value. ({flipType})")
            };
            
            return existedFlipType.HasAllFlags(flipType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FlippedSprites AddSprite(Sprite sprite, FlipType2D flipType)
        {
            flipType.AssertIsSingleFlag(nameof(flipType));
            
            var newFlipType = existedFlipType | flipType;

            if (newFlipType == existedFlipType)
            {
                throw new ArgumentException(
                    $"FlipType {flipType} already exists in this {nameof(FlippedSprites)} instance.");
            }

            return flipType switch
            {
                FlipType2D.NonFlipped => new(sprite, xFlipped, yFlipped, xyFlipped, newFlipType),
                FlipType2D.X => new(nonFlipped, sprite, yFlipped, xyFlipped, newFlipType),
                FlipType2D.Y => new(nonFlipped, xFlipped, sprite, xyFlipped, newFlipType),
                FlipType2D.XY => new(nonFlipped, xFlipped, yFlipped, sprite, newFlipType),
                _ => throw new ArgumentException($"Invalid {nameof(FlipType2D)} value. ({flipType})")
            };
        }
    }
}