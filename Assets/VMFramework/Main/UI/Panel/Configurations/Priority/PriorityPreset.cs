using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public sealed class PriorityPreset : BaseConfig, IIDOwner<string>, INameOwner
    {
        [HorizontalGroup]
        [IsNotNullOrEmpty]
        public string presetID;

        [HorizontalGroup]
        public int priority;

        string IIDOwner<string>.id => presetID;

        string INameOwner.Name => ToString();

        public override string ToString()
        {
            return $"{presetID}-{priority}";
        }
    }
}