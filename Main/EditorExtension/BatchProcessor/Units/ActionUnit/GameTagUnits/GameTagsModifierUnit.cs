#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.Editor.BatchProcessor
{
    public class GameTagsModifierUnit : DoubleButtonBatchProcessorUnit
    {
        [GameTagID]
        public HashSet<string> gameTags = new();

        protected override string ProcessButtonOneName => "Add Game Tags";

        protected override string ProcessButtonTwoName => "Remove Game Tags";

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.Any(obj => obj is IGameTagsOwner);
        }

        protected override IEnumerable<object> OnProcessOne(IEnumerable<object> selectedObjects)
        {
            if (gameTags.Count == 0)
            {
                return selectedObjects;
            }

            foreach (var obj in selectedObjects)
            {
                if (obj is IGameTagsOwner gameTagsOwner)
                {
                    foreach (var gameTag in gameTags)
                    {
                        gameTagsOwner.GameTags.Add(gameTag);
                    }
                }
            }

            return selectedObjects;
        }

        protected override IEnumerable<object> OnProcessTwo(IEnumerable<object> selectedObjects)
        {
            if (gameTags.Count == 0)
            {
                return selectedObjects;
            }

            foreach (var obj in selectedObjects)
            {
                if (obj is IGameTagsOwner gameTagsOwner)
                {
                    foreach (var gameTag in gameTags)
                    {
                        gameTagsOwner.GameTags.Remove(gameTag);
                    }
                }
            }

            return selectedObjects;
        }
    }
}
#endif