using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class BindObjectTracingModifier : PanelModifier
    {
        public enum Mode
        {
            TracingModifier,
            Direct,
        }

        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName(SingleModeLimit = BindObjectsNameAttribute.SingleModeLimitType.Single)]
        [IsNotNullOrEmpty]
        public string bindObjectsName;

        [TitleGroup(ComponentNames.CONFIG)]
        public Mode mode;

        [TitleGroup(ComponentNames.CONFIG)]
        [ShowIf(nameof(mode), Mode.TracingModifier)]
        public TargetReferenceOrParentConfig<ITracingPanelModifier> tracingModifierConfig = new();

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected Transform CurrentTransform { get; set; }

        protected ITracingPanelModifier tracingModifier;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            if (mode is Mode.TracingModifier)
            {
                tracingModifier = tracingModifierConfig.GetTarget(this);
            }

            Panel.BindObjectsManager.OnBindObjectChanged += OnBindObjectChanged;
        }

        protected virtual void OnBindObjectChanged(string bindName, object bindObject, object parentObject, bool added)
        {
            if (bindName != bindObjectsName)
            {
                return;
            }

            if (mode is Mode.TracingModifier)
            {
                if (added)
                {
                    if (bindObject.TryAsGameObject(out var gameObject))
                    {
                        TracingUIManager.Instance.StartTracing(tracingModifier, gameObject.transform);
                        CurrentTransform = gameObject.transform;
                    }
                }
                else
                {
                    if (bindObject.TryAsGameObject(out var gameObject))
                    {
                        if (gameObject.transform == CurrentTransform)
                        {
                            TracingUIManager.Instance.StopTracing(tracingModifier);
                        }
                    }
                }
            }
        }

        protected virtual void Update()
        {
            if (Panel.IsOpened == false)
            {
                return;
            }

            if (Panel.BindObjectsManager.GetObject(bindObjectsName) is not { } bindObject)
            {
                return;
            }

            if (mode is Mode.Direct)
            {
                if (bindObject.TryAsGameObject(out var gameObject))
                {
                    Panel.transform.position = gameObject.transform.position;
                }
            }
        }
    }
}