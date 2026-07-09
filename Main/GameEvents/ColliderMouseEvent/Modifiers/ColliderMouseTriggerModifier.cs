using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.GameEvents
{
    public abstract partial class ColliderMouseTriggerModifier : MonoBehaviour
    {
        [Required]
        public ColliderMouseEventTrigger trigger;
        
        public MouseEventType eventType = MouseEventType.RightMouseButtonClick;
        
        private MouseEventHandler onClickFunc;

        protected virtual void Awake()
        {
            onClickFunc = OnTrigger;
        }

        protected virtual void OnEnable()
        {
            if (trigger != null)
            {
                trigger.AddCallback(eventType, onClickFunc);
            }
        }

        protected virtual void OnDisable()
        {
            if (trigger != null)
            {
                trigger.RemoveCallback(eventType, onClickFunc);
            }
        }

        /// <summary>
        /// 子类需要实现的触发方法
        /// </summary>
        protected abstract void OnTrigger(ColliderMouseEventTrigger trigger, MouseEventType eventType);
    }
}