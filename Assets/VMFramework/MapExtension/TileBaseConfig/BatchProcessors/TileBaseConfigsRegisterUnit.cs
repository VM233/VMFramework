#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;
using VMFramework.Core;
using VMFramework.Editor.BatchProcessor;
using VMFramework.GameLogicArchitecture;
using VMFramework.Maps;

namespace VMFramework.MapExtension
{
    internal sealed class TileBaseConfigsRegisterUnit : GamePrefabRegisterUnit<TileBase>
    {
        protected override string ProcessButtonName => "Register TileBaseConfig";

        protected override IGamePrefab OnProcessAsset(TileBase tileBase)
        {
            if (checkUnique)
            {
                if (GamePrefabManager.GetAllGamePrefabs<DefaultTileBaseConfig>()
                    .Any(config => config.tile == tileBase))
                {
                    Debugger.LogWarning($"Ignore adding {tileBase.name} because it is already registered");
                    return null;
                }
            }

            var config = new DefaultTileBaseConfig()
            {
                tile = tileBase,
            };

            config.id = (tileBase.name + " " + config.IDSuffix).ToSnakeCase();
                
            return config;
        }
    }
}
#endif