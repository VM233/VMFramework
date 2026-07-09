using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class PairEntryAdder : PanelModifier, IVisualElementGenerator
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName]
        [IsNotNullOrEmpty]
        public string bindObjectsName;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool useParentObject = false;

        [TitleGroup(ComponentNames.CONFIG)]
        [HideIf(nameof(useParentObject))]
        [IsNotNullOrEmpty]
        public VisualElementPath containerPath = new();

        [TitleGroup(ComponentNames.CONFIG)]
        [ShowIf(nameof(useParentObject))]
        [BindObjectsName]
        [IsNotNullOrEmpty]
        public string parentBindObjectsName;

        [TitleGroup(ComponentNames.CONFIG)]
        [ShowIf(nameof(useParentObject))]
        [VisualElementPathSettings(IsFromLocalProvider = true, MustFromParent = true)]
        [IsNotNullOrEmpty]
        public VisualElementPath parentContainerPath = new();

        [TitleGroup(ComponentNames.CONFIG)]
        [Required]
        public VisualTreeAsset entryAsset;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool clearContainerOnOpen = true;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool ignorePicking = false;

        public event IVisualElementGenerator.GenerateVisualElementHandler OnGenerateVisualElement;

        protected VisualElement container;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            if (useParentObject)
            {
                var generator = transform.parent.GetComponentInParent<IVisualElementGenerator>();
                generator.OnGenerateVisualElement += OnRootGenerate;
            }
            else
            {
                this.UIToolkitPanel().OnGenerateVisualElement += OnRootGenerate;
            }

            Panel.OnOpen += OnOpen;
            Panel.BindObjectsManager.OnBindObjectPreChanged += OnBindObjectPreChanged;
        }

        protected virtual void OnRootGenerate(IVisualElementGenerator generator, VisualElement root)
        {
            if (clearContainerOnOpen)
            {
                if (useParentObject)
                {
                    var container = parentContainerPath.MandatoryQuery(root, nameof(parentContainerPath));
                    container.Clear();
                }
                else
                {
                    var container = containerPath.MandatoryQuery(root, nameof(containerPath));
                    container.Clear();
                }
            }
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            if (useParentObject == false)
            {
                container = containerPath.MandatoryQuery(this.RootVisualElement(), nameof(containerPath));
            }
        }

        protected virtual void OnBindObjectPreChanged(string bindName, object bindObject, object parentObject,
            bool added)
        {
            if (bindName != bindObjectsName)
            {
                return;
            }

            if (added)
            {
                AddItemEntry(bindObject, parentObject);
            }
            else
            {
                RemoveItemEntry(bindObject, parentObject);
            }
        }

        protected virtual void AddItemEntry(object bindObject, object parentObject)
        {
            var entry = GenerateVisualElement();

            if (ignorePicking)
            {
                entry.SetPickingMode(PickingMode.Ignore);
            }

            if (useParentObject)
            {
                if (this.UIToolkitPanel().BindVisualElementsManager
                    .TryGetVisualElement(parentBindObjectsName, parentObject, out var container))
                {
                    var parentContainer = parentContainerPath.MandatoryQuery(container, nameof(parentContainerPath));
                    parentContainer.Add(entry);
                }
            }
            else
            {
                container.Add(entry);
            }

            this.UIToolkitPanel().BindVisualElementsManager.Add(bindObjectsName, bindObject, entry);
        }

        protected virtual void RemoveItemEntry(object bindObject, object parentObject)
        {
            if (this.UIToolkitPanel().BindVisualElementsManager.Remove(bindObjectsName, bindObject, out var entry))
            {
                entry.RemoveFromHierarchy();
            }
        }

        public virtual VisualElement GenerateVisualElement()
        {
            if (entryAsset == null)
            {
                return null;
            }
            
            var root = entryAsset.CloneTree();
            OnGenerateVisualElement?.Invoke(this, root);
            return root;
        }
    }
}