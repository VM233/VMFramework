using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public abstract class BindProcessorBase : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        public bool customPriority = false;

        [TitleGroup(ComponentNames.CONFIG)]
        [CommonPreset(PriorityDefinesPreset.NAME)]
        [ShowIf(nameof(customPriority))]
        public int priority = 0;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool specificProvider = false;

        [TitleGroup(ComponentNames.CONFIG)]
        [ShowIf(nameof(specificProvider))]
        public TargetReferenceConfig<IBindPipelineProvider> specificProviderConfig = new();

        protected IBindPipelineProvider provider;

        protected abstract IFuncTargetsProcessor<object, object> Processor { get; }

        protected abstract int DefaultPriority { get; }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            if (specificProvider)
            {
                provider = specificProviderConfig.Target;
            }
            else
            {
                provider = GetComponentInParent<IBindPipelineProvider>();
            }

            var currentPriority = customPriority ? priority : DefaultPriority;
            provider.Pipeline.AddProcessor(Processor, currentPriority);
        }
    }
}