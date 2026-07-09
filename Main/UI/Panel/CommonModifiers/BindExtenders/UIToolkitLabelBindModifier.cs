using System;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    [InfoBox("需要绑定" + nameof(String))]
    public class UIToolkitLabelBindModifier : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName(SingleModeLimit = BindObjectsNameAttribute.SingleModeLimitType.Single)]
        [IsNotNullOrEmpty]
        public string bindObjectsName;

        [TitleGroup(ComponentNames.CONFIG)]
        [VisualElementPathSettings(typeof(Label))]
        [IsNotNullOrEmpty]
        public VisualElementPath labelPath = new();

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        public Label Label { get; protected set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpen += OnOpen;
            Panel.OnPostClose += OnClose;
            Panel.BindObjectsManager.OnBindObjectChanged += OnBindObjectChanged;
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            Label = labelPath.MandatoryQuery<Label>(this.RootVisualElement(), nameof(labelPath));

            Refresh();
        }

        protected virtual void OnClose(IUIPanel panel)
        {
            Label.text = string.Empty;
            Label = null;
        }

        protected virtual void OnBindObjectChanged(string bindName, object bindObject, object parentObject, bool added)
        {
            if (bindName != bindObjectsName)
            {
                return;
            }
            
            Refresh();
        }

        protected virtual void Refresh()
        {
            if (Panel.BindObjectsManager.GetObject(bindObjectsName) is string text)
            {
                Label.text = text;
                Label.DisplayFlex();
            }
            else
            {
                Label.DisplayNone();
            }
        }
    }
}