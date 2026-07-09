using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.GameEvents;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public partial class PanelColliderMouseEventModifierBase : ColliderMouseTriggerModifier
    {
        [Required]
        public GameObject bindObject;

        [GamePrefabID]
        [UIPanelConfigID(isUnique: true)]
        [IsNotNullOrEmpty]
        public string panelID;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void OnTrigger(ColliderMouseEventTrigger trigger, MouseEventType eventType)
        {
            if (CanOpenPanel() == false)
            {
                return;
            }
            
            var panel = UIPanelManager.Instance.GetUniquePanelStrictly<IUIPanel>(panelID);

            panel.Open(null);
            panel.BindObjectsManager.AddObject(bindObject);
        }

        protected virtual bool CanOpenPanel()
        {
            return true;
        }
    }
}