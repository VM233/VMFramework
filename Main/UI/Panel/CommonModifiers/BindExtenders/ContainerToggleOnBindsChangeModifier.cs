using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class ContainerToggleOnBindsChangeModifier : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName]
        [IsNotNullOrEmpty]
        public string bindObjectsName;

        [TitleGroup(ComponentNames.CONFIG)]
        [VisualElementName]
        public List<string> bindEmptyContainersName;

        [TitleGroup(ComponentNames.CONFIG)]
        [VisualElementName]
        public List<string> bindNonEmptyContainersName;

        protected readonly List<VisualElement> bindEmptyContainers = new();
        protected readonly List<VisualElement> bindNonEmptyContainers = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpen += OnOpen;
            Panel.BindObjectsManager.OnBindObjectChanged += OnBindObjectChanged;
        }

        protected virtual void OnBindObjectChanged(string bindName, object bindObject, object parentObject, bool added)
        {
            if (bindName != bindObjectsName)
            {
                return;
            }

            OnDirty();
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            bindEmptyContainers.Clear();
            bindEmptyContainers.AddRange(this.RootVisualElement()
                .QueryStrictly(bindEmptyContainersName, nameof(bindEmptyContainersName)));

            bindNonEmptyContainers.Clear();
            bindNonEmptyContainers.AddRange(this.RootVisualElement()
                .QueryStrictly(bindNonEmptyContainersName, nameof(bindNonEmptyContainersName)));
        }

        protected virtual void OnDirty()
        {
            if (Panel.BindObjectsManager.GetObjects(bindObjectsName).Count != 0)
            {
                foreach (var container in bindEmptyContainers)
                {
                    container.DisplayNone();
                }

                foreach (var container in bindNonEmptyContainers)
                {
                    container.DisplayFlex();
                }
            }
            else
            {
                foreach (var container in bindEmptyContainers)
                {
                    container.DisplayFlex();
                }

                foreach (var container in bindNonEmptyContainers)
                {
                    container.DisplayNone();
                }
            }
        }
    }
}