using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.OdinExtensions;
using VMFramework.Properties;

namespace VMFramework.UI
{
    public class TokenBindObjectsAdder : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName]
        [IsNotNullOrEmpty]
        public List<string> sourceBindObjectsNames = new();

        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName(AutoFirstThisGameObject = true)]
        [IsNotNullOrEmpty]
        public string targetBindObjectsName;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly DataTokenProperty<string, object> dataTokenProperty = new();

        protected override void OnInitialize()
        {
            Panel.BindObjectsManager.OnBindObjectChanged += OnBindObjectChanged;

            dataTokenProperty.OnDataChanged += OnDataChanged;
        }

        protected virtual void OnBindObjectChanged(string bindName, object bindObject, object parentObject, bool added)
        {
            if (sourceBindObjectsNames.Contains(bindName) == false)
            {
                return;
            }

            if (added)
            {
                dataTokenProperty.AddData(bindName, bindObject);
            }
            else
            {
                dataTokenProperty.RemoveData(bindName, bindObject);
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