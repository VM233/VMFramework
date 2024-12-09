#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using VMFramework.Core.Editor;
using VMFramework.GameLogicArchitecture;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class OpenScriptOfGamePrefabUnit : SingleButtonBatchProcessorUnit
    {
        protected override string ProcessButtonName => "Open GamePrefab Script";

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.Any(obj => obj is IGamePrefabProvider);
        }

        protected override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            foreach (var obj in selectedObjects)
            {
                if (obj is IGamePrefabProvider provider)
                {
                    provider.OpenGamePrefabScripts();
                }
            }
            
            return selectedObjects;
        }
    }
}
#endif