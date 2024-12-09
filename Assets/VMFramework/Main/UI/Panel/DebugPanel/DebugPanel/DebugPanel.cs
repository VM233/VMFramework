using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine.Localization;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;
using VMFramework.Timers;

namespace VMFramework.UI
{
    public sealed partial class DebugPanel : UIToolkitPanel, ITimer
    {
        public struct DebugEntryInfo
        {
            public IconLabelVisualElement iconLabel;

            [ShowInInspector]
            private bool Display => iconLabel.style.display.value == DisplayStyle.Flex;
        }
        
        private static float UpdateInterval => UISetting.DebugPanelGeneralSetting.updateInterval;

        private DebugPanelConfig DebugPanelConfig => (DebugPanelConfig)GamePrefab;

        [ShowInInspector]
        private VisualElement leftContainer;

        [ShowInInspector]
        private VisualElement rightContainer;

        [ShowInInspector]
        private VisualTreeAsset debugEntryPrototype;

        [ShowInInspector]
        private List<(IDebugEntry debugEntry, DebugEntryInfo info)> debugEntryInfos = new();

        protected override void OnOpen(IUIPanel source)
        {
            base.OnOpen(source);
            
            leftContainer = RootVisualElement.Q(DebugPanelConfig.leftContainerVisualElementName);
            rightContainer = RootVisualElement.Q(DebugPanelConfig.rightContainerVisualElementName);

            leftContainer.AssertIsNotNull(nameof(leftContainer));

            rightContainer.AssertIsNotNull(nameof(rightContainer));

            debugEntryInfos.Clear();

            foreach (var debugEntry in GamePrefabManager.GetAllActiveGamePrefabs<IDebugEntry>())
            {
                AddEntry(debugEntry);
            }
            
            TimerManager.Add(this, UpdateInterval);
        }

        protected override void OnClose()
        {
            base.OnClose();

            if (TimerManager.Contains(this))
            {
                TimerManager.Stop(this);
            }
        }

        void ITimer.OnTimed()
        {
            foreach (var (debugEntry, info) in debugEntryInfos)
            {
                if (debugEntry.ShouldDisplay())
                {
                    info.iconLabel.style.display = DisplayStyle.Flex;
                    info.iconLabel.Label.text = debugEntry.GetText();
                }
                else
                {
                    info.iconLabel.style.display = DisplayStyle.None;
                }
            }
            
            TimerManager.Add(this, UpdateInterval);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddEntry(IDebugEntry debugEntry)
        {
            var container = debugEntry.Position switch
            {
                LeftRightDirection.Left => leftContainer,
                LeftRightDirection.Right => rightContainer,
                _ => throw new ArgumentOutOfRangeException()
            };

            var debugEntryVisualElement = new IconLabelVisualElement
            {
                Label =
                {
                    text = ""
                }
            };

            AddVisualElement(container, debugEntryVisualElement);

            debugEntryInfos.Add((debugEntry, new DebugEntryInfo
            {
                iconLabel = debugEntryVisualElement
            }));
        }

        protected override void OnCurrentLanguageChanged(Locale currentLocale)
        {
            base.OnCurrentLanguageChanged(currentLocale);
            
            
        }
        
        #region Priority Queue Node

        double IGenericPriorityQueueNode<double>.Priority { get; set; }

        int IGenericPriorityQueueNode<double>.QueueIndex { get; set; }

        long IGenericPriorityQueueNode<double>.InsertionIndex { get; set; }

        #endregion
    }
}