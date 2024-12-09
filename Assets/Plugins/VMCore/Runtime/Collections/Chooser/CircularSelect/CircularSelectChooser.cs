using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace VMFramework.Core
{
    public sealed partial class CircularSelectChooser<TItem> : IChooser<TItem>
    {
        public bool PingPong { get; }
        
        public int StartCircularIndex { get; }

        public int CurrentCircularIndex { get; private set; }

        public int CurrentCircularTimes { get; private set; } = 1;

        public bool LoopForward { get; private set; } = true;

        private readonly CircularSelectItem<TItem>[] items;

        public IReadOnlyList<CircularSelectItem<TItem>> CircularItems => items;

        public CircularSelectChooser(CircularSelectItem<TItem>[] items, bool pingPong = false,
            int startCircularIndex = 0)
        {
            this.items = items;
            this.PingPong = pingPong;

            if (startCircularIndex >= items.Length)
            {
                Debug.LogWarning(
                    $"{nameof(startCircularIndex)} : {startCircularIndex} is greater than or equal to " +
                    $"the number of items in the {nameof(CircularSelectChooser<TItem>)}!");
            }

            this.StartCircularIndex = startCircularIndex.Clamp(0, items.Length - 1);
            CurrentCircularIndex = this.StartCircularIndex;

            if (this.items.Length == 0)
            {
                Debug.LogError($"{nameof(CircularSelectChooser<TItem>)} has no items!");
                return;
            }

            if (this.items.Length == 1 && pingPong)
            {
                Debug.LogWarning(
                    $"{nameof(CircularSelectChooser<TItem>)} has only one item and ping-pong is enabled!");
            }
        }

        public CircularSelectChooser(IEnumerable<CircularSelectItem<TItem>> items, bool pingPong = false,
            int startCircularIndex = 0) : this(items.ToArray(), pingPong, startCircularIndex)
        {
        }

        public CircularSelectChooser(IEnumerable<TItem> items, bool pingPong = false, int startCircularIndex = 0)
            : this(items.Select(item => new CircularSelectItem<TItem>(item)), pingPong, startCircularIndex)
        {
        }

        public CircularSelectChooser(IEnumerable<ICircularSelectItem<TItem>> items, bool pingPong = false,
            int startCircularIndex = 0) : this(
            items.Select(item => new CircularSelectItem<TItem>(item.Value, item.Times)), pingPong,
            startCircularIndex)
        {
        }

        public void ResetChooser()
        {
            CurrentCircularIndex = StartCircularIndex;
            CurrentCircularTimes = 1;
            LoopForward = true;
        }

        public TItem GetRandomItem(Random random)
        {
            if (items.Length == 0)
            {
                return default;
            }

            var item = items[CurrentCircularIndex];

            if (PingPong == false)
            {
                CurrentCircularTimes++;
                if (CurrentCircularTimes > item.times)
                {
                    CurrentCircularTimes = 1;
                    CurrentCircularIndex++;

                    if (CurrentCircularIndex >= items.Length)
                    {
                        CurrentCircularIndex = 0;
                    }
                }
            }
            else
            {
                CurrentCircularTimes++;
                if (CurrentCircularTimes > item.times)
                {
                    CurrentCircularTimes = 1;

                    if (LoopForward)
                    {
                        CurrentCircularIndex++;

                        if (CurrentCircularIndex >= items.Length)
                        {
                            CurrentCircularIndex = items.Length - 2;
                            LoopForward = false;
                        }
                    }
                    else
                    {
                        if (CurrentCircularIndex <= 0)
                        {
                            CurrentCircularIndex++;
                            LoopForward = true;
                        }
                        else
                        {
                            CurrentCircularIndex--;
                        }
                    }
                }
            }

            return item.value;
        }
    }
}