using UnityEngine;
using VMFramework.Core;
using VMFramework.GameEvents;

namespace VMFramework.GameLogicArchitecture
{
    public class IconManager : MonoBehaviour, IDirtyable
    {
        public delegate void GetHandler(string type, ref Sprite icon);

        public delegate void DirtyHandler(string type);

        public IReadOnlyPriorityEvents<GetHandler> GetEvents => getEvents;

        public event DirtyHandler OnIconDirty;
        public event IDirtyable.DirtyHandler OnDirty;

        protected readonly PriorityEvents<GetHandler> getEvents = new();

        public virtual Sprite GetIcon(string iconType)
        {
            Sprite icon = null;
            foreach (var callback in getEvents.GetCombinedCallbacks())
            {
                callback?.Invoke(iconType, ref icon);
            }

            return icon;
        }

        public virtual void DirtyIcon(string iconType)
        {
            OnIconDirty?.Invoke(iconType);
            OnDirty?.Invoke(this, local: true);
        }
    }
}