using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Localization;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class PairNameLabelModifier : PanelModifier, ILocalizedPanelModifier
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
        public bool considerPrefabID = true;

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

            if (TryGetString(bindObject, out var keyName))
            {
                label.text = keyName;
                label.DisplayFlex();
            }
            else
            {
                label.text = string.Empty;
                label.DisplayNone();
            }

            ProcessNameLabel(label, bindObject);
        }

        protected virtual void ProcessNameLabel(Label nameLabel, object key)
        {

        }

        protected virtual bool TryGetString(object key, out string result)
        {
            result = null;

            if (considerDescriptionType)
            {
                if (key.TryGetComponentOrAs(out IDescriptionManagerProvider provider) &&
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
                if (key.TryGetComponentOrAs(out INameOwner nameOwner))
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
                if (key.TryGetComponentOrAs(out IIDOwner<string> idOwner))
                {
                    result = idOwner.id;

                    if (result.IsNullOrEmpty() == false)
                    {
                        return true;
                    }
                }
            }

            if (considerPrefabID)
            {
                if (key is string strKey)
                {
                    if (GamePrefabManager.TryGetGamePrefab(strKey, out var gamePrefab))
                    {
                        result = gamePrefab.Name;
                    }
                    else
                    {
                        result = strKey;
                    }

                    return result.IsNullOrEmpty() == false;
                }
            }

            return false;
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