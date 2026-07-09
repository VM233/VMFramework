using System.Collections.Generic;
using UnityEngine.Localization;

namespace VMFramework.UI
{
    public class BindContentAdderFromLocalizedString : PipelinedBindObjectsAdder, ILocalizedPanelModifier
    {
        public override void ProcessTargets(IReadOnlyCollection<object> targets, ICollection<object> results)
        {
            base.ProcessTargets(targets, results);

            foreach (var target in targets)
            {
                if (target is LocalizedString localizedString)
                {
                    results.Add(localizedString.GetLocalizedString());
                }
            }
        }

        public virtual void OnCurrentLanguageChanged(Locale currentLocale)
        {
            foreach (var bindObject in Panel.BindObjectsManager.GetObjects(sourceBindName))
            {
                RemoveBindObject(bindObject);
            }

            foreach (var bindObject in Panel.BindObjectsManager.GetObjects(sourceBindName))
            {
                AddBindObject(bindObject);
            }
        }
    }
}