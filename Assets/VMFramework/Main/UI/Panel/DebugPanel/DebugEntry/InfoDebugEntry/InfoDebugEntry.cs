using Newtonsoft.Json;
using VMFramework.Localization;

namespace VMFramework.UI
{
    public sealed partial class InfoDebugEntry : TitleContentDebugEntry
    {
        [JsonProperty]
        public LocalizedStringReference content = new();

        protected override string GetContent() => content;
    }
}
