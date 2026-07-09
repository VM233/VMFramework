using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.GameLogicArchitecture
{
    public class IconOwnerIconComponent : MonoBehaviour
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [CommonPreset(IconMeta.TYPE_PRESET_KEY)]
        [IsNotNullOrEmpty]
        public string iconType;

        protected IIconOwner iconOwner;
        protected IconManager iconManager;

        protected virtual void Awake()
        {
            iconOwner = GetComponentInParent<IIconOwner>();

            iconManager = GetComponentInParent<IconManager>();
            iconManager.GetEvents.Add(PriorityDefines.MEDIUM, OnGetIcon);
        }

        protected virtual void OnGetIcon(string type, ref Sprite icon)
        {
            if (type != iconType)
            {
                return;
            }

            if (icon == null)
            {
                icon = iconOwner.Icon;
            }
        }
    }
}