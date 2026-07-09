using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.OdinExtensions;
using VMFramework.Properties;

namespace VMFramework.UI
{
    public class MultipleBindObjectsAdder : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName]
        [IsNotNullOrEmpty]
        public List<string> sourceBindObjectsNames = new();

        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName(AutoFirstThisGameObject = true)]
        [IsNotNullOrEmpty]
        public string targetBindObjectsName;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool usePriorityOrder = false;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly DataTokenProperty<string, object> bindObjectsProperty = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.BindObjectsManager.OnBindObjectChanged += OnBindObjectChanged;

            bindObjectsProperty.SetOwner(this);
            bindObjectsProperty.Clear();
            bindObjectsProperty.OnDataChanged += OnDataChanged;
        }

        protected virtual void OnBindObjectChanged(string bindName, object bindObject, object parentObject, bool added)
        {
            if (sourceBindObjectsNames.Contains(bindName) == false)
            {
                return;
            }

            if (added)
            {
                bindObjectsProperty.AddData(bindName, bindObject);

                if (usePriorityOrder)
                {
                    bindObjectsProperty.Clear();

                    foreach (var sourceBindObjectsName in sourceBindObjectsNames)
                    {
                        foreach (var sourceBindObject in Panel.BindObjectsManager.GetObjects(sourceBindObjectsName))
                        {
                            bindObjectsProperty.AddData(sourceBindObjectsName, sourceBindObject);
                        }
                    }
                }
            }
            else
            {
                bindObjectsProperty.RemoveData(bindName, bindObject);
            }
        }

        protected virtual void OnDataChanged(IReadOnlyProperty property, object data, bool added)
        {
            if (added)
            {
                Panel.BindObjectsManager.AddObject(targetBindObjectsName, data);
            }
            else
            {
                Panel.BindObjectsManager.RemoveObject(targetBindObjectsName, data);
            }
        }
    }
}