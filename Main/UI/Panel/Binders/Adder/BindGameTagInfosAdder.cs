using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class BindGameTagInfosAdder : PipelinedBindObjectsAdder
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [GameTagID]
        [IsNotNullOrEmpty]
        public List<string> validTags = new();

        public override void ProcessTargets(IReadOnlyCollection<object> targets, ICollection<object> results)
        {
            base.ProcessTargets(targets, results);

            foreach (var target in targets)
            {
                if (target.TryGetComponentOrAs(out IGameTagsOwner gameTagsOwner))
                {
                    foreach (var validTag in validTags)
                    {
                        if (gameTagsOwner.HasTag(validTag))
                        {
                            if (GameTag.TryGetTag(validTag, out var tagInfo))
                            {
                                results.Add(tagInfo);
                            }
                        }
                    }
                }
            }
        }
    }
}