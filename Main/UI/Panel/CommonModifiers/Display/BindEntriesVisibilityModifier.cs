using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class BindEntriesVisibilityModifier : PanelModifier
    {
        public enum Condition
        {
            AnyEmpty,
            AllEmpty,
            AnyNotEmpty,
            AllNotEmpty,
        }

        [TitleGroup(ComponentNames.CONFIG)]
        [EnumToggleButtons]
        public Condition condition = Condition.AllEmpty;
        
        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName]
        [IsNotNullOrEmpty]
        public List<string> bindObjectsNames = new();
        
        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        public List<VisualElementPath> containerPaths = new();

        [TitleGroup(ComponentNames.CONFIG)]
        public bool display = false;

        protected readonly List<VisualElement> containers = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            Panel.OnOpen += OnOpen;
            Panel.BindObjectsManager.OnBindObjectChanged += OnBindObjectChanged;
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            containers.Clear();
            containers.AddRange(containerPaths.MandatoryQuery(this.RootVisualElement(), nameof(containerPaths)));
            
            Refresh();
        }
        
        protected virtual void OnBindObjectChanged(string bindName, object bindObject, object parentObject, bool added)
        {
            if (bindObjectsNames.Contains(bindName) == false)
            {
                return;
            }
            
            Refresh();
        }

        protected virtual void Refresh()
        {
            bool conditionSatisfied;

            if (condition is Condition.AnyEmpty)
            {
                conditionSatisfied = bindObjectsNames.Any(bindObjectName =>
                    Panel.BindObjectsManager.GetObjects(bindObjectName).Count == 0);
            }
            else if (condition is Condition.AllEmpty)
            {
                conditionSatisfied = bindObjectsNames.All(bindObjectName =>
                    Panel.BindObjectsManager.GetObjects(bindObjectName).Count == 0);
            }
            else if (condition is Condition.AnyNotEmpty)
            {
                conditionSatisfied = bindObjectsNames.Any(bindObjectName =>
                    Panel.BindObjectsManager.GetObjects(bindObjectName).Count > 0);
            }
            else if (condition is Condition.AllNotEmpty)
            {
                conditionSatisfied = bindObjectsNames.All(bindObjectName =>
                    Panel.BindObjectsManager.GetObjects(bindObjectName).Count > 0);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(condition), condition, null);
            }

            foreach (var container in containers)
            {
                container.Display(conditionSatisfied == display);
            }
        }
    }
}