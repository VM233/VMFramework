using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Parameters
{
    public class Vector2ParameterConverter : ParameterProviderBehaviour<Vector2>
    {
        [TitleGroup(ComponentNames.CONFIG)]
        public bool invertX = false;
        
        [TitleGroup(ComponentNames.CONFIG)]
        public bool invertY = false;

        protected override bool TryGetValue(out Vector2 result)
        {
            if (base.TryGetValue(out result) == false)
            {
                return false;
            }

            if (invertX)
            {
                result.x = -result.x;
            }

            if (invertY)
            {
                result.y = -result.y;
            }
            
            return true;
        }
    }
}