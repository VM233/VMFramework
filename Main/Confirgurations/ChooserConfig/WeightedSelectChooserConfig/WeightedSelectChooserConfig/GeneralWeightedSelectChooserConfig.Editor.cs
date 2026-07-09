#if UNITY_EDITOR
using Sirenix.OdinInspector;

namespace VMFramework.Configuration
{
    [TypeInfoBox("Choose a value from weighted items!")]
    public partial class GeneralWeightedSelectChooserConfig<TWrapper, TItem>
    {
        protected override void OnItemsChangedGUI()
        {
            base.OnItemsChangedGUI();
            
            if (chooser != null)
            {
                chooser = GenerateThisChooser();
            }
        }
    }
}
#endif