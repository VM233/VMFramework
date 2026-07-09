#if FISHNET
using VMFramework.Containers;
using VMFramework.Core;
using VMFramework.Network;

namespace VMFramework.UI
{
    public class AutoContainerObserveModifier : BindModifierExtender, IToken
    {
        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnPostCloseEvent += OnPostClose;
        }

        protected override void OnBindObjectAdded(IObjectBinderDispatcher dispatcher, object value)
        {
            base.OnBindObjectAdded(dispatcher, value);

            if (value is IContainer { UUIDOwner: { } uuidOwner })
            {
                LocalObservationManager.Instance.Observations.AddData(this, uuidOwner);
            }
        }

        protected override void OnBindObjetRemoved(IObjectBinderDispatcher dispatcher, object value)
        {
            base.OnBindObjetRemoved(dispatcher, value);

            if (value is IContainer { UUIDOwner: { } uuidOwner })
            {
                LocalObservationManager.Instance.Observations.RemoveData(this, uuidOwner);
            }
        }

        protected virtual void OnPostClose(IUIPanel panel)
        {
            LocalObservationManager.Instance.Observations.RemoveToken(this);
        }
    }
}
#endif