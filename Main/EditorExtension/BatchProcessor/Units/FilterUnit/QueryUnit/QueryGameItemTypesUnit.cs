#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Editor.BatchProcessor
{
    internal sealed class QueryGameItemTypesUnit : SingleButtonBatchProcessorUnit
    {
        protected override string ProcessButtonName => "Query Game Item Types";

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.Any(obj =>
            {
                if (obj is Type type)
                {
                    if (type.IsDerivedFrom(typeof(IGamePrefab), false))
                    {
                        return true;
                    }
                }
                else if (obj is IGamePrefab gamePrefab)
                {
                    return true;
                }

                return false;
            });
        }

        protected override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            foreach (var obj in selectedObjects)
            {
                IGamePrefab gamePrefab = null;
                if (obj is Type type)
                {
                    if (type.IsDerivedFrom(typeof(IGamePrefab), false))
                    {
                        gamePrefab = (IGamePrefab)InstanceCache.Get(type);
                    }
                }
                else if (obj is IGamePrefab targetGamePrefab)
                {
                    gamePrefab = targetGamePrefab;
                }
                
                if (gamePrefab?.GameItemType != null)
                {
                    yield return gamePrefab.GameItemType;
                }
            }
        }
    }
}
#endif