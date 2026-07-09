using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class PriorityBindObjectsAdder : PanelModifier
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
        [ShowInInspector, DisplayAsString]
        protected string currentBindObjectsName;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.BindObjectsManager.OnBindObjectChanged += OnBindObjectChanged;
        }

        protected virtual void OnBindObjectChanged(string bindName, object bindObject, object parentObject, bool added)
        {
            if (sourceBindObjectsNames.Contains(bindName) == false)
            {
                return;
            }

            if (added)
            {
                if (currentBindObjectsName == bindName)
                {
                    Panel.BindObjectsManager.AddObject(currentBindObjectsName, bindObject);
                    return;
                }

                if (currentBindObjectsName != null)
                {
                    if (IsPriorityHigherThanCurrent(bindName) == false)
                    {
                        return;
                    }

                    Panel.BindObjectsManager.RemoveObjects(targetBindObjectsName,
                        Panel.BindObjectsManager.GetObjects(currentBindObjectsName));
                }

                currentBindObjectsName = bindName;
                Panel.BindObjectsManager.AddObjects(targetBindObjectsName,
                    Panel.BindObjectsManager.GetObjects(currentBindObjectsName));
            }
            else
            {
                if (currentBindObjectsName == bindName)
                {
                    Panel.BindObjectsManager.RemoveObject(targetBindObjectsName, bindObject);

                    if (Panel.BindObjectsManager.GetObjects(currentBindObjectsName).Count <= 0)
                    {
                        currentBindObjectsName = null;
                    }
                }
            }
        }

        protected virtual bool IsPriorityHigherThanCurrent(string bindName)
        {
            return sourceBindObjectsNames.IndexOf(bindName) < sourceBindObjectsNames.IndexOf(currentBindObjectsName);
        }
    }
}