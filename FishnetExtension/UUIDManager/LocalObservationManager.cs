#if FISHNET
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Procedure;
using VMFramework.Properties;

namespace VMFramework.Network
{
    [ManagerCreationProvider(ManagerType.NetworkCore)]
    public class LocalObservationManager : ManagerBehaviour<LocalObservationManager>
    {
        public IDataTokenProperty<IToken, IUUIDOwner> Observations => observationsProperty;

        [ShowInInspector]
        protected readonly DataTokenProperty<IToken, IUUIDOwner> observationsProperty = new();

        protected override void Awake()
        {
            base.Awake();

            observationsProperty.ClearAndReset();
            observationsProperty.OnDataChanged += OnDataChanged;
        }

        protected virtual void OnDataChanged(IReadOnlyProperty property, IUUIDOwner data, bool added)
        {
            if (added)
            {
                UUIDCoreManager.Instance.Observe(data);
            }
            else
            {
                UUIDCoreManager.Instance.Unobserve(data);
            }
        }
    }
}
#endif