using System;
using Sirenix.OdinInspector;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public class TitleContentGenerator : INameOwner, IDescriptionOwner
    {
        [ShowInInspector]
        public Func<string> TitleGenerator { get; set; }

        [ShowInInspector]
        public Func<string> ContentGenerator { get; set; }

        public string Name => TitleGenerator?.Invoke();

        public string Description => ContentGenerator?.Invoke();
    }
}