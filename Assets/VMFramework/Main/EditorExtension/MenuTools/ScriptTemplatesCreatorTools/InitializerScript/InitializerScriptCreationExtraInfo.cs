﻿#if UNITY_EDITOR
using VMFramework.Core.Editor;

namespace VMFramework.Editor
{
    public class InitializerScriptCreationExtraInfo : ScriptCreationExtraInfo
    {
        public string initializationOrderName { get; init; }
    }
}
#endif