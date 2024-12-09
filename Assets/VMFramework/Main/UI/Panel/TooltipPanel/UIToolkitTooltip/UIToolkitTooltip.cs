using System.Collections.Generic;
using VMFramework.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

namespace VMFramework.UI
{
    public class UIToolkitTooltip : UIToolkitPanel, ITooltip
    {
        protected UIToolkitTracingTooltipConfig TracingTooltipConfig => (UIToolkitTracingTooltipConfig)GamePrefab;
        
        [ShowInInspector]
        protected ITooltipProvider TooltipProvider { get; private set; }
        
        [ShowInInspector]
        protected TooltipOpenInfo CurrentOpenInfo { get; private set; }

        [ShowInInspector] 
        protected Label title, description;

        [ShowInInspector] 
        protected VisualElement propertyContainer;

        [ShowInInspector] 
        private List<TooltipProviderUIToolkitRenderUtility.DynamicPropertyInfo> dynamicPropertyInfos = new();

        [ShowInInspector]
        private Dictionary<string, GroupVisualElement> groupVisualElements = new();

        protected override void OnOpen(IUIPanel source)
        {
            base.OnOpen(source);
            
            title = RootVisualElement.Q<Label>(TracingTooltipConfig.titleLabelName);
            description = RootVisualElement.Q<Label>(TracingTooltipConfig.descriptionLabelName);
            propertyContainer = RootVisualElement.Q(TracingTooltipConfig.propertyContainerName);

            title.AssertIsNotNull(nameof(title));
            description.AssertIsNotNull(nameof(description));
            propertyContainer.AssertIsNotNull(nameof(propertyContainer));

            dynamicPropertyInfos.Clear();

            groupVisualElements.Clear();
            
            propertyContainer.Clear();
            
            var renderResult = TooltipProvider.RenderToVisualElement(title, description,
                propertyContainer, AddVisualElement);

            groupVisualElements = renderResult.groups;
            dynamicPropertyInfos = renderResult.dynamicPropertyInfos;
        }

        protected override void OnClose()
        {
            base.OnClose();
            
            if (TooltipProvider != null)
            {
                if (TooltipProvider.TryGetTooltipBindGlobalEvent(out var gameEvent))
                {
                    gameEvent.OnEnabledChangedEvent -= OnGlobalEventEnabledStateChanged;
                }

                TooltipProvider = null;
            }

            title = null;
            description = null;
        }
        
        private void FixedUpdate()
        {
            if (TooltipProvider == null)
            {
                return;
            }
            
            if (IsOpened)
            {
                if (TooltipProvider.IsDestroyed)
                {
                    this.Close();
                }
            }
            
            if (IsOpened)
            {
                if (dynamicPropertyInfos.Count > 0)
                {
                    foreach (var attributeInfo in dynamicPropertyInfos)
                    {
                        attributeInfo.iconLabel.SetContent(attributeInfo.valueGetter());
                    }
                }
            }
        }

        private void OnGlobalEventEnabledStateChanged(bool previous, bool current)
        {
            if (current == false)
            {
                this.Close();
            }
        }

        public void Open(ITooltipProvider tooltipProvider, IUIPanel source, TooltipOpenInfo info)
        {
            if (this.TooltipProvider == tooltipProvider)
            {
                return;
            }

            if (tooltipProvider.TryGetTooltipBindGlobalEvent(out var gameEvent))
            {
                if (gameEvent.IsEnabled == false)
                {
                    return;
                }

                gameEvent.OnEnabledChangedEvent += OnGlobalEventEnabledStateChanged;
            }

            if (this.TooltipProvider != null)
            {
                if (info.priority < CurrentOpenInfo.priority)
                {
                    return;
                }
            }
            
            this.TooltipProvider = tooltipProvider;
            CurrentOpenInfo = info;

            this.Open(source);
        }

        public void Close(ITooltipProvider tooltipProvider)
        {
            if (this.TooltipProvider == tooltipProvider)
            {
                this.Close();
            }
        }
    }
}
