using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;
using VMFramework.Parameters;

namespace VMFramework.UI
{
    public class PairGameTagNameModifier : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName]
        [IsNotNullOrEmpty]
        public string bindObjectsName;
        
        [TitleGroup(ComponentNames.CONFIG)]
        [GameTagID]
        [IsNotNullOrEmpty]
        public List<string> gameTags = new();

        [TitleGroup(ComponentNames.CONFIG)]
        [VisualElementPathSettings(typeof(Label), IsFromLocalProvider = true)]
        [IsNotNullOrEmpty]
        public List<VisualElementPath> labelPaths = new();

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<string> separatorConfig = new FixedValueParameterSourceConfig<string>(" ");

        protected IParameterSource<string> separator;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            separator = separatorConfig.GetParameterSource();
            
            this.UIToolkitPanel().BindVisualElementsManager.OnBindVisualElementChanged += OnBindVisualElementChanged;
        }

        protected virtual void OnBindVisualElementChanged(string bindName, object bindObject,
            VisualElement visualElement, bool added)
        {
            if (bindName != bindObjectsName)
            {
                return;
            }
            
            var gameTagsOwner = (IGameTagsOwner)bindObject;
            
            if (added)
            {
                var validTags = ListPool<string>.Default.Get();
                validTags.Clear();

                foreach (var tag in gameTags)
                {
                    if (gameTagsOwner.HasTag(tag))
                    {
                        validTags.Add(tag);
                    }
                }

                string text;

                if (validTags.Count > 0)
                {
                    var separatorValue = separator.GetValueOrDefault(string.Empty);
                    text = GameTagNameUtility.GetTagNames(validTags).Join(separatorValue);
                }
                else
                {
                    text = string.Empty;
                }

                foreach (var labelPath in labelPaths)
                {
                    var label = labelPath.MandatoryQuery<Label>(visualElement, nameof(labelPath));
                    label.text = text;
                }

                validTags.ReturnToDefaultPool();
            }
        }
    }
}