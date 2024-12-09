using System;
using VMFramework.GameLogicArchitecture;


namespace VMFramework.Examples 
{
    public partial class EntityConfig : LocalizedGamePrefab, IEntityConfig
    {
        public override string IDSuffix => "entity";
        
        public override Type GameItemType => typeof(Entity);
    }
}