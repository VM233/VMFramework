using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class GameTagsBindProcessor : BindProcessorBase, IFuncTargetsProcessor<object, object>
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [GameTagID]
        public List<string> requiredTags = new();

        [TitleGroup(ComponentNames.CONFIG)]
        [GameTagID]
        public List<string> limitTags = new();

        [TitleGroup(ComponentNames.CONFIG)]
        [GameTagID]
        public List<string> excludeTags = new();

        protected override IFuncTargetsProcessor<object, object> Processor => this;

        protected override int DefaultPriority => PriorityDefines.LOW;

        public virtual void ProcessTargets(IReadOnlyCollection<object> targets, ICollection<object> results)
        {
            if (requiredTags.IsNullOrEmpty() && limitTags.IsNullOrEmpty() && excludeTags.IsNullOrEmpty())
            {
                results.AddRange(targets);
                return;
            }

            foreach (var target in targets)
            {
                if (target.TryGetComponentOrAs(out IGameTagsOwner gameTagsOwner) == false)
                {
                    continue;
                }

                if (requiredTags.IsNullOrEmpty() == false)
                {
                    if (gameTagsOwner.HasAllTags(requiredTags) == false)
                    {
                        continue;
                    }
                }

                if (limitTags.IsNullOrEmpty() == false)
                {
                    if (gameTagsOwner.HasAnyTags(limitTags) == false)
                    {
                        continue;
                    }
                }

                if (excludeTags.IsNullOrEmpty() == false)
                {
                    if (gameTagsOwner.HasAnyTags(excludeTags))
                    {
                        continue;
                    }
                }

                results.Add(target);
            }
        }
    }
}