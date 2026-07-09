#if UNITY_EDITOR
namespace VMFramework.GameEvents
{
    public abstract partial class ColliderMouseTriggerModifier
    {
        protected virtual void Reset()
        {
            trigger = GetComponentInParent<ColliderMouseEventTrigger>();
        }
    }
}
#endif