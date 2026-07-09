using UnityEngine.Localization;

namespace VMFramework.GameLogicArchitecture
{
    public interface ILocalizedDescriptionOwner
    {
        public LocalizedString DescriptionReference { get; }
    }
}