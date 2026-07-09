using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine.Localization;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.GameLogicArchitecture
{
    [Serializable]
    public partial class GameTagInfo : IIDOwner<string>, INameOwner, IDescriptionOwner, ILocalizedNameOwner
    {
        [IsNotNullOrEmpty, IsGameTagID]
        [DelayedProperty]
        [JsonProperty]
        public string id;
        
        public bool hasName = false;

        [ShowIf(nameof(hasName))]
        public LocalizedString name = new();
        
        public bool hasDescription = false;
        
        [ShowIf(nameof(hasDescription))]
        public LocalizedString description = new();

        string IIDOwner<string>.id => id;

        public string Name
        {
            get
            {
                if (hasName == false || name == null)
                {
                    return null;
                }

                return name.GetLocalizedString();
            }
        }

        public string Description
        {
            get
            {
                if (hasDescription == false || description == null)
                {
                    return null;
                }
                
                return description.GetLocalizedString();
            }
        }

        public LocalizedString NameReference => name;
    }
}