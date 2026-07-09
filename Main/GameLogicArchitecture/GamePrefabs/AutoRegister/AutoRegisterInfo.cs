using System;

namespace VMFramework.GameLogicArchitecture
{
    public readonly struct AutoRegisterInfo
    {
        public readonly string id;
        public readonly Type gamePrefabType;
        
        public AutoRegisterInfo(string id, Type gamePrefabType)
        {
            this.id = id;
            this.gamePrefabType = gamePrefabType;
        }
    }
}