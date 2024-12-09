namespace VMFramework.Configuration
{
    public partial class WeightedSelectChooserConfig<TWrapper, TItem>
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