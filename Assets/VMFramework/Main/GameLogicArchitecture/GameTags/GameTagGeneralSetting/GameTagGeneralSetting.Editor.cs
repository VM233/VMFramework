#if UNITY_EDITOR
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture 
{
    public partial class GameTagGeneralSetting
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            gameTagInfos ??= new();
        }
        
        
    }
}
#endif