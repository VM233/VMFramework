#if UNITY_EDITOR
using VMFramework.Core;
using VMFramework.Localization;

namespace VMFramework.Properties
{
    public partial class Vector3PositionProperty
    {
        public override void AutoConfigureLocalizedString(LocalizedStringAutoConfigSettings settings)
        {
            name ??= new();
            
            if (name.defaultValue.IsNullOrEmpty())
            {
                name.defaultValue = "Position";
            }
            
            base.AutoConfigureLocalizedString(settings);
        }
    }
}
#endif