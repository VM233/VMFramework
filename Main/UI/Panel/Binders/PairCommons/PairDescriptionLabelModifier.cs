using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Localization;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class PairDescriptionLabelModifier : PanelModifier, ILocalizedPanelModifier, IDescriptionTypesModifyProvider
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName]
        [IsNotNullOrEmpty]
        public string bindObjectsName;

        [TitleGroup(ComponentNames.CONFIG)]
        [VisualElementPathSettings(typeof(Label), IsFromLocalProvider = true)]
        [IsNotNullOrEmpty]
        public VisualElementPath labelPath = new();

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

        protected override void OnInitialize()
        {
            base.OnInitialize();

            this.UIToolkitPanel().BindVisualElementsManager.OnBindVisualElementChanged += OnBindVisualElementChanged;
        }

        protected virtual void OnBindVisualElementChanged(string bindName, object bindObject,
            VisualElement visualElement, bool added)
        {
            if (bindName != bindObjectsName)
            {
                return;
            }

            if (added)
            {
                Refresh(bindObject, visualElement);
            }
        }

        protected virtual void Refresh(object bindObject, VisualElement visualElement)
        {
            var label = labelPath.MandatoryQuery<Label>(visualElement, nameof(labelPath));

            var keyName = GetBindObjectDescription(bindObject);

            label.text = keyName;
        }

        protected virtual string GetBindObjectDescription(object bindObject)
        {
            string keyName = null;

            if (bindObject.IsUnityNull())
            {
                return null;
            }

            if (considerDescriptionOwner)
            {
                if (bindObject.TryGetComponentOrAs(out IDescriptionOwner owner))
                {
                    keyName = owner.Description;

                    if (keyName.IsNullOrEmpty() == false)
                    {
                        return keyName;
                    }
                }
            }

            if (considerDescriptionType)
            {
                if (bindObject.TryGetComponentOrAs(out IDescriptionManagerProvider provider) &&
                    provider.DescriptionManager is { } descriptionManager)
                {
                    var resultDescriptionTypes = priorityDescriptionTypes.ToListDefaultPooled();
                    OnModifyDescriptionTypes?.Invoke(bindObject, resultDescriptionTypes);
                    
                    foreach (var type in resultDescriptionTypes)
                    {
                        if (descriptionManager.TryGetDescription(type, out keyName))
                        {
                            resultDescriptionTypes.ReturnToDefaultPool();
                            return keyName;
                        }
                    }
                    
                    resultDescriptionTypes.ReturnToDefaultPool();
                }
            }

            if (considerIDOwner)
            {
                if (bindObject.TryGetComponentOrAs(out IIDOwner<string> idOwner))
                {
                    var id = idOwner.id;

                    if (GamePrefabManager.TryGetGamePrefab(id, out var gamePrefab))
                    {
                        if (gamePrefab is IDescriptionOwner descriptionOwner)
                        {
                            keyName = descriptionOwner.Description;

                            if (keyName.IsNullOrEmpty() == false)
                            {
                                return keyName;
                            }
                        }
                    }
                }
            }

            if (keyName == null)
            {
                if (bindObject is string strKey)
                {
                    if (GamePrefabManager.TryGetGamePrefab(strKey, out var gamePrefab))
                    {
                        if (gamePrefab is IDescriptionOwner prefabDescriptionOwner)
                        {
                            keyName = prefabDescriptionOwner.Description ?? string.Empty;
                        }
                    }
                }
            }

            keyName ??= string.Empty;

            return keyName;
        }

        public virtual void OnCurrentLanguageChanged(Locale currentLocale)
        {
            foreach (var (bindObject, visualElement) in this.UIToolkitPanel().BindVisualElementsManager
                         .GetBindInfos(bindObjectsName))
            {
                Refresh(bindObject, visualElement);
            }
        }
    }
}