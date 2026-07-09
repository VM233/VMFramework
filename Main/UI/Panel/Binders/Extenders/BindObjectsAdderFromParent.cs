using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public abstract class BindObjectsAdderFromParent : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName(AutoFirstParentGameObject = true)]
        [IsNotNullOrEmpty]
        public string sourceBindObjectsName;

        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName(AutoFirstThisGameObject = true)]
        [IsNotNullOrEmpty]
        public string targetBindObjectsName;

        protected readonly Dictionary<object, HashSet<object>> addedObjectsLookup = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.BindObjectsManager.OnBindObjectChanged += OnBindObjectChanged;
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
                    foreach (var targetObject in ProcessBindObject(bindObject))
                    {
                        if (Panel.BindObjectsManager.AddObject(targetBindObjectsName, targetObject,
                                parentObject: bindObject))
                        {
                            var addedObjects = addedObjectsLookup.GetOrCreateFromFactory(bindObject,
                                HashSetPoolFactory<object>.CreateFromDefaultPool);
                            addedObjects.Add(targetObject);
                        }
                    }
                }
            }
            else
            {
                if (addedObjectsLookup.Remove(bindObject, out var addedObjects))
                {
                    foreach (var targetObject in addedObjects)
                    {
                        Panel.BindObjectsManager.RemoveObject(targetBindObjectsName, targetObject, bindObject);
                    }

                    addedObjects.ReturnToDefaultPool();
                }
            }
        }

        protected abstract IEnumerable<object> ProcessBindObject(object bindObject);

        protected virtual bool CanAddObject(object value)
        {
            return true;
        }
    }
}