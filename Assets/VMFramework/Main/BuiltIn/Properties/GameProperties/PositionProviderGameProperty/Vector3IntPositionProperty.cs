using System;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Properties
{
    [GamePrefabTypeAutoRegister(ID)]
    public sealed partial class Vector3IntPositionProperty : GameProperty
    {
        public const string ID = "vector_3_int_position_property";
        
        public override Type TargetType => typeof(IVector3IntPositionProvider);

        public override string GetValueString(object target)
        {
            IVector3IntPositionProvider vector3IntPositionProvider = (IVector3IntPositionProvider)target;
            return vector3IntPositionProvider.Position.ToString();
        }
    }
}