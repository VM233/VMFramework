#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class FindGamePrefabWrapperUnit : SingleButtonBatchProcessorUnit
    {
        protected override string ProcessButtonName => "Get GamePrefabWrapper";

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            foreach (var obj in selectedObjects)
            {
                if (obj is IGamePrefab)
                {
                    return true;
                }

                if (obj is Type type)
                {
                    if (type.IsDerivedFrom<IGamePrefab>(true))
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }

        protected override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            foreach (var obj in selectedObjects)
            {
                if (obj is IGamePrefab gamePrefab)
                {
                    foreach (var wrapper in GamePrefabWrapperQueryTools.GetGamePrefabWrappers(gamePrefab))
                    {
                        yield return wrapper;
                    }
                    
                    continue;
                }

                if (obj is Type type)
                {
                    if (type.IsDerivedFrom<IGamePrefab>(true))
                    {
                        foreach (var wrapper in GamePrefabWrapperQueryTools.GetGamePrefabWrappers(type))
                        {
                            yield return wrapper;
                        }
                    }
                    continue;
                }
                
                yield return obj;
            }
        }
    }
}
#endif