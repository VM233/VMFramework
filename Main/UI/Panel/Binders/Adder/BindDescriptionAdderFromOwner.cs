using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Localization;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class BindDescriptionAdderFromOwner : PipelinedBindObjectsAdder, ILocalizedPanelModifier,
        IDescriptionTypesModifyProvider
    {
        [TitleGroup(ComponentNames.CONFIG)]
        public bool refreshWhenLocaleChanges = true;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool considerDescriptionOwner = true;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool considerDescriptionType = true;

        [TitleGroup(ComponentNames.CONFIG)]
        [CommonPreset(DescriptionMeta.TYPE_PRESET_KEY)]
        [IsNotNullOrEmpty]
        public List<string> priorityDescriptionTypes = new();

        [TitleGroup(ComponentNames.CONFIG)]
        public bool considerIDOwner = true;

        public event IDescriptionTypesModifyProvider.GetHandler OnModifyDescriptionTypes;

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
            if (considerDescriptionOwner)
            {
                if (target.TryGetComponentOrAs(out IDescriptionOwner owner))
                {
                    result = owner.Description;

                    if (result.IsNullOrEmpty() == false)
                    {
                        return true;
                    }
                }
            }

            if (considerDescriptionType)
            {
                if (target.TryGetComponentOrAs(out IDescriptionManagerProvider provider) &&
                    provider.DescriptionManager is { } descriptionManager)
                {
                    var resultDescriptionTypes = priorityDescriptionTypes.ToListDefaultPooled();
                    OnModifyDescriptionTypes?.Invoke(target, resultDescriptionTypes);
                    
                    foreach (var type in resultDescriptionTypes)
                    {
                        if (descriptionManager.TryGetDescription(type, out result))
                        {
                            resultDescriptionTypes.ReturnToDefaultPool();
                            return true;
                        }
                    }
                    
                    resultDescriptionTypes.ReturnToDefaultPool();
                }
            }

            if (considerIDOwner)
            {
                if (target.TryGetComponentOrAs(out IIDOwner<string> idOwner))
                {
                    var id = idOwner.id;

                    if (GamePrefabManager.TryGetGamePrefab(id, out var gamePrefab))
                    {
                        if (gamePrefab is IDescriptionOwner descriptionOwner)
                        {
                            result = descriptionOwner.Description;

                            if (result.IsNullOrEmpty() == false)
                            {
                                return true;
                            }
                        }
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