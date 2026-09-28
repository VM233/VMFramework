using VMFramework.UI;

namespace VMFramework.Editor.Tests
{
    public sealed class NativeLifetimePanel : UIPanel
    {
        public void InitializeModifier(PanelModifier modifier)
        {
            modifiers.Add(modifier);
            modifier.Initialize(this, null);
        }
    }
}
