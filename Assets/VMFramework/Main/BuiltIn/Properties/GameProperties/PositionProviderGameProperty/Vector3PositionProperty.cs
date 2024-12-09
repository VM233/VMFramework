using System;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Properties
{
    [GamePrefabTypeAutoRegister(ID)]
    public sealed partial class Vector3PositionProperty : GameProperty
    {
        public const string ID = "vector_3_position_property";
        
        public override Type TargetType => typeof(IVector3PositionProvider);

        public override string GetValueString(object target)
        {
            IVector3PositionProvider vector3PositionProvider = (IVector3PositionProvider)target;
            return vector3PositionProvider.Position.ToString(1);
        }
    }
}