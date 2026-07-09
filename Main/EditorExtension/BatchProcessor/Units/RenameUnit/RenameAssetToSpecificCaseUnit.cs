#if UNITY_EDITOR
using VMFramework.Core;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class RenameAssetToSpecificCaseUnit : DoubleButtonRenameAssetUnit
    {
        protected override string ProcessButtonOneName => "Rename To Snake Case";

        protected override string ProcessButtonTwoName => "Rename To Pascal Case";

        protected override string ProcessAssetNameOne(string oldName)
        {
            return oldName.ToSnakeCase();
        }

        protected override string ProcessAssetNameTwo(string oldName)
        {
            return oldName.ToPascalCase(" ");
        }
    }
}
#endif