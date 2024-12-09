#if UNITY_EDITOR
using System;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Editor
{
    public sealed class GamePropertyScriptCreationViewer : ScriptCreationViewer
    {
        protected override string NameSuffix => "Property";

        [IsNotNullOrEmpty]
        [OnValueChanged(nameof(OnTargetTypeChanged))]
        public Type targetType;

        private string oldName;

        private void OnTargetTypeChanged()
        {
            if (name.IsNullOrEmpty())
            {
                name = GetNameFromTargetType(targetType);
                oldName = name;
            }
            else if (name == oldName)
            {
                name = GetNameFromTargetType(targetType);
                oldName = name;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetNameFromTargetType(Type targetType)
        {
            return targetType.Name.TrimEnd("Provider").TrimEnd("Owner").TrimStart('I');
        }
    }
}
#endif