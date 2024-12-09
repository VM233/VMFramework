using System.Collections.Generic;
using UnityEngine;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    public abstract partial class GameTagBasedConfigBase : BaseConfig, IGameTagsOwner, INameOwner
    {
        [GameTagID]
        [IsNotNullOrEmpty]
        [SerializeField]
        private List<string> gameTags = new();

        public ICollection<string> GameTags => gameTags;

        string INameOwner.Name => gameTags.Join(", ");
    }
}