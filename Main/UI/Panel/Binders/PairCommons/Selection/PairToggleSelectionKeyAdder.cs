using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class PairToggleSelectionKeyAdder : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName]
        [IsNotNullOrEmpty]
        public string bindObjectsName;

        [TitleGroup(ComponentNames.CONFIG)]
        [VisualElementPathSettings(typeof(Toggle), IsFromLocalProvider = true)]
        [IsNotNullOrEmpty]
        public VisualElementPath togglePath = new();

        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName]
        [IsNotNullOrEmpty]
        public List<string> targetBindObjectsNames = new();

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly Dictionary<object, Toggle> togglesLookup = new();

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly Dictionary<VisualElement, object> keys = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            this.UIToolkitPanel().BindVisualElementsManager.OnBindVisualElementChanged += OnBindVisualElementChanged;
            this.UIToolkitPanel().BindObjectsManager.OnBindObjectChanged += OnBindObjectChanged;
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
                var toggle = togglePath.MandatoryQuery<Toggle>(visualElement, nameof(togglePath));
                toggle.RegisterValueChangedCallback(OnValueChanged);

                keys.Add(toggle, bindObject);
                togglesLookup.Add(bindObject, toggle);
            }
            else
            {
                if (togglesLookup.Remove(bindObject, out var toggle))
                {
                    keys.Remove(toggle);
                    toggle.UnregisterValueChangedCallback(OnValueChanged);
                    toggle.RemoveFromHierarchy();
                }

                foreach (var targetBinderObjectsName in targetBindObjectsNames)
                {
                    Panel.BindObjectsManager.RemoveObject(targetBinderObjectsName, bindObject);
                }
            }
        }
        
        protected virtual void OnBindObjectChanged(string bindName, object bindObject, object parentObject, bool added)
        {
            if (targetBindObjectsNames.Contains(bindName) == false)
            {
                return;
            }

            if (added)
            {
                if (togglesLookup.TryGetValue(bindObject, out var toggle))
                {
                    toggle.SetValueWithoutNotify(true);
                }
            }
            else
            {
                if (togglesLookup.TryGetValue(bindObject, out var toggle))
                {
                    toggle.SetValueWithoutNotify(false);
                }
            }
        }

        protected virtual void OnValueChanged(ChangeEvent<bool> evt)
        {
            var toggle = (Toggle)evt.currentTarget;
            
            if (evt.newValue)
            {
                Select(toggle);
            }
            else
            {
                Deselect(toggle);
            }
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