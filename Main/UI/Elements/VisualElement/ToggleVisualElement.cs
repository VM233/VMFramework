using System;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;

namespace VMFramework.UI
{
    [UxmlElement]
    public partial class ToggleVisualElement : BasicVisualElement
    {
        protected const string ussClassName = "toggle";

        protected const string borderUssClassName = ussClassName + "Border";

        protected const string checkmarkUssClassName = ussClassName + "Checkmark";

        protected VisualElement border;

        protected VisualElement checkmark;

        private bool _isChecked;

        [UxmlAttribute]
        public bool isChecked
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _isChecked;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => SetIsCheckedValue(value);
        }

        public event Action<bool> OnValueChanged;

        public ToggleVisualElement() : base()
        {
            border = new VisualElement();
            Add(border);
            border.AddToClassList(borderUssClassName);

            checkmark = new VisualElement();
            Add(checkmark);
            checkmark.AddToClassList(checkmarkUssClassName);

            isChecked = true;

            RegisterCallback<PointerDownEvent>(e => { isChecked = !isChecked; });
        }

        private void SetIsCheckedValue(bool newIsCheckedValue)
        {
            _isChecked = newIsCheckedValue;
            OnValueChanged?.Invoke(newIsCheckedValue);

            checkmark.style.visibility =
                isChecked ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
