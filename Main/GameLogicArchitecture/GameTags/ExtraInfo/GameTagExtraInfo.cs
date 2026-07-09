using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.GameLogicArchitecture
{
    public abstract class GameTagExtraInfo : BaseConfig, IIDOwner<string>
    {
        [GameTagID]
        [IsNotNullOrEmpty]
        public string gameTagID;

        string IIDOwner<string>.id => gameTagID;
    }
}