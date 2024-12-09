#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using VMFramework.Procedure.Editor;

namespace VMFramework.Procedure
{
    internal sealed class ManagerCreatorEditorInitializer : IEditorInitializer
    {
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.BeforeInitStart, OnBeforeInitStart, this));
        }
        
        private static void OnBeforeInitStart(Action onDone)
        {
            ManagerCreator.CreateManagers();
            onDone();
        }
    }
}
#endif