using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class PairSelectionKeyAdder : PanelModifier
    {
        public enum SelectionMode
        {
            Click,
            Hover,
            HoveringOnly
        }
        
        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName]
        [IsNotNullOrEmpty]
        public string bindObjectsName;

        [TitleGroup(ComponentNames.CONFIG)]
        public SelectionMode selectionMode = SelectionMode.Hover;

        [TitleGroup(ComponentNames.CONFIG)]
        [VisualElementPathSettings(IsFromLocalProvider = true)]
        [IsNotNullOrEmpty]
        public VisualElementPath targetElementPath = new();

        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName]
        [IsNotNullOrEmpty]
        public List<string> targetBindObjectsNames = new();

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly Dictionary<object, VisualElement> targetElements = new();

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly Dictionary<VisualElement, object> keys = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            this.UIToolkitPanel().BindVisualElementsManager.OnBindVisualElementChanged += OnBindVisualElementChanged;
        }

        protected virtual void OnBindVisualElementChanged(string bindName, object bindObject,
            VisualElement visualElement, bool added)
        {
            if (bindName != bindObjectsName)
            {
                return;
            }
            
            if (added)
            {
                var targetElement = targetElementPath.MandatoryQuery(visualElement, nameof(targetElementPath));

                if (selectionMode is SelectionMode.Click)
                {
                    targetElement.RegisterCallback<MouseUpEvent>(OnElementMouseUp);
                }
                else if (selectionMode is SelectionMode.Hover)
                {
                    targetElement.RegisterCallback<MouseEnterEvent>(OnElementMouseEnter);
                }
                else if (selectionMode is SelectionMode.HoveringOnly)
                {
                    targetElement.RegisterCallback<MouseEnterEvent>(OnElementMouseEnter);
                    targetElement.RegisterCallback<MouseLeaveEvent>(OnElementMouseLeave);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(selectionMode), selectionMode, null);
                }

                keys.Add(targetElement, bindObject);
                targetElements.Add(bindObject, targetElement);
            }
            else
            {
                if (targetElements.Remove(bindObject, out var targetElement))
                {
                    keys.Remove(targetElement);

                    if (selectionMode is SelectionMode.Click)
                    {
                        targetElement.UnregisterCallback<MouseUpEvent>(OnElementMouseUp);
                    }
                    else if (selectionMode is SelectionMode.Hover)
                    {
                        targetElement.UnregisterCallback<MouseEnterEvent>(OnElementMouseEnter);
                    }
                    else if (selectionMode is SelectionMode.HoveringOnly)
                    {
                        targetElement.UnregisterCallback<MouseEnterEvent>(OnElementMouseEnter);
                        targetElement.UnregisterCallback<MouseLeaveEvent>(OnElementMouseLeave);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException(nameof(selectionMode), selectionMode, null);
                    }

                    targetElement.RemoveFromHierarchy();
                }

                foreach (var targetBinderObjectsName in targetBindObjectsNames)
                {
                    Panel.BindObjectsManager.RemoveObject(targetBinderObjectsName, bindObject);
                }
            }
        }

        protected virtual void OnElementMouseUp(MouseUpEvent evt)
        {
            var targetElement = (VisualElement)evt.currentTarget;
            Select(targetElement);
        }

        protected virtual void OnElementMouseEnter(MouseEnterEvent evt)
        {
            var targetElement = (VisualElement)evt.currentTarget;
            Select(targetElement);
        }

        protected virtual void OnElementMouseLeave(MouseLeaveEvent evt)
        {
            var targetElement = (VisualElement)evt.currentTarget;
            Deselect(targetElement);
        }

        protected virtual void Select(VisualElement targetElement)
        {
            var key = keys[targetElement];
            if (key != null)
            {
                foreach (var targetBinderObjectsName in targetBindObjectsNames)
                {
                    Panel.BindObjectsManager.AddObject(targetBinderObjectsName, key);
                }
            }
        }

        protected virtual void Deselect(VisualElement targetElement)
        {
            var key = keys[targetElement];
            if (key != null)
            {
                foreach (var targetBinderObjectsName in targetBindObjectsNames)
                {
                    Panel.BindObjectsManager.RemoveObject(targetBinderObjectsName, key);
                }
            }
        }
    }
}