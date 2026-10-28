#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace VMFramework.Procedure.Editor
{
    internal sealed class EditorInitializationBuildBoundary : IPreprocessBuildWithReport
    {
        public int callbackOrder => int.MinValue;

        public void OnPreprocessBuild(BuildReport report)
        {
            EditorInitializer.SuspendForBuild();
        }
    }
}
#endif
