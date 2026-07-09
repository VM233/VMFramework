using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.OdinExtensions;
using VMFramework.Properties;

namespace VMFramework.UI
{
    public class BindObjectsAdder : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName]
        [IsNotNullOrEmpty]
        public string sourceBindObjectsName;

        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName]
        [IsNotNullOrEmpty]
        public string targetBindObjectsName;
        
        protected readonly HashSetProperty<object> addedObjects = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.BindObjectsManager.OnBindObjectChanged += OnBindObjectChanged;
            addedObjects.OnCollectionChanged += OnAddedObjectsChanged;
        }

        protected virtual void OnBindObjectChanged(string bindName, object bindObject, object parentObject, bool added)
        {
            if (bindName != sourceBindObjectsName)
            {
                return;
            }

            if (added)
            {
                if (CanAddObject(bindObject))
                {
                    addedObjects.Add(bindObject, initial: false);
                }
            }
            else
            {
                addedObjects.Remove(bindObject, initial: false);
            }
        }

        protected virtual void OnAddedObjectsChanged(IReadOnlyProperty property, object value, bool added, bool initial)
        {
            if (added)
            {
                Panel.BindObjectsManager.AddObject(targetBindObjectsName, value);
            }
            else
            {
                Panel.BindObjectsManager.RemoveObject(targetBindObjectsName, value);
            }
        }

        protected virtual bool CanAddObject(object value)
        {
            return true;
        }
    }
}