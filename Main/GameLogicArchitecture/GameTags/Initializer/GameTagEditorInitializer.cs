#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using VMFramework.Procedure;
using VMFramework.Procedure.Editor;

namespace VMFramework.GameLogicArchitecture.Editor
{
    internal sealed class GameTagEditorInitializer : IEditorInitializer
    {
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.PreInit, OnPreInit, this));
        }

        private static void OnPreInit(Action onDone)
        {
            if (CoreSetting.GameTagGeneralSetting == null)
            {
                Debug.LogError($"{nameof(GameTagGeneralSetting)} is not set. Please set it in the {nameof(CoreSetting)}.");
                
                onDone();
                return;
            }
            
            CoreSetting.GameTagGeneralSetting.InitGameTags();
            
            onDone();
        }
    }
}
#endif