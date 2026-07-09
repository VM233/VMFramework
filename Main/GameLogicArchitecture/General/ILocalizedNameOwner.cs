using UnityEngine.Localization;

namespace VMFramework.GameLogicArchitecture
{
    public interface ILocalizedNameOwner
    {
        public LocalizedString NameReference { get; }
    }
}
