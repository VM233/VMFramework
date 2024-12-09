using Sirenix.OdinInspector;
using VMFramework.Localization;

namespace VMFramework.GameLogicArchitecture
{
    public partial class LocalizedGameTagInfo : GameTagInfo, INameOwner, ILocalizedNameOwner
    {
        [PropertyOrder(-5000)]
        public LocalizedStringReference name = new();

        #region Interface Implementation

        string INameOwner.Name => name;

        IReadOnlyLocalizedStringReference ILocalizedNameOwner.NameReference => name;

        #endregion
    }
}