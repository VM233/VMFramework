using System;
using System.Collections.Generic;
using VMFramework.GameLogicArchitecture;
using VMFramework.Core;
using Sirenix.OdinInspector;

namespace VMFramework.Properties
{
    public sealed partial class GamePropertyGeneralSetting : GamePrefabGeneralSetting
    {
        #region Categories
        
        public const string PROPERTY_SETTING_CATEGORY = "Property";

        #endregion

        #region Metadata

        public override Type BaseGamePrefabType => typeof(GameProperty);

        #endregion

        public IEnumerable<ValueDropdownItem> GetPropertyNameList(Type targetType)
        {
            if (targetType == null)
            {
                yield break;
            }
            
            foreach (var config in GamePrefabManager.GetAllGamePrefabs<IGameProperty>())
            {
                if (config is { IsActive: true } && targetType.IsDerivedFrom(config.TargetType, true))
                {
                    yield return new(config.Name, config.id);
                }
            }
        }
    }
}
