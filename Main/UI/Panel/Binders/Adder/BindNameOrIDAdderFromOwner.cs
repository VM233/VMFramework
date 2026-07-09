using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Localization;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class BindNameOrIDAdderFromOwner : PipelinedBindObjectsAdder, ILocalizedPanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        public bool considerDescriptionType = true;

        [TitleGroup(ComponentNames.CONFIG)]
        [ShowIf(nameof(considerDescriptionType))]
        [CommonPreset(DescriptionMeta.TYPE_PRESET_KEY)]
        [IsNotNullOrEmpty]
        public List<string> priorityDescriptionTypes = new();

        [TitleGroup(ComponentNames.CONFIG)]
        public bool considerNameOwner = true;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool considerIDOwner = true;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool refreshWhenLocaleChanges = true;

        public override void ProcessTargets(IReadOnlyCollection<object> targets, ICollection<object> results)
        {
            base.ProcessTargets(targets, results);

            foreach (var target in targets)
            {
                if (TryGetString(target, out var result))
                {
                    results.Add(result);
                }
            }
        }

        protected virtual bool TryGetString(object target, out string result)
        {
            if (considerDescriptionType)
            {
                if (target.TryGetComponentOrAs(out IDescriptionManagerProvider provider) &&
                    provider.DescriptionManager is { } descriptionManager)
                {
                    foreach (var type in priorityDescriptionTypes)
                    {
                        if (descriptionManager.TryGetDescription(type, out result))
                        {
                            return true;
                        }
                    }
                }
            }

            if (considerNameOwner)
            {
                if (target.TryGetComponentOrAs(out INameOwner nameOwner))
                {
                    result = nameOwner.Name;

                    if (result.IsNullOrEmpty() == false)
                    {
                        return true;
                    }
                }
            }

            if (considerIDOwner)
            {
                if (target.TryGetComponentOrAs(out IIDOwner<string> idOwner))
                {
                    result = idOwner.id;

                    if (result.IsNullOrEmpty() == false)
                    {
                        return true;
                    }
                }
            }

            result = null;
            return false;
        }

        public virtual void OnCurrentLanguageChanged(Locale currentLocale)
        {
            if (refreshWhenLocaleChanges)
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
}