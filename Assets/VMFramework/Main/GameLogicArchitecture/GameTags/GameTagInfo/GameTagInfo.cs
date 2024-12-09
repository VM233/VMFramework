using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.Localization;
using VMFramework.OdinExtensions;

namespace VMFramework.GameLogicArchitecture
{
    public class GameTagInfo : BaseConfig, IIDOwner<string>, INameOwner, ILocalizedNameOwner
    {
        [LabelText("ID")]
        [IsNotNullOrEmpty, IsGameTagID]
        [JsonProperty]
        public string id;

        [HideInEditorMode]
        public string parentID;

        #region Interface Implementation

        string IIDOwner<string>.id => id;

        string INameOwner.Name => id.ToPascalCase(" ");

        IReadOnlyLocalizedStringReference ILocalizedNameOwner.NameReference => new LocalizedStringReference()
        {
            defaultValue = id.ToPascalCase(" ")
        };

        #endregion
    }
}