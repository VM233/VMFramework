using VMFramework.GameLogicArchitecture;

namespace VMFramework.ResourcesManagement
{
    public partial class Effect : ControllerGameItem, IEffect
    {
        protected override void OnReturn()
        {
            base.OnReturn();
            
            transform.SetParent(EffectSpawner.Container);
        }
    }
}