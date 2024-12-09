using System.Runtime.CompilerServices;
using UnityEditor;

namespace VMFramework.Core.Editor
{
    public static class DialogUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool DisplayWarningDialog(this string message)
        {
            return EditorUtility.DisplayDialog("Warning", message, "OK", "Cancel");
        }
    }
}
