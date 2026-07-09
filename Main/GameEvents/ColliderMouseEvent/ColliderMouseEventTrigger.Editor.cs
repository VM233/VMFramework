#if UNITY_EDITOR
namespace VMFramework.GameEvents
{
    public partial class ColliderMouseEventTrigger
    {
        private void Reset()
        {
            if (Owner == null)
            {
                Owner = transform;
            }
        }
    }
}
#endif