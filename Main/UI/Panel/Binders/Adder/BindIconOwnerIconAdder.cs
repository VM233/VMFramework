using System.Collections.Generic;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public class BindIconOwnerIconAdder : PipelinedBindObjectsAdder
    {
        public override void ProcessTargets(IReadOnlyCollection<object> targets, ICollection<object> results)
        {
            base.ProcessTargets(targets, results);

            foreach (var target in targets)
            {
                if (target.TryGetComponentOrAs(out IIconOwner iconOwner))
                {
                    var icon = iconOwner.Icon;

                    if (icon == null)
                    {
                        continue;
                    }

                    results.Add(iconOwner.Icon);
                }
            }
        }
    }
}