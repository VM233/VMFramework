using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.OdinExtensions;
using VMFramework.Properties;

namespace VMFramework.UI
{
    public abstract class PipelinedBindObjectsAdder<TValue> : PipelinedBindObjectsAdder
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [HideIf(nameof(IsComponents))]
        public bool invert = false;

        public virtual bool IsComponentType => true;

        public virtual bool IsComponents => false;

        public override void ProcessTargets(IReadOnlyCollection<object> targets, ICollection<object> results)
        {
            base.ProcessTargets(targets, results);

            foreach (var target in targets)
            {
                if (IsComponentType)
                {
                    if (IsComponents)
                    {
                        if (target.TryAsGameObject(out var targetGameObject))
                        {
                            foreach (var component in targetGameObject.GetComponentsInChildren<TValue>())
                            {
                                results.Add(component);
                            }
                        }
                        else
                        {
                            if (target is TValue value)
                            {
                                results.Add(value);
                            }
                        }
                    }
                    else
                    {
                        if (invert)
                        {
                            if (target.TryGetComponentOrAs(out TValue _) == false)
                            {
                                results.Add(target);
                            }
                        }
                        else
                        {
                            if (target.TryGetComponentOrAs(out TValue value))
                            {
                                results.Add(value);
                            }
                        }
                    }
                }
                else
                {
                    if (invert)
                    {
                        if (target is not TValue)
                        {
                            results.Add(target);
                        }
                    }
                    else
                    {
                        if (target is TValue value)
                        {
                            results.Add(value);
                        }
                    }
                }
            }
        }
    }

    public abstract class PipelinedBindObjectsAdder : PanelModifier, IBindPipelineProvider,
        IFuncTargetsProcessor<object, object>
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName(AutoFirstParentGameObject = true)]
        [IsNotNullOrEmpty]
        public string sourceBindName;

        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName(AutoFirstThisGameObject = true)]
        [IsNotNullOrEmpty]
        public string targetBindName;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool enableDefaultProcessor = true;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        public RingTargetsProcessorPipeline<object> Pipeline { get; } = new();

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly DataTokenProperty<object, object> bindObjectsProperty = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            bindObjectsProperty.SetOwner(this);
            bindObjectsProperty.OnDataChanged += OnDataChanged;
            bindObjectsProperty.OnTokenChanged += OnTokenChanged;

            if (enableDefaultProcessor)
            {
                Pipeline.AddProcessor(this, PriorityDefines.HIGH);
            }

            Panel.BindObjectsManager.OnBindObjectChanged += OnBindObjectChanged;
        }

        protected virtual void OnBindObjectChanged(string bindName, object bindObject, object parentObject, bool added)
        {
            if (bindName != sourceBindName)
            {
                return;
            }

            if (added)
            {
                AddBindObject(bindObject);
            }
            else
            {
                RemoveBindObject(bindObject);
            }
        }

        protected virtual void OnDataChanged(IReadOnlyProperty property, object data, bool added)
        {
            if (added)
            {
                Panel.BindObjectsManager.AddObject(targetBindName, data);
            }
            else
            {
                Panel.BindObjectsManager.RemoveObject(targetBindName, data);
            }
        }

        protected virtual void OnTokenChanged(IReadOnlyProperty property, object token, bool added)
        {

        }

        public virtual void AddBindObject(object value)
        {
            var targets = ListPool<object>.Default.Get();
            targets.Clear();
            Pipeline.Process(value, targets);

            if (targets.Count > 0)
            {
                foreach (var target in targets)
                {
                    if (target == null)
                    {
                        UnityEngine.Debug.LogError($"Target cannot be null.", this);
                        continue;
                    }

                    bindObjectsProperty.AddData(value, target);
                }
            }

            targets.ReturnToDefaultPool();
        }

        public virtual void RemoveBindObject(object value)
        {
            bindObjectsProperty.RemoveToken(value);
        }

        public virtual void ProcessTargets(IReadOnlyCollection<object> targets, ICollection<object> results)
        {

        }
    }
}