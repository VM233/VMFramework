using System;
using VMFramework.UI;

namespace VMFramework.OdinExtensions
{
    public sealed class UIPresetIDAttribute : GamePrefabIDAttribute
    {
        public bool? IsUnique = null;

        public UIPresetIDAttribute(params Type[] uiPrefabTypes) : base(uiPrefabTypes.Length == 0
            ? new[] { typeof(IUIPanelConfig) }
            : uiPrefabTypes)
        {

        }

        public UIPresetIDAttribute() : this(typeof(IUIPanelConfig))
        {
            
        }

        public UIPresetIDAttribute(bool isUnique, params Type[] uiPrefabTypes) : this(uiPrefabTypes)
        {
            IsUnique = isUnique;
        }
    }
}