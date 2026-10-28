#if UNITY_EDITOR
using UnityEditor.Build;

namespace VMFramework.Procedure.Editor
{
    internal sealed class EditorInitializationBuildBoundary : BuildPlayerProcessor
    {
        public override int callbackOrder => int.MinValue;

        public override void PrepareForBuild(BuildPlayerContext context)
        {
            EditorInitializer.SuspendForBuild();
        }
    }
}
#endif
