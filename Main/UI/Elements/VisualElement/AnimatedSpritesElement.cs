using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core.Linq;

namespace VMFramework.UI
{
    [UxmlElement]
    public partial class AnimatedSpritesElement : VisualElement
    {
        [UxmlAttribute]
        public List<Sprite> Images
        {
            get => images;
            set
            {
                images = value;
                RefreshSchedule();
            }
        }

        [UxmlAttribute]
        public long ImageSwappingInterval
        {
            get => imageSwappingInterval;
            set
            {
                imageSwappingInterval = value;
                RefreshSchedule();
            }
        }

        [UxmlAttribute]
        public long ImageSwappingDuration
        {
            get => imageSwappingDuration;
            set
            {
                imageSwappingDuration = value;
                RefreshSchedule();
            }
        }

        [UxmlAttribute]
        public bool ClearOnLast
        {
            get => clearOnLast;
            set
            {
                clearOnLast = value;
                RefreshSchedule();
            }
        }

        protected List<Sprite> images;
        protected long imageSwappingInterval = 100;
        protected long imageSwappingDuration = 2500;
        protected bool clearOnLast = true;

        protected int currentBackgroundImageIndex = 0;
        protected IVisualElementScheduledItem swapDurationScheduledItem;
        protected IVisualElementScheduledItem swappingScheduledItem;

        public void PlayFromStart()
        {
            currentBackgroundImageIndex = 0;
            RefreshSchedule();
        }

        public virtual void RefreshSchedule()
        {
            if (images.IsNullOrEmpty())
            {
                swapDurationScheduledItem?.Pause();
                swappingScheduledItem?.Pause();
                style.backgroundImage = new StyleBackground(StyleKeyword.Null);
                return;
            }

            if (images.Count == 1)
            {
                swapDurationScheduledItem?.Pause();
                swappingScheduledItem?.Pause();
                style.backgroundImage = new StyleBackground(Images[0]);
                return;
            }

            swapDurationScheduledItem ??= schedule.Execute(SwapDuration);
            swapDurationScheduledItem.Resume();
            swapDurationScheduledItem.Every(imageSwappingDuration);
        }

        protected virtual void SwapDuration(TimerState timerState)
        {
            swappingScheduledItem ??= schedule.Execute(SwapImage);
            swappingScheduledItem.Resume();
            swappingScheduledItem.Every(imageSwappingInterval);
        }

        protected virtual void SwapImage(TimerState timerState)
        {
            Sprite sprite;
            if (clearOnLast == false)
            {
                if (currentBackgroundImageIndex >= images.Count - 1)
                {
                    swappingScheduledItem.Pause();
                    currentBackgroundImageIndex = images.Count - 1;
                }

                sprite = images[currentBackgroundImageIndex];

                currentBackgroundImageIndex = (currentBackgroundImageIndex + 1) % images.Count;
            }
            else
            {
                if (currentBackgroundImageIndex >= images.Count)
                {
                    swappingScheduledItem.Pause();
                    sprite = null;
                    currentBackgroundImageIndex = images.Count;
                }
                else
                {
                    sprite = images[currentBackgroundImageIndex];
                }

                currentBackgroundImageIndex = (currentBackgroundImageIndex + 1) % (images.Count + 1);
            }

            style.backgroundImage = new StyleBackground(sprite);
        }
    }
}